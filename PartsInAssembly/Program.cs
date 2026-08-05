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
                        Console.Error.WriteLine(
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
                return DecodeCp866(bytes);
            }
        }

        // Ручной декодер CP866 — без пакета System.Text.Encoding.CodePages,
        // т.к. служба выполнения скриптов восстанавливает NuGet только из локального
        // офлайн-источника, внешние пакеты там недоступны.
        //
        // 0x00-0x7F — ASCII как есть.
        // 0x80-0xAF — А-Я, а-п (byte + 0x390).
        // 0xB0-0xDF — псевдографика (только таблицей).
        // 0xE0-0xEF — р-я (byte + 0x360).
        // 0xF0-0xFF — Ё/ё/Є/є/Ї/ї/Ў/ў и спецсимволы (только таблицей).
        private static readonly char[] Cp866BoxDrawingBlock =
        {
            '░', '▒', '▓', '│', '┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝', '╜', '╛', '┐',
            '└', '┴', '┬', '├', '─', '┼', '╞', '╟', '╚', '╔', '╩', '╦', '╠', '═', '╬', '╧',
            '╨', '╤', '╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█', '▄', '▌', '▐', '▀'
        };

        private static readonly char[] Cp866SpecialBlock =
        {
            'Ё', 'ё', 'Є', 'є', 'Ї', 'ї', 'Ў', 'ў', '°', '∙', '·', '√', '№', '¤', '■', ' '
        };

        private static string DecodeCp866(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[i] = b switch
                {
                    < 0x80 => (char)b,
                    < 0xB0 => (char)(b + 0x390),
                    < 0xE0 => Cp866BoxDrawingBlock[b - 0xB0],
                    < 0xF0 => (char)(b + 0x360),
                    _ => Cp866SpecialBlock[b - 0xF0]
                };
            }
            return new string(chars);
        }
    }
}
