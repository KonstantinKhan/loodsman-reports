using System.Text;

namespace CSharp
{
    internal class Program
    {
        private const string DefaultLinkTypeName = "Состоит из ...";
        private const string DefaultTargetTypeName = "Деталь";
        private const int DefaultMaxDepth = 5;

        static async Task Main(string[] args)
        {
            // Регистрируем провайдер codepages — без него Encoding.GetEncoding(1251) кидает исключение
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            // Явно фиксируем UTF-8 для вывода — вывод (JSON-результат) мы формируем сами,
            // поэтому кодировка на выходе под контролем
            Console.OutputEncoding = Encoding.UTF8;

            try
            {
                var userData = ReadUserDataFromStdin();
                var config = AppConfiguration.GetConfiguration(args, userData);

                if (config.ObjectIds.Count == 0)
                    throw new InvalidOperationException("Не указан идентификатор объекта \"Сборочная единица\" (object_ids)");

                var depthStr = config.GetStringParameterByName("Глубина разузловки");
                int maxDepth;
                if (!int.TryParse(depthStr, out maxDepth) || maxDepth < 1)
                {
                    var foundKeys = config.Params.Count == 0
                        ? "(params пустой — параметр \"Глубина разузловки\", вероятно, не настроен в регистрации отчёта в Конфигураторе)"
                        : string.Join(", ", config.Params.Keys);
                    Console.Error.WriteLine(
                        $"Предупреждение: параметр \"Глубина разузловки\" не задан или некорректен, использую значение по умолчанию {DefaultMaxDepth}. Найденные ключи params: {foundKeys}");
                    maxDepth = DefaultMaxDepth;
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
                {
                    Console.Error.WriteLine($"Детали: {ex.InnerException.Message}");
                }

                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Читает userData из stdin. Служба выполнения скриптов на Windows не всегда
        /// передаёт данные в UTF-8 — пробуем UTF-8 (строго), при ошибке — Windows-1251.
        /// </summary>
        private static string ReadUserDataFromStdin()
        {
            byte[] bytes;
            using (var stdin = Console.OpenStandardInput())
            using (var buffer = new MemoryStream())
            {
                stdin.CopyTo(buffer);
                bytes = buffer.ToArray();
            }

            if (bytes.Length == 0)
                return string.Empty;

            try
            {
                var strictUtf8 = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
                return strictUtf8.GetString(bytes);
            }
            catch (DecoderFallbackException)
            {
                return Encoding.GetEncoding(1251).GetString(bytes);
            }
        }
    }
}