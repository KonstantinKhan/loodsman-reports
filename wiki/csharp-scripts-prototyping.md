# Подготовка моделей данных с помощью документации OpenApi (Swagger)

## Как пользоваться документацией на WebAPI

Приложение взаимодействует с сервером приложений средствами WebAPI — обмен данными в формате JSON. WebAPI сервера приложений задокументировано в формате OpenAPI (Swagger).

Страница документации: `http://host:port/swagger/index.html`.

Для изучения документации авторизация не нужна, но для живого взаимодействия нужно выполнить вход через эндпоинт `/Auth/login`: кнопка **Try it out** → заполнить JSON с данными для входа → **Execute**. Значение `sessionId` из ответа пригодится для тестирования приложения (см. [[csharp-scripts-best-practices.md]]).

## Определение моделей данных

Сначала нужно понять, какие данные ожидаются в отчёте (например: тип, обозначение, наименование, номер, количество, масса, позиция), затем — через какие эндпоинты их получить.

Для отбора объектов и их атрибутов используются эндпоинты раздела `ObjectInfo` (актуальные пути и параметры — в `swagger.lapis`):

- **`GET /api/v4/ObjectInfo/get-prop-objects?objectList={id}`** — данные об объектах по указанным идентификаторам. Возвращает массив (по одному элементу на идентификатор). Используется для получения информации о корневом объекте.
- **`GET /api/v4/ObjectInfo/get-linked-fast?idVersion={id}&linkType={type}`** — дочерние версии объекта, связанные указанным типом связи, на один уровень вниз.
- **`GET /api/v4/ObjectInfo/get-info-about-version-mode-3?idVersion={id}`** — все атрибуты версии объекта.
- **`GET /api/v4/ObjectInfo/get-link-attributes-2?linkId={id}&mode=0`** — атрибуты экземпляра связи (`mode=0` — обычные и служебные атрибуты).

Поля выходных данных `get-prop-objects` и `get-linked-fast` частично пересекаются, что позволяет использовать общую модель `ObjectInfo`:

```csharp
public class ObjectInfo
{
    public int idLink { get; set; }
    public int idVersion { get; set; }
    public string type { get; set; }
    public string product { get; set; }
    public string version { get; set; }
    public double minCalc { get; set; }
    public double maxCalc { get; set; }
}
```

`get-info-about-version-mode-3` и `get-link-attributes-2` тоже пересекаются по интересующим полям — общая модель атрибута:

```csharp
public class Attributes
{
    public int id { get; set; }
    public string name { get; set; }
    public string value { get; set; }
}
```

## Модель строки отчёта

Данные для итоговой таблицы отчёта аккумулируются в отдельном объекте `ReportRow` — набор полей зависит от конкретного отчёта. Пример (отчёт по точной структуре):

```csharp
public class ReportRow
{
    public int IdVersion { get; set; }
    public int IdLink { get; set; }
    public string Type { get; set; }
    public string Product { get; set; }
    public string Name { get; set; }
    public string VersionNumber { get; set; }
    public double Quantity { get; set; }
    public string Weight { get; set; }
    public string Position { get; set; }
}
```

`IdVersion`/`IdLink` — служебные поля, не выводятся в отчёт, но нужны для рекурсивных запросов и обогащения данных.

Модели удобно генерировать из примера JSON-ответа конкретного эндпоинта (в Visual Studio — специальная вставка «Paste JSON as Classes»), затем вручную убрать лишние поля и переименовать в PascalCase.

## Связанные узлы

- [[csharp-scripts-running.md]] — вызов эндпоинтов через HttpClient
- [[csharp-scripts-automation.md]] — сбор и маппинг данных
- [[csharp-scripts-structure.md]] — структура
