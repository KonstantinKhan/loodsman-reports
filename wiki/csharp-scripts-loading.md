# Получение адреса СП, идентификатора сессии и пользовательских данных

## Аргументы командной строки

Для исполняемых типов приложений служба выполнения скриптов, в общем случае, передаёт два аргумента:

- `-a` — адрес сервера приложений;
- `--session` — идентификатор пользовательской сессии.

## Пользовательские данные (userData)

В рамках работы с системой отчётов пользовательские данные передаются в **стандартный поток ввода** (stdin) в формате JSON:

```json
{
  "object_ids": [1012, 2, 3],
  "params": {
    "Accuracy": "2",
    "Depth": "3",
    "LinkType": "Состоит из ..."
  },
  "conf_rules": {
    "rule_id": 10,
    "final_product_id": 200,
    "path": [5, 6, 7],
    "rule_params": [
      { "param_name": "Color", "param_type": 1, "param_value": "Red", "is_any": false },
      { "param_name": "Size", "param_type": 2, "param_value": "XL", "is_any": true }
    ],
    "fixed_context_id": 99
  }
}
```

- `object_ids` — список идентификаторов объектов, по которым нужно сформировать отчёт;
- `params` — словарь параметров, определённых администратором при регистрации отчёта, со значениями, заполненными пользователем при запуске (имена параметров произвольные, конкретный набор зависит от отчёта);
- `conf_rules` — правила версионного конфигурирования.

Служба выполнения скриптов запускает приложение примерно так:

```text
./ExactProductStructureReport -a http://host:port --session 4e19c13d-cab0-467b-8776-423eaee61f2a
```

При этом через stdin передаётся JSON с пользовательскими данными.

## Класс AppConfiguration

Для работы с входными аргументами и userData используется универсальный класс `AppConfiguration` (см. [[csharp-scripts-structure.md]]). Финальный код:

```csharp
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ExactProductStructureReport
{
    public class AppConfiguration
    {
        public string ApiVersion { get; set; } = "4";
        public int RequestTimeoutSeconds { get; set; } = 60;
        public string AppServerHost { get; set; } = "http://localhost:8076";
        public string SessionId { get; set; }

        [JsonPropertyName("object_ids")]
        public List<int> ObjectIds { get; set; } = new();

        [JsonPropertyName("params")]
        public Dictionary<string, object?> Params { get; set; } = new();

        [JsonPropertyName("conf_rules")]
        public ConfRules? ConfRules { get; set; }

        public string GetStringParameterByName(string parameterName)
        {
            if (Params.TryGetValue(parameterName, out object? parameterValue))
                return parameterValue?.ToString() ?? "";

            return "";
        }

        public static AppConfiguration GetConfiguration(string[] arguments, string? userData)
        {
            try
            {
                var config = DeserializeFromJson(userData);
                ApplyCommandLineArguments(config, arguments);
                return config;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при разборе конфигурации приложения", ex);
            }
        }

        private static AppConfiguration DeserializeFromJson(string? userData)
        {
            if (string.IsNullOrWhiteSpace(userData))
                return new AppConfiguration();

            var json = userData.TrimStart('\ufeff').Trim();

            return JsonSerializer.Deserialize<AppConfiguration>(json)
                   ?? throw new InvalidOperationException("JSON конфигурации пустой или некорректный");
        }

        private static void ApplyCommandLineArguments(AppConfiguration config, string[] arguments)
        {
            if (TryGetArgumentValue(arguments, "-a", out var host))
                config.AppServerHost = host;

            if (TryGetArgumentValue(arguments, "--session", out var sessionId))
                config.SessionId = sessionId;
        }

        private static bool TryGetArgumentValue(string[] arguments, string argumentKey, out string value)
        {
            value = string.Empty;

            for (int i = 0; i < arguments.Length - 1; i++)
            {
                if (string.Equals(arguments[i], argumentKey, StringComparison.OrdinalIgnoreCase))
                {
                    value = arguments[i + 1];
                    return true;
                }
            }

            return false;
        }
    }

    public class ConfRules
    {
        [JsonPropertyName("rule_id")]
        public int RuleId { get; set; }

        [JsonPropertyName("final_product_id")]
        public int FinalProductId { get; set; }

        [JsonPropertyName("path")]
        public List<int> Path { get; set; } = new();

        [JsonPropertyName("rule_params")]
        public List<RuleParam> RuleParams { get; set; } = new();

        [JsonPropertyName("fixed_context_id")]
        public int FixedContextId { get; set; }
    }

    public class RuleParam
    {
        [JsonPropertyName("param_name")]
        public string ParamName { get; set; } = string.Empty;

        [JsonPropertyName("param_type")]
        public int ParamType { get; set; }

        [JsonPropertyName("param_value")]
        public string ParamValue { get; set; } = string.Empty;

        [JsonPropertyName("is_any")]
        public bool IsAny { get; set; }
    }
}
```

> `ApiVersion` по умолчанию — `"4"` (актуальная версия REST API, см. `swagger.lapis`).

## Кодировка stdin/stdout/stderr (частая проблема на Windows)

Служба выполнения скриптов на Windows **не использует UTF-8** ни на входе, ни на выходе дочернего процесса — она читает и пишет через OEM-кодировку консоли, на русской Windows это **CP866** (не Windows-1251!). Это касается **всех** потоков:

- `userData` приходит в stdin в CP866;
- всё, что скрипт печатает в stdout (сам JSON-результат отчёта) и в stderr (диагностика/ошибки), служба читает обратно тоже как CP866 — если писать обычным `Console.WriteLine`/`Console.Error.WriteLine` (даже с `Console.OutputEncoding = Encoding.UTF8`), кириллица в результате и в логах будет битой на стороне, где это прочитают.

Если декодировать/кодировать через UTF-8 напрямую — decoder либо упадёт на невалидных байтах, либо (при попытке декодировать "на глаз" другой кодировкой типа CP1251) даст читаемую на вид, но неверную кириллицу.

> **Как проверяли**: при отладке реального стенда предупреждение с "найденными ключами params" выводило кракозябры даже после попытки декодировать как CP1251. Чтобы не гадать по консольному выводу (он сам по себе может смешивать несколько слоёв неправильного перекодирования при отображении), в лог временно добавляли `Convert.ToBase64String(Encoding.UTF8.GetBytes(...))` — base64 нельзя испортить консольным рендерингом. Декодировали base64 → получили сырые UTF-8-байты строки → перебором кодировок нашли, что `bytes.Decode("cp866")` даёт ожидаемый `"Глубина разузловки"`. Тот же приём (закодировать подозрительную строку в исходную кодировку и раскодировать как UTF-8/наоборот, перебором) применили и для диагностики самого JSON-вывода. Если снова столкнётесь с похожей проблемой — используйте тот же приём (base64 в диагностику + перебор кодировок), не пытайтесь распознать кодировку по консольному выводу глазами.

Рабочий вариант — **и на чтение, и на запись** работать с сырыми байтами через общий кодек CP866, а не через `Console.In`/`Console.Out`/`Console.Error` напрямую.

> **Важно**: `Encoding.GetEncoding(866)` в .NET (Core/5+) штатно требует пакет
> `System.Text.Encoding.CodePages` и регистрацию `CodePagesEncodingProvider`.
> **Не используйте этот пакет в скриптах для службы выполнения скриптов** —
> она восстанавливает NuGet-зависимости только из своего локального офлайн-источника
> (`...\GlobalScriptService\data\uploads\<guid>\nuget-packages`), внешнего доступа
> в интернет для `dotnet restore` там нет. Любой внешний `PackageReference` даёт
> `NU1301: Локальный источник ... не существует`, и сборка на сервере падает — даже
> если она проходила локально на машине разработчика с доступом в NuGet.org.
>
> Вместо пакета — кодировать/декодировать CP866 вручную явными таблицами
> (self-contained, без внешних зависимостей): `0x00-0x7F` — ASCII как есть,
> `0x80-0xAF` — А-Я/а-п одним смещением (`byte + 0x390`), `0xE0-0xEF` — р-я
> смещением (`byte + 0x360`), `0xB0-0xDF` — псевдографика и `0xF0-0xFF` —
> Ё/ё/Є/є/Ї/ї/Ў/ў и спецсимволы — только явными таблицами из стандарта CP866.
> На запись — обратные таблицы (unicode → byte), непредставимые в CP866 символы
> заменяются на `?`.

Удобно вынести кодек в отдельный файл `Cp866.cs` и использовать его *везде*, где текст с кириллицей уходит в консоль — не только для чтения `userData`, но и для `ReportPrinter` (вывод JSON) и любых диагностических `Console.Error.WriteLine` по коду:

```csharp
namespace CSharp
{
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
            'Ё', 'ё', 'Є', 'є', 'Ї', 'ї', 'Ў', 'ў', '°', '∙', '·', '√', '№', '¤', '■', ' '
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
```

Использование при чтении `userData`:

```csharp
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
        var strictUtf8 = new System.Text.UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true);
        return strictUtf8.GetString(bytes);
    }
    catch (System.Text.DecoderFallbackException)
    {
        return Cp866.Decode(bytes);
    }
}
```

Использование при выводе результата (вместо `Console.WriteLine`):

```csharp
Cp866.WriteLine(Console.OpenStandardOutput(), prettyJson);
```

И при выводе диагностики (вместо `Console.Error.WriteLine`):

```csharp
Cp866.WriteLine(Console.OpenStandardError(), $"Предупреждение: ...");
```

## Связанные узлы

- [[csharp-scripts-references.md]] — зависимости
- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-best-practices.md]] — best practices
