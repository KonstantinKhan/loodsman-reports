namespace PartsInAssemblyReport
{
    internal class Program
    {
        private const string DefaultLinkTypeName = "Состоит из ...";
        private const string DefaultTargetTypeName = "Деталь";

        static async Task Main(string[] args)
        {
            try
            {
                // Заполняем конфигурацию из потока ввода и аргументов командной строки
                var config = AppConfiguration.GetConfiguration(args, Console.In.ReadToEnd());

                if (config.ObjectIds.Count == 0)
                    throw new InvalidOperationException("Не указан идентификатор объекта \"Сборочная единица\" (object_ids)");

                var depthStr = config.GetStringParameterByName("Глубина разузловки");
                if (!int.TryParse(depthStr, out var maxDepth) || maxDepth < 1)
                    throw new InvalidOperationException("Параметр \"Глубина разузловки\" не задан или некорректен");

                var linkTypeName = config.GetStringParameterByName("Тип связи");
                if (string.IsNullOrWhiteSpace(linkTypeName))
                    linkTypeName = DefaultLinkTypeName;

                var targetTypeName = config.GetStringParameterByName("Тип для отбора");
                if (string.IsNullOrWhiteSpace(targetTypeName))
                    targetTypeName = DefaultTargetTypeName;

                using var apiClient = new LoodsmanApiClient(config);
                var reportService = new ReportService(apiClient, linkTypeName, targetTypeName);

                var report = await reportService.GenerateReportAsync(config.ObjectIds.First(), maxDepth);

                ReportPrinter.PrintJson(report);
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Ошибка при генерации отчета: {ex.Message}");
                Console.ResetColor();

                if (ex.InnerException != null)
                    Console.Error.WriteLine($"Детали: {ex.InnerException.Message}");

                Environment.Exit(1);
            }
        }
    }
}
