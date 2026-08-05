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
                    if (config.Params.Count == 0)
                    {
                        Cp866.WriteLine(Console.OpenStandardError(),
                            $"Предупреждение: params пустой — параметр \"Глубина разузловки\", вероятно, не настроен в регистрации отчёта в Конфигураторе. Использую значение по умолчанию {DefaultMaxDepth}.");
                    }
                    else
                    {
                        var foundKeys = string.Join(", ", config.Params.Keys);
                        Cp866.WriteLine(Console.OpenStandardError(),
                            $"Предупреждение: параметр \"Глубина разузловки\" не задан или некорректен, использую значение по умолчанию {DefaultMaxDepth}. Найденные ключи params: {foundKeys}");
                    }
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
                Cp866.WriteLine(Console.OpenStandardError(), $"Ошибка при генерации отчета: {ex.Message}");

                if (ex.InnerException != null)
                {
                    Cp866.WriteLine(Console.OpenStandardError(), $"Детали: {ex.InnerException.Message}");
                }

                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Читает userData из stdin. Служба выполнения скриптов на Windows передаёт
        /// данные не в UTF-8, а в OEM-кодировке консоли (CP866 на русской Windows) —
        /// пробуем UTF-8 (строго), при ошибке — CP866.
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
                return Cp866.Decode(bytes);
            }
        }
    }
}
