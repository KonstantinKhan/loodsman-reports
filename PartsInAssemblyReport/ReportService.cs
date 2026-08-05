namespace PartsInAssemblyReport
{
    /// <summary>
    /// Формирует список объектов заданного типа (например, "Деталь"),
    /// найденных при обходе структуры изделия от корневого объекта на заданную глубину.
    /// </summary>
    public class ReportService
    {
        private readonly LoodsmanApiClient _apiClient;
        private readonly string _linkTypeName;
        private readonly string _targetTypeName;
        private List<ReportRow> _reportTable = new();

        public ReportService(LoodsmanApiClient apiClient, string linkTypeName, string targetTypeName)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _linkTypeName = linkTypeName;
            _targetTypeName = targetTypeName;
        }

        public async Task<List<ReportRow>> GenerateReportAsync(int rootIdVersion, int maxDepth)
        {
            _reportTable = new List<ReportRow>();

            var rootObjects = await _apiClient.GetObjectInfoAsync(rootIdVersion);
            if (rootObjects == null || !rootObjects.Any())
                throw new InvalidOperationException($"Не удалось получить информацию об объекте с ID {rootIdVersion}");

            var rootObject = rootObjects.First();

            await TraverseTreeAsync(rootObject, 1, maxDepth);
            await EnrichReportDataAsync();

            return _reportTable.OrderBy(x => x.Product).ToList();
        }

        private async Task TraverseTreeAsync(ObjectInfo currentObject, int currentLevel, int maxDepth)
        {
            if (string.Equals(currentObject.type, _targetTypeName, StringComparison.Ordinal))
            {
                _reportTable.Add(new ReportRow
                {
                    IdLink = currentObject.idLink,
                    IdVersion = currentObject.idVersion,
                    Type = currentObject.type ?? string.Empty,
                    Product = currentObject.product ?? string.Empty,
                    VersionNumber = currentObject.version ?? string.Empty
                });
            }

            if (currentLevel >= maxDepth)
                return;

            List<ObjectInfo> children;
            try
            {
                children = await _apiClient.GetLinkedObjectsAsync(currentObject.idVersion, _linkTypeName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Предупреждение: не удалось получить дочерние объекты для версии {currentObject.idVersion}: {ex.Message}");
                return;
            }

            if (children == null || children.Count == 0)
                return;

            foreach (var child in children)
            {
                await TraverseTreeAsync(child, currentLevel + 1, maxDepth);
            }
        }

        private async Task EnrichReportDataAsync()
        {
            foreach (var row in _reportTable)
            {
                try
                {
                    var attributes = await _apiClient.GetVersionAttributesAsync(row.IdVersion);
                    row.Name = attributes.FirstOrDefault(x => x.name == "Наименование")?.value ?? string.Empty;
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Предупреждение: не удалось получить атрибуты версии {row.IdVersion}: {ex.Message}");
                }
            }
        }
    }
}
