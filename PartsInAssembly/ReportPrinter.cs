using System.Text.Encodings.Web;
using System.Text.Json;

namespace CSharp
{
    /// <summary>
    /// Утилита для печати табличных данных в консоль
    /// </summary>
    public static class ReportPrinter
    {
        /// <summary>
        /// Настройки форматирования JSON. Не рекомендуется изменять без необходимости.
        /// </summary>
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        /// <summary>
        /// Печать коллекции объектов в виде JSON. Пишем байты сами через Cp866 —
        /// служба выполнения скриптов читает stdout дочернего процесса не в UTF-8,
        /// а в OEM-кодировке консоли (CP866 на русской Windows), см. Cp866.cs.
        /// </summary>
        public static void PrintJson<T>(IEnumerable<T> items)
        {
            if (items == null)
            {
                Cp866.WriteLine(Console.OpenStandardOutput(), "Нет данных для отображения");
                return;
            }

            var itemsList = items.ToList();
            if (itemsList.Count == 0)
            {
                Cp866.WriteLine(Console.OpenStandardOutput(), "Нет данных для отображения");
                return;
            }

            var prettyJson = JsonSerializer.Serialize(items, _options);

            Cp866.WriteLine(Console.OpenStandardOutput(), prettyJson);
        }
    }
}
