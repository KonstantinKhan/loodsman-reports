# Основная программа, вывод результата и локальное тестирование

## Program.cs — сбор и возврат данных

Точка входа собирает все компоненты, запускает генерацию отчёта и выводит результат:

```csharp
namespace ExactProductStructureReport
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Заполняем конфигурацию из потока ввода и аргументов
                var config = AppConfiguration.GetConfiguration(args, Console.In.ReadToEnd());

                // Инициализируем API-клиент
                var apiClient = new LoodsmanApiClient(config);

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
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Error.WriteLine($"Ошибка при генерации отчета: {ex.Message}");
                Console.ResetColor();

                if (ex.InnerException != null)
                    Console.Error.WriteLine($"Детали: {ex.InnerException.Message}");
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
                Console.WriteLine("Нет данных для отображения");
                return;
            }

            var itemsList = items.ToList();
            if (itemsList.Count == 0)
            {
                Console.WriteLine("Нет данных для отображения");
                return;
            }

            var prettyJson = JsonSerializer.Serialize(items, _options);
            Console.WriteLine(prettyJson);
        }
    }
}
```

Опции сериализации необязательны, но без них JSON выводится в одну строку с экранированием — форматированный вывод удобнее при ручной проверке результата.

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
3. Запустить приложение через PowerShell, передав `userdata.json` в stdin:
   ```powershell
   Get-Content userdata.json | .\ExactProductStructureReport.exe -a http://localhost:8076 --session <sessionId>
   ```
4. В выводе должен появиться JSON-список объектов с нужными свойствами — значит всё работает правильно, можно переходить к разработке шаблона и регистрации отчёта.

### Локальный запуск из WSL (альтернатива PowerShell)

PowerShell на Windows по умолчанию использует не-UTF8 кодовую страницу консоли, из-за чего кириллица в `userdata.json` может побиться ещё на этапе передачи в stdin приложения (отдельно от проблемы кодировки на стороне службы выполнения скриптов, см. [[csharp-scripts-loading.md]]). Терминал WSL по умолчанию UTF-8, поэтому для локальной отладки может быть удобнее собирать и запускать проект прямо там:

```bash
# Установка .NET SDK 8 в WSL (Ubuntu/Debian), один раз
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh --channel 8.0
export PATH="$HOME/.dotnet:$PATH"   # добавить в ~/.bashrc, чтобы не повторять каждый раз

# Если проект лежит на Windows-диске — он доступен из WSL по /mnt/<буква диска>/...
cd /mnt/c/Users/<user>/путь/до/проекта

dotnet build
cat userdata.json | dotnet run -- -a http://<host>:<port> --session <sessionId>
```

WSL2 обычно имеет сетевой доступ к тем же хостам, что и Windows, так что обращение к реальному серверу приложений ЛОЦМАН:PLM работает без дополнительной настройки.

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-automation.md]] — сбор данных
- [[report-template-binding.md]] — привязка результата к шаблону FastReport
