namespace CSharp
{
    /// <summary>
    /// Ручной кодек CP866 (без пакета System.Text.Encoding.CodePages — служба выполнения
    /// скриптов восстанавливает NuGet только из локального офлайн-источника, внешние
    /// пакеты там недоступны).
    ///
    /// Служба выполнения скриптов на Windows читает и пишет stdin/stdout/stderr дочернего
    /// процесса в OEM-кодировке консоли (CP866 на русской Windows), а не в UTF-8 — поэтому
    /// весь текст с кириллицей нужно и читать, и писать именно через этот кодек, а не через
    /// обычные Console.Write*/Console.OutputEncoding.
    ///
    /// 0x00-0x7F — ASCII как есть.
    /// 0x80-0xAF — А-Я, а-п (byte + 0x390).
    /// 0xB0-0xDF — псевдографика (только таблицей).
    /// 0xE0-0xEF — р-я (byte + 0x360).
    /// 0xF0-0xFF — Ё/ё/Є/є/Ї/ї/Ў/ў и спецсимволы (только таблицей).
    /// </summary>
    internal static class Cp866
    {
        private static readonly char[] BoxDrawingBlock =
        {
            '░', '▒', '▓', '│', '┤', '╡', '╢', '╖', '╕', '╣', '║', '╗', '╝', '╜', '╛', '┐',
            '└', '┴', '┬', '├', '─', '┼', '╞', '╟', '╚', '╔', '╩', '╦', '╠', '═', '╬', '╧',
            '╨', '╤', '╥', '╙', '╘', '╒', '╓', '╫', '╪', '┘', '┌', '█', '▄', '▌', '▐', '▀'
        };

        private static readonly char[] SpecialBlock =
        {
            'Ё', 'ё', 'Є', 'є', 'Ї', 'ї', 'Ў', 'ў', '°', '∙', '·', '√', '№', '¤', '■', ' '
        };

        private static readonly Dictionary<char, byte> EncodeMap = BuildEncodeMap();

        private static Dictionary<char, byte> BuildEncodeMap()
        {
            var map = new Dictionary<char, byte>();

            for (int b = 0x80; b < 0xB0; b++)
                map[(char)(b + 0x390)] = (byte)b;

            for (int b = 0xE0; b < 0xF0; b++)
                map[(char)(b + 0x360)] = (byte)b;

            for (int i = 0; i < BoxDrawingBlock.Length; i++)
                map[BoxDrawingBlock[i]] = (byte)(0xB0 + i);

            for (int i = 0; i < SpecialBlock.Length; i++)
                map[SpecialBlock[i]] = (byte)(0xF0 + i);

            return map;
        }

        public static string Decode(byte[] bytes)
        {
            var chars = new char[bytes.Length];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[i] = b switch
                {
                    < 0x80 => (char)b,
                    < 0xB0 => (char)(b + 0x390),
                    < 0xE0 => BoxDrawingBlock[b - 0xB0],
                    < 0xF0 => (char)(b + 0x360),
                    _ => SpecialBlock[b - 0xF0]
                };
            }
            return new string(chars);
        }

        public static byte[] Encode(string text)
        {
            var bytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                bytes[i] = c < 0x80
                    ? (byte)c
                    : (EncodeMap.TryGetValue(c, out var b) ? b : (byte)'?');
            }
            return bytes;
        }

        public static void WriteLine(Stream stream, string text)
        {
            var bytes = Encode(text + "\n");
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
        }
    }
}
