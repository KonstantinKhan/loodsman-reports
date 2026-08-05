# Loodsman REST API - Обзор

**Версия API**: 4.0  
**Базовый URL**: `/api/v4`  
**Авторизация**: API key через заголовок `Authorization`

## Информация о системе

- **Версия бинарного файла**: 24.0.9634.32853
- **Всего методов**: 1,372
- **Всего DTO типов**: 12,900+

## Функциональные области

| Область | Методов | Описание |
|---------|---------|----------|
| Objects | 285 | Работа с объектами, версиями, состояниями |
| Workflow | 185 | Маршруты, стадии, задачи, типовые процессы |
| Search | 156 | Поиск, фильтрация, навигация |
| Types | 142 | Типы объектов, атрибуты, шаблоны, иерархия |
| Links | 118 | Связи, link entries, действительность |
| Reports | 98 | Отчёты, скрипты, экспорт |
| Users/Roles | 87 | Пользователи, роли, замещения, права |
| Files | 76 | Файлы, загрузка, скачивание, архивы |
| Config | 65 | Конфигурации, карточки, варианты |
| Processes | 52 | Бизнес-процессы, асинхронные задачи |
| Auth | 44 | Аутентификация, сессии |
| BoReference | - | Интеграция со справочником |

## Авторизация

API использует API key авторизацию через HTTP-заголовок:

```
Authorization: <api_key>
```

## Формат ответов

Все ответы API возвращаются в JSON формате и содержат:

- `result` - результат операции (если успешен)
- `errorCode` - код ошибки (если есть)
- `errorMessage` - сообщение об ошибке (если есть)
- `timeGeneratedUtc` - время генерации ответа (UTC)

## Обработка ошибок

См. [[errors.md]] - коды ошибок и их описание.

## Быстрый старт

1. Получите API key
2. Добавьте заголовок `Authorization` с вашим ключом
3. Используйте базовый URL: `https://<server>/api/v4`

## Ссылки по областям

- [[objects-api.md]] - Objects API
- [[workflow-api.md]] - Workflow API
- [[search-api.md]] - Search API
- [[types-api.md]] - Types API
- [[links-api.md]] - Links API
- [[reports-api.md]] - Reports API
- [[users-roles-api.md]] - Users & Roles API
- [[files-api.md]] - Files API
- [[config-api.md]] - Configuration API
- [[processes-api.md]] - Processes API
- [[auth-api.md]] - Authentication API
- [[bo-reference-api.md]] - BO Reference API
- [[types-reference.md]] - Типы данных и DTO