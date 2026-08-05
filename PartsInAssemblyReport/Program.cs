using System.Text;

namespace PartsInAssemblyReport
{
    internal class Program
    {
        private const string DefaultLinkTypeName = "Состоит из ...";
        private const string DefaultTargetTypeName = "Деталь";

        static async Task Main(string[] args)
        {
            // Явно фиксируем UTF-8 для вывода — иначе кириллица в JSON-результате
            // может побиться так же, как и во входных данных
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                // Читаем stdin явно как UTF-8, не полагаясь на кодовую страницу консоли
                // (Console.In при пайпе на Windows/PowerShell может декодировать кириллицу неверно)
                string userData;
                using (var reader = new StreamReader(Console.OpenStandardInput(), Encoding.UTF8))
                {
                    userData = reader.ReadToEnd();
                }

                var config = AppConfiguration.GetConfiguration(args, userData);

                if (config.ObjectIds.Count == 0)
                    throw new InvalidOperationException("Не указан идентификатор объекта \"Сборочная единица\" (object_ids)");

                var depthStr = config.GetStringParameterByName("Глубина разузловки");
                if (!int.TryParse(depthStr, out var maxDepth) || maxDepth < 1)
                {
                    var foundKeys = config.Params.Count == 0
                        ? "(params пустой — либо не пришёл userData, либо не распарсился JSON)"
                        : string.Join(", ", config.Params.Keys);
                    throw new InvalidOperationException(
                        $"Параметр \"Глубина разузловки\" не задан или некорректен. Найденные ключи params: {foundKeys}");
                }

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
