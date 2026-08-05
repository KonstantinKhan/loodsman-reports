# Workflow API

Маршруты, стадии, задачи, типовые процессы. **185 методов**.

## Области API

- `/api/v4/Routes` - маршруты
- `/api/v4/RouteTasks` - задачи маршрутов
- `/api/v4/RouteItems` - элементы маршрутов
- `/api/v4/TypicalRoutes` - типовые маршруты

## Основные DTO

- `RouteDto` - маршрут
- `StageDto` - этап маршрута
- `TaskDto` - задача
- `MultitaskDto` - мультитаска
- `RouteItemDto` - элемент маршрута

См. [[types-reference.md]]

---

## Маршруты (Routes)

### Информация о маршрутах

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/Routes/{id}` | Получить маршрут |
| GET | `/api/v4/Routes/completed` | Завершённые маршруты |
| GET | `/api/v4/Routes/initial` | Инициализированные маршруты |
| GET | `/api/v4/Routes/favorites` | Избранные маршруты |
| GET | `/api/v4/Routes/auditing` | Аудит маршрутов |

### Операции с маршрутами

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/filter` | Фильтр маршрутов |
| POST | `/api/v4/Routes/by-typical/{typicalId}` | Создать маршрут по типовому |
| POST | `/api/v4/Routes/favorites` | Добавить в избранное |
| DELETE | `/api/v4/Routes/favorites` | Удалить из избранного |
| DELETE | `/api/v4/Routes/{id}` | Удалить маршрут |

### Блокировка маршрутов

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/lock` | Заблокировать маршрут |
| GET | `/api/v4/Routes/{id}/lock` | Статус блокировки |
| DELETE | `/api/v4/Routes/{id}/lock` | Разблокировать маршрут |

### Объекты маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/objects` | Добавить объекты |
| GET | `/api/v4/Routes/{id}/attached` | Прикреплённые объекты |
| GET | `/api/v4/Routes/{id}/attached-objects` | Прикреплённые объекты |
| DELETE | `/api/v4/Routes/{id}/objects/{objectId}` | Удалить объект |
| GET | `/api/v4/Routes/{id}/objects/without-access/count` | Количество объектов без доступа |

### Элементы маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/Routes/{id}/elements` | Элементы маршрута |
| GET | `/api/v4/Routes/{id}/items` | Элементы маршрута |

### Этапы маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/stages` | Добавить этапы |

### Атрибуты маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/Routes/attributes-values` | Значения атрибутов |
| POST | `/api/v4/Routes/attributes-values` | Установить значения атрибутов |

### Свойства маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/properties` | Свойства маршрута |

### Схема маршрута

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/scheme` | Схема маршрута |

### Подписчики

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/subscribers/{userId}` | Добавить подписчика |
| DELETE | `/api/v4/Routes/{id}/subscribers/{userId}` | Удалить подписчика |

### Переменные

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/variables` | Установить переменные |
| DELETE | `/api/v4/Routes/{id}/variables/{variableName}` | Удалить переменную |

### Сообщения задач

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Routes/{id}/tasks/{taskId}/messages/initiator` | Сообщение инициатора |

---

## Задачи маршрутов (RouteTasks)

### Операции с задачами

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/RouteTasks/...` | Операции с задачами |

---

## Элементы маршрутов (RouteItems)

### Операции с элементами

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/RouteItems/...` | Операции с элементами |

---

## Типовые маршруты (TypicalRoutes)

### Операции с типовыми маршрутами

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/TypicalRoutes/...` | Операции с типовыми маршрутами |

---

## См. также

- [[objects-api.md]] - Objects API
- [[users-roles-api.md]] - Users & Roles API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок