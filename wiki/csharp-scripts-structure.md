# Структура и служебные классы шаблона

Архитектура и ключевые компоненты шаблона скрипта для ЛОЦМАН:PLM. Понимание структуры этих классов необходимо для разработки новых отчётов.

## AppConfiguration.cs

Парсинг и хранение всей конфигурации, поступающей в скрипт:

- адрес сервера приложений;
- список идентификаторов объектов, по которым формируется отчёт;
- параметры формирования отчёта;
- правила ревизионного конфигурирования;
- прочие параметры.

**Класс `AppConfiguration`** — центральное хранилище конфигурации. Содержит свойства, полученные из `userData` (JSON), и переопределённые аргументами командной строки.

Ключевые свойства:

- `ApiVersion` (string) — версия API сервера приложений;
- `RequestTimeoutSeconds` (int) — таймаут HTTP-запросов, по умолчанию 60;
- `AppServerHost` (string) — адрес сервера приложений, по умолчанию `http://localhost:8076`, переопределяется аргументом `-a`;
- `SessionId` (string) — идентификатор сессии пользователя, переопределяется аргументом `--session`;
- `ObjectIds` (`List<int>`) — идентификаторы объектов отчёта, из поля `object_ids`;
- `Params` (`Dictionary<string, object?>`) — пользовательские параметры отчёта, из поля `params`;
- `ConfRules` (`ConfRules?`) — правила версионного конфигурирования, из поля `conf_rules`.

Ключевые методы:

- `GetStringParameterByName(string)` — безопасное получение строкового значения из `Params`, `string.Empty` если ключ не найден;
- `GetConfiguration(string[] arguments, string? userData)` — точка входа инициализации: `DeserializeFromJson` → `ApplyCommandLineArguments`;
- `DeserializeFromJson(string? userData)` — десериализация `userData`, обработка BOM и пустых строк;
- `ApplyCommandLineArguments(config, arguments)` — извлечение `-a`/`--session` и заполнение соответствующих свойств;
- `TryGetArgumentValue(...)` — поиск значения конкретного аргумента в массиве.

Вложенные классы `ConfRules` и `RuleParam` хранят правила конфигурирования из `userData`.

Подробнее о формате входных данных — [[csharp-scripts-loading.md]].

## LoodsmanApiClient.cs (AppClient.cs)

Инкапсулирует взаимодействие с WebAPI сервера приложений через `HttpClient`.

**Класс `LoodsmanApiClient : IDisposable`** — предоставляет асинхронные методы для вызова конкретных эндпоинтов, абстракция над транспортным уровнем.

Приватные поля: `_httpClient` (HttpClient), `_config` (AppConfiguration).

Инициализация `_httpClient` в конструкторе:

```csharp
_httpClient = new HttpClient
{
    BaseAddress = new Uri($"{_config.AppServerHost}/api/v{_config.ApiVersion}/"),
    Timeout = TimeSpan.FromSeconds(_config.RequestTimeoutSeconds)
};
_httpClient.DefaultRequestHeaders.Add("web-loodsman-session", _config.SessionId);
```

- `BaseAddress` позволяет использовать относительные пути в методах;
- `Timeout` предотвращает зависание при проблемах с сетью;
- заголовок `web-loodsman-session` — стандартный способ передать идентификатор сессии для аутентификации запросов к WebAPI ЛОЦМАН:PLM.

`Dispose()` вызывает `_httpClient?.Dispose()`.

Набор методов расширяется разработчиком отчёта под конкретную задачу (см. [[csharp-scripts-running.md]] для примера).

## Models.cs

Объектные модели (DTO) для обмена данными со скриптом, плюс внутренние модели для строк отчёта. Конкретный набор моделей зависит от отчёта — см. [[csharp-scripts-prototyping.md]].

## Program.cs

Точка входа. Метод `Main` (асинхронный):

1. получает `AppConfiguration` через `GetConfiguration(args, Console.In.ReadToEnd())`;
2. создаёт `LoodsmanApiClient`;
3. создаёт сервис сбора данных (`ReportService`) с `apiClient` и `config`;
4. запускает генерацию отчёта;
5. печатает результат через `ReportPrinter.PrintJson`;
6. оборачивает всё в `try-catch`, ошибки — в `Console.Error`.

## ReportService.cs

Основная бизнес-логика: обход дерева объектов и формирование строк отчёта. Реализация специфична для каждого отчёта — см. [[csharp-scripts-automation.md]].

## ReportPrinter.cs

Статическая утилита для форматированного вывода результата в консоль в виде JSON (с отступами, camelCase, корректной обработкой кириллицы). См. [[csharp-scripts-best-practices.md]].

## Связанные узлы

- [[csharp-scripts-loading.md]] — модульность и входные данные
- [[csharp-scripts-best-practices.md]] — рекомендации
- [[csharp-scripts-basics.md]] — основы
