# Основная программа, вывод результата и локальное тестирование

> **Перед копированием этого раздела** прочитайте [[csharp-scripts-loading.md]] →
> "Кодировка stdin/stdout/stderr". Служба выполнения скриптов на Windows общается
> с процессом не в UTF-8, а в CP866 — обычные `Console.In.ReadToEnd()` и
> `Console.WriteLine`/`Console.Error.WriteLine` **ломают кириллицу и на входе, и
> на выходе**. Ниже — уже рабочий вариант через кодек `Cp866`.

## Program.cs — сбор и возврат данных

Точка входа собирает все компоненты, запускает генерацию отчёта и выводит результат:

```csharp
using System.Text;

namespace ExactProductStructureReport
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Читаем userData через кодек Cp866 (см. csharp-scripts-loading.md),
                // а не через Console.In.ReadToEnd()
                var userData = ReadUserDataFromStdin();
                var config = AppConfiguration.GetConfiguration(args, userData);

                // Инициализируем API-клиент
                using var apiClient = new LoodsmanApiClient(config);

                // Инициализируем сервис сбора данных
                var reportService = new ReportService(apiClient, config);

                // Генерируем список строк отчёта
                var report = await reportService.GenerateReportAsync(
                    config.ObjectIds.First(),
                    int.Parse(config.GetStringParameterByName("Глубина разузловки")));

                ReportPrinter.PrintJson(report);
            }
            catch (Exception ex)
            {
                Cp866.WriteLine(Console.OpenStandardError(), $"Ошибка при генерации отчета: {ex.Message}");

                if (ex.InnerException != null)
                    Cp866.WriteLine(Console.OpenStandardError(), $"Детали: {ex.InnerException.Message}");

                Environment.Exit(1);
            }
        }

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
```

## ReportPrinter.cs — вывод в JSON

```csharp
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ExactProductStructureReport
{
    public static class ReportPrinter
    {
        private static readonly JsonSerializerOptions _options = new()
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

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
```

Опции сериализации необязательны, но без них JSON выводится в одну строку с экранированием — форматированный вывод удобнее при ручной проверке результата. Сам вывод — через `Cp866.WriteLine`, не `Console.WriteLine` (см. предупреждение в начале раздела).

## Проверка написанного приложения

Проверить работоспособность удобнее всего загрузив скрипт в редактор, встроенный в веб-приложение "ЛОЦМАН-Конфигуратор" (см. [[report-template-binding.md]]). Но возможна и локальная проверка — потребуется:

- адрес сервера приложений ЛОЦМАН (например, `http://localhost:8076`);
- идентификатор активной сессии (получить в Swagger через `/api/v4/Auth/login`, см. [[csharp-scripts-prototyping.md]]);
- входные параметры отчёта (`userdata.json`).

1. Собрать приложение (`dotnet build` или сборка в Visual Studio) — в логе сборки будет путь к бинарным файлам.
2. В папке со скомпилированным приложением создать `userdata.json`, например:
   ```json
   {
     "object_ids": [1012],
     "params": {
       "Глубина разузловки": "3",
       "Тип связи": "Состоит из ..."
     }
   }
   ```
3. Запустить приложение, передав `userdata.json` в stdin — PowerShell:
   ```powershell
   Get-Content userdata.json | .\ExactProductStructureReport.exe -a http://localhost:8076 --session <sessionId>
   ```
   или из WSL/bash:
   ```bash
   cat userdata.json | dotnet run -- -a http://localhost:8076 --session <sessionId>
   ```
   Раз декодер в `Program.cs` сам определяет UTF-8/CP866 (см. [[csharp-scripts-loading.md]]), локально можно пользоваться любым терминалом — PowerShell, cmd или WSL — результат должен быть одинаково корректным.
4. В выводе должен появиться JSON-список объектов с нужными свойствами (кириллица должна отображаться нормально) — значит всё работает правильно, можно переходить к разработке шаблона и регистрации отчёта.

### Если сервер приложений на этой же машине, а тестируете из WSL

`localhost` внутри WSL2 не всегда указывает на Windows-хост. Если получаете
`Connection refused`/таймаут при обращении на `localhost`, а сервер точно
поднят на этой же машине:

1. Проверьте, что сервер слушает не только `127.0.0.1`, а все интерфейсы:
   ```powershell
   netstat -ano | findstr :8076
   ```
   Строка вида `0.0.0.0:8076 ... LISTENING` — ок. Если видно только
   `127.0.0.1:8076` — сервер принимает соединения только с себя, из WSL не
   достучаться в принципе, дело не в файрволе.
2. Если слушает `0.0.0.0` — скорее всего блокирует файрвол Windows. Либо
   открыть порт для подсети WSL, либо (проще) тестировать прямо из PowerShell/cmd
   на этой же машине, не из WSL — раз декодер сам умеет и UTF-8, и CP866,
   отдельно WSL ради корректной кириллицы больше не нужен (см. выше).

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-automation.md]] — сбор данных
- [[csharp-scripts-loading.md]] — кодировка stdin/stdout/stderr (Cp866)
- [[report-template-binding.md]] — привязка результата к шаблону FastReport
