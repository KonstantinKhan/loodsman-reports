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
                    if (config.Params.Count == 0)
                    {
                        Console.Error.WriteLine(
                            $"Предупреждение: params пустой — параметр \"Глубина разузловки\", вероятно, не настроен в регистрации отчёта в Конфигураторе. Использую значение по умолчанию {DefaultMaxDepth}.");
                    }
                    else
                    {
                        var foundKeys = string.Join(", ", config.Params.Keys);
                        // base64 от UTF-8-байт — чтобы диагностировать реальное содержимое ключей
                        // независимо от того, как консоль отрендерит кириллицу
                        var foundKeysBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes(foundKeys));
                        Console.Error.WriteLine(
                            $"Предупреждение: параметр \"Глубина разузловки\" не задан или некорректен, использую значение по умолчанию {DefaultMaxDepth}. Найденные ключи params: {foundKeys} [base64: {foundKeysBase64}]");
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
                return DecodeWindows1251(bytes);
            }
        }

        // Ручной декодер Windows-1251 — без пакета System.Text.Encoding.CodePages,
        // т.к. служба выполнения скриптов восстанавливает NuGet только из локального
        // офлайн-источника, внешние пакеты там недоступны.
        //
        // 0x00-0x7F — ASCII как есть.
        // 0xC0-0xFF — кириллица А-Я,а-я одним диапазоном (byte + 0x350).
        // 0x80-0xBF — разрозненные символы (кавычки, Ё/ё, №, спецсимволы) — только таблицей.
        private static readonly char[] Cp1251SpecialBlock =
        {
            'Ђ', 'Ѓ', '‚', 'ѓ', '„', '…', '†', '‡',
            '€', '‰', 'Љ', '‹', 'Њ', 'Ќ', 'Ћ', 'Џ',
            'ђ', '‘', '’', '“', '”', '•', '–', '—',
            '?',      '™', 'љ', '›', 'њ', 'ќ', 'ћ', 'џ',
            ' ', 'Ў', 'ў', 'Ј', '¤', 'Ґ', '¦', '§',
            'Ё', '©', 'Є', '«', '¬', '­', '®', 'Ї',
            '°', '±', 'І', 'і', 'ґ', 'µ', '¶', '·',
            'ё', '№', 'є', '»', 'ј', 'Ѕ', 'ѕ', 'ї'
        };

        private static string DecodeWindows1251(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[i] = b switch
                {
                    < 0x80 => (char)b,
                    < 0xC0 => Cp1251SpecialBlock[b - 0x80],
                    _ => (char)(b + 0x350)
                };
            }
            return new string(chars);
        }
    }
}