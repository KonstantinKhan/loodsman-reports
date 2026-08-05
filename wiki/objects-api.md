# Objects API

Работа с объектами, версиями, состояниями. **285 методов**.

**Базовый путь**: `/api/v4/ObjectInfo`

## Основные DTO

- `ObjectDto` - базовая информация об объекте
- `VersionDto` - версия объекта
- `VersionInfoDto` - подробная информация о версии
- `BusinessObjectDto` - бизнес-объект
- `VersionDtoEnvelope` - обёртка для ответа с версией

См. [[types-reference.md]]

---

## Получение информации

### Информация о версиях

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-info-about-version` | Информация о версии |
| GET | `/get-info-about-version-mode-{N}` | Информация о версии (режим N) |
| GET | `/get-info-about-version-by-guid` | Информация о версии по GUID |
| POST | `/get-info-about-version-by-guids` | Информация о версиях по GUIDs |

### Информация о связях

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-info-about-link` | Информация о связи |
| GET | `/get-info-about-link-mode-1` | Информация о связи (режим 1) |
| GET | `/get-info-about-link-mode-2` | Информация о связи (режим 2) |
| GET | `/get-info-about-links-by-ids` | Информация о связях по IDs |

### Другая информация

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/get-info-about-owner` | Информация о владельце |
| POST | `/get-original-attributes` | Исходные атрибуты |
| POST | `/get-attributes-values-2` | Значения атрибутов |

---

## Атрибуты объектов

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/objects/by-ids/attributes/by-names/text-plain-values` | Текстовые значения атрибутов |
| POST | `/links/by-ids/attributes/by-names/text-plain-values` | Текстовые значения атрибутов связей |
| POST | `/get-attr-image-values` | Значения атрибутов-изображений |

---

## Связанные объекты

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/get-linked-objects-for-objects` | Связанные объекты |
| POST | `/get-prop-objects-2` | Свойства объектов |

---

## Поиск и навигация

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/find-path` | Найти путь между объектами |
| POST | `/check-unique-names` | Проверка уникальности имён |
| POST | `/load-ids` | Загрузка IDs по фильтру |

---

## Типовые операции

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/get-spec-info-for-main-by-type-id-and-key-attr` | Информация для спецификации |
| POST | `/get-info-about-versions-mode-7` | Информация о версиях (режим 7) |

---

## См. также

- [[types-api.md]] - Types API (работа с типами объектов)
- [[links-api.md]] - Links API (связи между объектами)
- [[workflow-api.md]] - Workflow API (маршруты и задачи)
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок