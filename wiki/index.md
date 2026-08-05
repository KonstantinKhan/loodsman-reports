# Loodsman Web API Wiki

## API Overview
- [[loodsman-api-overview.md]] - Общая информация о Loodsman Web API v4.0

## Lapis REST API v4.0

### Overview
- [[api-overview.md]] - Обзор REST API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок

### Core APIs
- [[objects-api.md]] - Objects API (285 методов)
- [[types-api.md]] - Types API (142 метода)
- [[links-api.md]] - Links API (118 методов)
- [[workflow-api.md]] - Workflow API (185 методов)
- [[users-roles-api.md]] - Users & Roles API (87 методов)

### Additional APIs
- [[search-api.md]] - Search API (156 методов)
- [[files-api.md]] - Files API (76 методов)
- [[reports-api.md]] - Reports API (98 методов)
- [[config-api.md]] - Configuration API (65 методов)
- [[processes-api.md]] - Processes API (52 метода)
- [[auth-api.md]] - Authentication API (44 метода)
- [[bo-reference-api.md]] - BO Reference API

## Основные сущности
### Attributes
- [[loodsman-attributes-overview.md]] - Обзор атрибутов
- [[loodsman-attribute-types.md]] - Типы атрибутов
- [[loodsman-attribute-values.md]] - Значения атрибутов
- [[loodsman-attribute-templates.md]] - Шаблоны атрибутов
- **API:** [[types-api.md]] - Types API

### Links
- [[loodsman-links-overview.md]] - Обзор связей
- [[loodsman-link-types.md]] - Типы связей
- [[loodsman-abs-links.md]] - Абстрактные связи
- [[loodsman-link-entries.md]] - Вложения связей
- **API:** [[links-api.md]] - Links API

### Objects and Versions
- [[loodsman-objects-overview.md]] - Обзор объектов
- [[loodsman-versions.md]] - Версии объектов
- [[loodsman-business-objects.md]] - Бизнес-объекты
- [[loodsman-object-search.md]] - Поиск объектов
- **API:** [[objects-api.md]] - Objects API

## Workflow
- [[loodsman-workflow-overview.md]] - Обзор workflow
- [[loodsman-routes.md]] - Маршруты
- [[loodsman-stages.md]] - Этапы
- [[loodsman-business-processes.md]] - Бизнес-процессы
- [[loodsman-typical-processes.md]] - Типовые процессы
- **API:** [[workflow-api.md]] - Workflow API

## Tasks
- [[loodsman-tasks-overview.md]] - Обзор задач
- [[loodsman-task-states.md]] - Состояния задач
- [[loodsman-task-attributes.md]] - Атрибуты задач
- [[loodsman-multitasks.md]] - Мультитаски
- [[loodsman-task-search.md]] - Поиск задач
- **API:** [[workflow-api.md]] - Workflow API

## Reports
- [[loodsman-reports-overview.md]] - Обзор отчётов
- [[loodsman-report-parameters.md]] - Параметры отчётов
- [[loodsman-report-formats.md]] - Форматы экспорта отчётов
- [[loodsman-report-generation.md]] - Генерация отчётов
- **API:** [[reports-api.md]] - Reports API

## Files
- [[loodsman-files-overview.md]] - Обзор работы с файлами
- [[loodsman-file-operations.md]] - Операции с файлами
- [[loodsman-archives.md]] - Архивы
- [[loodsman-file-signatures.md]] - Подпись файлов
- **API:** [[files-api.md]] - Files API

## Authorization
- [[loodsman-authorization-overview.md]] - Обзор авторизации
- [[loodsman-sign-roles.md]] - Роли подписи
- [[loodsman-access-levels.md]] - Уровни доступа
- [[loodsman-organizational-structure.md]] - Организационная структура
- [[loodsman-user-replacements.md]] - Замещения пользователей
- **API:** [[auth-api.md]] - Authentication API
- **API:** [[users-roles-api.md]] - Users & Roles API

## Search and Filter
- [[loodsman-search-overview.md]] - Обзор поиска
- [[loodsman-filters.md]] - Фильтры
- [[loodsman-filter-operators.md]] - Операторы фильтрации
- [[loodsman-search-variants.md]] - Варианты поиска
- **API:** [[search-api.md]] - Search API

## Effectivity
- [[loodsman-effectivity-overview.md]] - Обзор действительности
- [[loodsman-effectivity-types.md]] - Типы действительности
- [[loodsman-effectivity-rules.md]] - Правила действительности

## Configuration
- [[loodsman-configuration-overview.md]] - Обзор конфигурации
- [[loodsman-templates.md]] - Шаблоны
- [[loodsman-object-configurations.md]] - Конфигурации объектов
- [[loodsman-configuration-scripts.md]] - Скрипты конфигурации
- **API:** [[config-api.md]] - Configuration API
- **API:** [[processes-api.md]] - Processes API
- **API:** [[bo-reference-api.md]] - BO Reference API

## C# Scripts

Отчёты типа "C# скрипт" — консольные .NET-проекты, вызывающие WebAPI сервера
приложений ЛОЦМАН:PLM (REST, см. `swagger.lapis`). Это не `.csx`-скрипты.

- [[csharp-scripts-basics.md]] - Основы: что такое отчёт "C# скрипт", требования, общий пайплайн
- [[csharp-scripts-structure.md]] - Структура и служебные классы шаблона
- [[csharp-scripts-references.md]] - Создание проекта и зависимости
- [[csharp-scripts-loading.md]] - Получение адреса СП, сессии и пользовательских данных; кодировка stdin/stdout/stderr (CP866)
- [[csharp-scripts-prototyping.md]] - Подготовка моделей данных через Swagger
- [[csharp-scripts-running.md]] - Подключение к серверу приложений (HttpClient)
- [[csharp-scripts-automation.md]] - Получение данных и маппинг в модели
- [[csharp-scripts-best-practices.md]] - Точка входа, вывод результата, локальное тестирование
- [[report-template-binding.md]] - Привязка результата скрипта к шаблону FastReport

## VS Code для C# Scripts
- [[vscode-setup-for-csharp-scripts.md]] - Настройка VS Code
- [[vscode-intellisense-scripts.md]] - IntelliSense
- [[vscode-debug-scripts.md]] - Отладка