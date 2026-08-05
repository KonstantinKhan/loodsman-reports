# Processes API

Бизнес-процессы, асинхронные задачи. **52 метода**.

## Области API

- `/api/v4/AsyncTasks` - асинхронные задачи
- `/api/v4/Workflow` - бизнес-процессы
- `/api/v4/Integration` - интеграция

## Основные DTO

- `AsyncTaskDto` - асинхронная задача
- `BackgroundConversionAsyncTaskDto` - фоновая конвертация
- `BusinessProcessDto` - бизнес-процесс

См. [[types-reference.md]]

---

## Асинхронные задачи (AsyncTasks)

### Серверные отчёты

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/AsyncTasks/server-reports` | Получить серверные отчёты |
| GET | `/api/v4/AsyncTasks/server-reports/{id}/download` | Скачать серверный отчёт |
| DELETE | `/api/v4/AsyncTasks/server-reports/{id}` | Удалить серверный отчёт |
| PATCH | `/api/v4/AsyncTasks/server-reports/{id}/cancel` | Отменить серверный отчёт |

### Фоновые конвертации

| Метод | Путь | Описание |
|-------|------|----------|
| GET | `/api/v4/AsyncTasks/background-conversions` | Получить фоновые конвертации |
| POST | `/api/v4/AsyncTasks/background-conversions/objects/{id}` | Создать конвертацию объекта |
| DELETE | `/api/v4/AsyncTasks/background-conversions/{id}` | Удалить конвертацию |
| PATCH | `/api/v4/AsyncTasks/background-conversions/{id}/cancel` | Отменить конвертацию |

### Состояния асинхронных задач

- `1` - Создана
- `2` - В работе
- `3` - Завершена успешно
- `4` - Завершена с ошибкой
- `5` - Отменена
- `6` - Прервана

---

## Бизнес-процессы (Workflow)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Workflow/...` | Операции с бизнес-процессами |
| GET | `/api/v4/Workflow/...` | Получить бизнес-процессы |

### Состояния бизнес-процессов

- `0` - Не запущен
- `1` - Запущен
- `2` - Приостановлен
- `3` - Завершён

---

## Интеграция (Integration)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/Integration/...` | Операции интеграции |
| GET | `/api/v4/Integration/...` | Получить данные интеграции |

---

## См. также

- [[workflow-api.md]] - Workflow API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок