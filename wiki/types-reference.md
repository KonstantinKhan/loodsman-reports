# Типы данных и DTO

Справочник по основным типам данных Loodsman API.

## Примитивные типы

| Тип | Описание | Пример |
|-----|----------|--------|
| `str` | Строка | `"Документ"` |
| `int` | Целое число | `123` |
| `float` | Число с плавающей точкой | `12.5` |
| `bool` | Булево значение | `true` |
| `datetime` | Дата и время (ISO 8601) | `"2024-01-01T10:30:00Z"` |
| `date` | Дата (ISO 8601) | `"2024-01-01"` |

## Коллекции

| Тип | Описание |
|-----|----------|
| `[int]` | Массив целых чисел |
| `[str]` | Массив строк |
| `[any]` | Массив любого типа |

## Специальные типы

| Тип | Описание |
|-----|----------|
| `file` | Файл для загрузки |
| `FilePath` | Путь к файлу |
| `Crc32` | Контрольная сумма CRC32 |

## Основные категории DTO

### Objects & Versions

- `ObjectDto` - базовая информация об объекте
- `VersionDto` - версия объекта
- `VersionInfoDto` - подробная информация о версии
- `BusinessObjectDto` - бизнес-объект

### Links

- `LinkDto` - связь между объектами
- `AbsLinkDto` - абстрактная связь
- `LinkEntryDto` - вхождение в связь
- `LinkTypeDto` - тип связи

### Attributes

- `AttributeDto` - атрибут
- `AttributeValueDto` - значение атрибута
- `AttributeTemplateDto` - шаблон атрибута
- `AttributeTypeDto` - тип атрибута

### Workflow

- `RouteDto` - маршрут
- `StageDto` - этап маршрута
- `TaskDto` - задача
- `MultitaskDto` - мультитаска

### Users & Roles

- `UserDto` - пользователь
- `RoleDto` - роль
- `UserReplacementDto` - замещение

### Files

- `FileDto` - файл
- `ArchiveDto` - архив
- `FileSignatureDto` - подпись файла

### Reports

- `ReportDto` - отчёт
- `ReportParameterDto` - параметр отчёта

### Async Tasks

- `AsyncTaskDto` - асинхронная задача
- `BackgroundConversionAsyncTaskDto` - фоновая конвертация

## Enum-типы

Полный список enum-типов см. в [[../raw/swagger.lapis]] в секции `[types]`.

Ключевые enum-типы:

- `TaskStates` - состояния задач
- `StageStates` - состояния этапов
- `RouteItemStates` - состояния элементов маршрута
- `AsyncTaskStates` - состояния асинхронных задач
- `AttributeTypes` - типы атрибутов
- `LinkTypeKinds` - виды связей

## Паттерны DTO

### Input DTO

Используются для передачи входных параметров в методы API.

Пример:
```
CreateVersionInputDto:
  documentId?: int
  name?: str?
  description?: str?
```

### Output DTO

Используются для возврата результатов из методов API.

Пример:
```
VersionDtoEnvelope:
  result?: VersionDto
  errorCode?: str?
  errorMessage?: str?
  timeGeneratedUtc?: datetime
```

## Вспомогательные DTO

### Filter DTO

- `StringFilter` - фильтр по строковому значению
- `NumericFilter` - фильтр по числовому значению
- `DateFilter` - фильтр по дате
- `EnumFilter` - фильтр по enum

### Expression DTO

- `AndExpression` - логическое И
- `OrExpression` - логическое ИЛИ
- `NotExpression` - логическое НЕ

## Дополнительно

- [[api-overview.md]] - Обзор API
- [[errors.md]] - Коды ошибок
- [[../raw/swagger.lapis]] - Полный список DTO