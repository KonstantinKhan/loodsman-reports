# Types API

Типы объектов, атрибуты, шаблоны, иерархия. **142 метода**.

**Базовый путь**: `/api/v4/MetaData`

## Основные DTO

- `TypeDto` - тип объекта
- `AttributeDto` - атрибут
- `AttributeTemplateDto` - шаблон атрибута
- `TypeHierarchyDto` - иерархия типов
- `TypeLinkDto` - связь между типами

См. [[types-reference.md]]

---

## Типы объектов

### Информация о типах

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-info-about-type` | Информация о типе |
| GET | `/get-info-about-type-mode-{N}` | Информация о типе (режим N) |

### Текущая база

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-info-about-current-base` | Информация о текущей базе |
| GET | `/get-info-about-current-base-mode-{N}` | Информация о текущей базе (режим N) |

---

## Атрибуты

### Информация об атрибутах

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-info-about-attribute` | Информация об атрибуте |
| GET | `/get-info-about-attribute-mode-{N}` | Информация об атрибуте (режим N) |
| GET | `/get-attribute-list-2` | Список атрибутов |

### Шаблоны атрибутов

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-attr-templates` | Шаблоны атрибутов |

### Коды документов

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/get-doc-codes` | Коды документов |

---

## Управление наследованием

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/allow-delete-inherited-elements` | Проверка возможности удаления |
| POST | `/allow-delete-inherited-elements` | Проверка возможности удаления |
| DELETE | `/allow-delete-inherited-elements` | Отмена проверки |

---

## См. также

- [[objects-api.md]] - Objects API (работа с объектами)
- [[config-api.md]] - Configuration API (конфигурации)
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок