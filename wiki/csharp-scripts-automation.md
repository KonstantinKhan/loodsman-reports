# Получение данных из СП и маппинг в подготовленные модели

> **Кодировка**: в коде ниже используется `Console.WriteLine` для диагностических
> сообщений — это исходный вариант из тюториала ASCON. На практике эти строки
> нужно писать через кодек `Cp866` (`Cp866.WriteLine(Console.OpenStandardError(), ...)`),
> иначе кириллица в предупреждениях побьётся — см. [[csharp-scripts-loading.md]]
> и рабочий пример в [[csharp-scripts-best-practices.md]].

Основная задача `ReportService`: для заданного объекта найти все дочерние объекты на заданную глубину и собрать информацию об этих объектах, их атрибутах и связях.

Используются ранее подготовленные компоненты (тем же способом "manual dependency passing"):

- `AppConfiguration` — параметры;
- `LoodsmanApiClient` — обращения к серверу приложений;
- `ReportRow` — модель одной строки итоговой таблицы, накапливается в `List<ReportRow>`.

## Рекурсивный обход дерева

Логика метода:

1. добавить текущий объект в результирующую модель в виде `ReportRow`;
2. проверить первое условие выхода — достижение заданной глубины;
3. получить дочерние объекты через `_apiClient.GetLinkedObjectsAsync`;
4. проверить второе условие выхода — отсутствие дочерних объектов;
5. повторить для каждого дочернего объекта.

## Обогащение данных

Отдельный метод добавляет для всех уже собранных объектов информацию об атрибутах и связях:

1. для каждой строки в результирующей таблице получить атрибуты версии через `GetVersionAttributesAsync`, найти нужные по имени (например, "Наименование") и записать значение в строку;
2. получить атрибуты связи через `GetLinkAttributesAsync`, найти нужные по имени (например, "Позиция") и записать в строку.

Вспомогательные расчёты (например, суммирование массы с учётом количества и округления) реализуются отдельными приватными методами.

## Финальный код класса ReportService.cs

```csharp
using System.Globalization;

namespace ExactProductStructureReport
{
    public class ReportService
    {
        private readonly LoodsmanApiClient _apiClient;
        private readonly AppConfiguration _config;
        private List<ReportRow> _reportTable;

        public ReportService(LoodsmanApiClient apiClient, AppConfiguration config)
        {
            _apiClient = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        public async Task<List<ReportRow>> GenerateReportAsync(int rootIdVersion, int maxDepth)
        {
            _reportTable = new List<ReportRow>();

            var rootObjects = await _apiClient.GetObjectInfoAsync(rootIdVersion);
            if (rootObjects == null || !rootObjects.Any())
                throw new InvalidOperationException($"Не удалось получить информацию об объекте с ID {rootIdVersion}");

            var rootObject = rootObjects.First();

            await TraverseTreeAsync(rootObject, 1, maxDepth);
            await EnrichReportDataAsync();

            var firstRow = _reportTable.First();
            var sortedTable = _reportTable.Skip(1).OrderBy(x => x.Product).ToList();
            sortedTable.Insert(0, firstRow);
            return sortedTable;
        }

        private async Task TraverseTreeAsync(ObjectInfo currentObject, int currentLevel, int maxDepth)
        {
            _reportTable.Add(new ReportRow
            {
                IdLink = currentObject.idLink,
                IdVersion = currentObject.idVersion,
                Type = currentObject.type ?? string.Empty,
                Product = currentObject.product ?? string.Empty,
                VersionNumber = currentObject.version ?? string.Empty,
                Quantity = currentObject.maxCalc
            });

            if (currentLevel >= maxDepth)
                return;

            List<ObjectInfo> children;
            try
            {
                children = await _apiClient.GetLinkedObjectsAsync(currentObject.idVersion, _config.GetStringParameterByName("Тип связи"));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Предупреждение: не удалось получить дочерние объекты для версии {currentObject.idVersion}: {ex.Message}");
                return;
            }

            if (children == null || children.Count == 0)
                return;

            foreach (var child in children)
            {
                await TraverseTreeAsync(child, currentLevel + 1, maxDepth);
            }
        }

        private async Task EnrichReportDataAsync()
        {
            foreach (var row in _reportTable)
            {
                if (row.IdVersion != 0)
                {
                    try
                    {
                        var attributes = await _apiClient.GetVersionAttributesAsync(row.IdVersion);
                        row.Name = attributes.FirstOrDefault(x => x.name == "Наименование")?.value ?? string.Empty;

                        var weightStr = attributes.FirstOrDefault(x => x.name == "Масса")?.value;
                        if (!string.IsNullOrWhiteSpace(weightStr))
                            row.Weight = CalculateTotalWeight(weightStr, row.Quantity);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Предупреждение: не удалось получить атрибуты версии {row.IdVersion}: {ex.Message}");
                    }
                }

                if (row.IdLink != 0)
                {
                    try
                    {
                        var linkAttributes = await _apiClient.GetLinkAttributesAsync(row.IdLink);
                        row.Position = linkAttributes.FirstOrDefault(x => x.name == "Позиция")?.value ?? string.Empty;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Предупреждение: не удалось получить атрибуты связи {row.IdLink}: {ex.Message}");
                    }
                }
            }
        }

        private string CalculateTotalWeight(string weightStr, double quantity)
        {
            if (string.IsNullOrWhiteSpace(weightStr))
                return string.Empty;

            if (!double.TryParse(weightStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double weight))
                return string.Empty;

            double totalWeight = weight * quantity;
            if (totalWeight == 0)
                return string.Empty;

            return Math.Round(totalWeight, int.Parse(_config.GetStringParameterByName("Количество знаков после запятой")))
                .ToString(CultureInfo.CurrentCulture);
        }
    }
}
```

Реализация логики специфична для каждого отчёта — приведённый код (по мотивам отчёта "Состав точной структуры") — рабочий пример подхода, а не готовый шаблон для копирования без изменений.

## Связанные узлы

- [[csharp-scripts-best-practices.md]] — best practices, точка входа, тестирование
- [[csharp-scripts-prototyping.md]] — подготовка моделей
- [[csharp-scripts-running.md]] — HTTP-клиент
