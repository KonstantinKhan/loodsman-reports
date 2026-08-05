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

## Кодировка stdin (частая проблема на Windows)

Служба выполнения скриптов на Windows **не всегда** передаёт `userData` в UTF-8 — на практике встречается Windows-1251. Если декодировать stdin как UTF-8 напрямую, кириллица в ключах `params` побьётся (символы заменятся на replacement character), и `GetStringParameterByName` не найдёт нужный параметр, хотя в логах он как будто есть.

Рабочий вариант — читать stdin как сырые байты и пробовать декодировать сначала строго как UTF-8, при ошибке — как Windows-1251.

> **Важно**: `Encoding.GetEncoding(1251)` в .NET (Core/5+) штатно требует пакет
> `System.Text.Encoding.CodePages` и регистрацию `CodePagesEncodingProvider`.
> **Не используйте этот пакет в скриптах для службы выполнения скриптов** —
> она восстанавливает NuGet-зависимости только из своего локального офлайн-источника
> (`...\GlobalScriptService\data\uploads\<guid>\nuget-packages`), внешнего доступа
> в интернет для `dotnet restore` там нет. Любой внешний `PackageReference` даёт
> `NU1301: Локальный источник ... не существует`, и сборка на сервере падает — даже
> если она проходила локально на машине разработчика с доступом в NuGet.org.
>
> Вместо пакета — декодировать Windows-1251 вручную явной таблицей (self-contained,
> без внешних зависимостей). Диапазон `0x00-0x7F` — ASCII как есть, `0xC0-0xFF` —
> кириллица А-Я/а-я одним смещением (`byte + 0x350`), `0x80-0xBF` — набор
> разрозненных символов (кавычки, Ё/ё, №, тире и т.п.), которые можно взять только
> явной таблицей из стандарта CP1251.

```csharp
using System.Text;

static string ReadUserDataFromStdin()
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

// 0x80-0xBF из таблицы Windows-1251 (Unicode.org CP1251.TXT)
private static readonly char[] Cp1251SpecialBlock =
{
    'Ђ', 'Ѓ', '‚', 'ѓ', '„', '…', '†', '‡',
    '€', '‰', 'Љ', '‹', 'Њ', 'Ќ', 'Ћ', 'Џ',
    'ђ', '‘', '’', '“', '”', '•', '–', '—',
    '?',      '™', 'љ', '›', 'њ', 'ќ', 'ћ', 'џ',
    ' ', 'Ў', 'ў', 'Ј', '¤', 'Ґ', '¦', '§',
    'Ё', '©', 'Є', '«', '¬', '­', '®', 'Ї',
    '°', '±', 'І', 'і', 'ґ', 'µ', '¶', '·',
    'ё', '№', 'є', '»', 'ј', 'Ѕ', 'ѕ', 'ї'
};

static string DecodeWindows1251(byte[] bytes)
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
```

Вывод (JSON-результат в stdout) формируется приложением самостоятельно — там достаточно `Console.OutputEncoding = Encoding.UTF8;` в начале `Main`, отдельного автоопределения не требуется.

## Связанные узлы

- [[csharp-scripts-references.md]] — зависимости
- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-best-practices.md]] — best practices
