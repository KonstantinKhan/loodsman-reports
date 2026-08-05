# Links API

Связи, link entries, действительность. **118 методов**.

## Области API

- `/api/v4/Links` - связи между объектами
- `/api/v4/Effectivity` - действительность связей

## Основные DTO

- `LinkDto` - связь между объектами
- `LinkEntryDto` - вхождение в связь
- `AbsLinkDto` - абстрактная связь
- `LinkTypeDto` - тип связи

См. [[types-reference.md]]

---

## Связи (Links)

### Информация о связях

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/Links/...` | Информация о связях |

---

## Действительность (Effectivity)

### Абстрактные связи

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/add-abs-link` | Добавить абстрактную связь |
| POST | `/api/v4/Effectivity/fix-abs-link` | Зафиксировать абстрактную связь |
| POST | `/api/v4/Effectivity/add-abs-link-effectivity` | Добавить действительность абс. связи |

### Связи

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/add-link-effectivity` | Добавить действительность связи |
| POST | `/api/v4/Effectivity/fix-link` | Зафиксировать связь |

### Версии в действительности

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/add-version-eff` | Добавить версию в действительность |
| POST | `/api/v4/Effectivity/add-version-to-eff` | Добавить версию к действительности |

### Динамические деревья

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/get-dynamic-tree` | Получить динамическое дерево |
| POST | `/api/v4/Effectivity/get-dynamic-full-tree` | Получить полное динамическое дерево |
| POST | `/api/v4/Effectivity/get-dynamic-linked-objects` | Связанные объекты (динамически) |
| POST | `/api/v4/Effectivity/get-dynamic-full-tree/by-configuration` | Дерево по конфигурации |

### Правила действительности

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/new-eff-rule` | Создать правило действительности |
| POST | `/api/v4/Effectivity/add-rule-component` | Добавить компонент правила |
| POST | `/api/v4/Effectivity/rename-eff-rule` | Переименовать правило |
| POST | `/api/v4/Effectivity/make-eff-rule-common` | Сделать правило общим |
| POST | `/api/v4/Effectivity/make-eff-rule-own` | Сделать правило собственным |

### Контекст действительности

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/copy-fixed-context` | Скопировать фиксированный контекст |
| POST | `/api/v4/Effectivity/clear-fixed-context` | Очистить фиксированный контекст |
| POST | `/api/v4/Effectivity/publish-fixed-context` | Опубликовать фиксированный контекст |

### Проверка по правилам

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/check-object-by-eff-rule` | Проверить объект по правилу |
| POST | `/api/v4/Effectivity/check-object-by-eff-rule-with-root-version` | Проверить объект (с корневой версией) |

### Атрибуты действительности

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/set-abs-link-attribute-value` | Установить атрибут абс. связи |
| POST | `/api/v4/Effectivity/set-abs-link-effectivity-attribute-value` | Атрибут действительности абс. связи |
| POST | `/api/v4/Effectivity/set-abs-link-effectivity-final-product` | Финальный продукт абс. связи |
| POST | `/api/v4/Effectivity/set-link-effectivity-attribute-value` | Атрибут действительности связи |
| POST | `/api/v4/Effectivity/set-link-effectivity-final-product` | Финальный продукт связи |

### Компоненты правил

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/set-rule-component-exact-structure` | Точная структура компонента |
| POST | `/api/v4/Effectivity/set-rule-component-param` | Параметр компонента |

### Quick параметры

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Effectivity/set-fixed-context-rule-quick-param-value` | Quick параметр контекста |

---

## См. также

- [[objects-api.md]] - Objects API
- [[workflow-api.md]] - Workflow API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок