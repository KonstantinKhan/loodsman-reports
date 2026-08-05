# Configuration API

Конфигурации, карточки, варианты. **65 методов**.

## Области API

- `/api/v4/Card` - карточки
- `/api/v4/ConfMetaData` - метаданные конфигураций
- `/api/v4/ObjectConfiguration` - конфигурации объектов
- `/api/v4/ConfigEffectivity` - действительность конфигураций
- `/api/v4/ConfigNotifySystem` - система уведомлений конфигураций
- `/api/v4/SystemSettings` - системные настройки

## Основные DTO

- `CardDto` - карточка
- `ObjectConfigurationDto` - конфигурация объекта
- `ConfigEffectivityDto` - действительность конфигурации

См. [[types-reference.md]]

---

## Карточки (Card)

| Метод | Путь | Описание |
|-------|------|----------|
| DELETE | `/api/v4/Card/del-card` | Удалить карточку |
| DELETE | `/api/v4/Card/del-card-all-attr` | Удалить все атрибуты карточки |
| DELETE | `/api/v4/Card/del-card-type-user-group` | Удалить группу пользователя типа карточки |
| DELETE | `/api/v4/Card/del-card-type-all-user-group` | Удалить все группы пользователей типа карточки |
| GET | `/api/v4/Card/get-all-card-group-types` | Получить все типы групп карточек |
| GET | `/api/v4/Card/get-all-cards` | Получить все карточки |
| GET | `/api/v4/Card/get-card-attributes` | Получить атрибуты карточки |

---

## Метаданные конфигураций (ConfMetaData)

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/ConfMetaData/...` | Получить метаданные конфигураций |

---

## Конфигурации объектов (ObjectConfiguration)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/ObjectConfiguration/...` | Операции с конфигурациями объектов |
| GET | `/api/v4/ObjectConfiguration/...` | Получить конфигурации объектов |

---

## Действительность конфигураций (ConfigEffectivity)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/ConfigEffectivity/...` | Операции с действительностью конфигураций |

---

## Система уведомлений (ConfigNotifySystem)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/ConfigNotifySystem/...` | Операции с системой уведомлений |

---

## Системные настройки (SystemSettings)

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/SystemSettings/...` | Получить системные настройки |
| POST | `/api/v4/SystemSettings/...` | Установить системные настройки |

---

## См. также

- [[types-api.md]] - Types API
- [[objects-api.md]] - Objects API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок