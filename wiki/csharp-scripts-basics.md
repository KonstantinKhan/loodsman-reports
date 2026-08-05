# C# скрипты: основы

## Что такое отчёт типа "C# скрипт"

Отчёт типа "C# скрипт" — это **консольное приложение на .NET**, которое:

1. получает от службы выполнения скриптов адрес сервера приложений, идентификатор сессии и пользовательские параметры отчёта;
2. обращается к WebAPI сервера приложений ЛОЦМАН:PLM (REST, `HttpClient`) за нужными данными;
3. формирует результат в виде списка строк и печатает его в стандартный поток вывода как JSON;
4. этот JSON затем привязывается как источник данных к визуальному шаблону FastReport.NET в "ЛОЦМАН-Конфигураторе" — см. [[report-template-binding.md]].

Это не `.csx`-скрипты и не `dotnet-script` — обычный компилируемый проект с `Main`, который собирается в `.exe`.

> Есть и другой, устаревший тип — "C# ServerAPI скрипт" (библиотека `Ascon.Plm.ServerApi`, обращение к серверным хранимым процедурам). Использовать его для новых отчётов не нужно — актуальный способ получения данных — REST API, описанный в `swagger.lapis`.

## Требования к инструментам

- Visual Studio (Community и старше) — https://visualstudio.microsoft.com/ru/vs/community/
- .NET SDK 8 — https://dotnet.microsoft.com/ru-ru/download/dotnet/8.0

Версия .NET выбирается исходя из требований серверной части ЛОЦМАН:PLM (на момент написания — .NET 8).

## Общий пайплайн разработки отчёта

1. Создать консольный проект — [[csharp-scripts-references.md]]
2. Изучить структуру шаблона (`AppConfiguration`, `LoodsmanApiClient`, `Models`, `ReportService`, `ReportPrinter`, `Program`) — [[csharp-scripts-structure.md]]
3. Реализовать получение входных параметров (`-a`, `--session`, JSON в stdin) — [[csharp-scripts-loading.md]]
4. Изучить нужные методы WebAPI через Swagger и подготовить модели данных — [[csharp-scripts-prototyping.md]]
5. Реализовать клиент к WebAPI (`HttpClient`) — [[csharp-scripts-running.md]]
6. Реализовать сбор и маппинг данных (обход дерева, обогащение атрибутами) — [[csharp-scripts-automation.md]]
7. Собрать точку входа и протестировать локально — [[csharp-scripts-best-practices.md]]
8. Привязать результат к шаблону FastReport — [[report-template-binding.md]]

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура шаблона
- [[csharp-scripts-references.md]] — создание проекта
