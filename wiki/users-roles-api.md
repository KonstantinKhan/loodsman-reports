# Users & Roles API

Пользователи, роли, замещения, права. **87 методов**.

## Области API

- `/api/v4/OrgStructure` - организационная структура
- `/api/v4/UserReplacements` - замещения пользователей
- `/api/v4/UserSet` - наборы пользователей
- `/api/v4/Auth` - аутентификация

## Основные DTO

- `UserDto` - пользователь
- `RoleDto` - роль
- `PostDto` - должность
- `UnitDto` - подразделение
- `UserReplacementDto` - замещение пользователя

См. [[types-reference.md]]

---

## Организационная структура (OrgStructure)

### Пользователи

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/OrgStructure/users/register` | Зарегистрировать пользователя |
| DELETE | `/api/v4/OrgStructure/users/{id}/dismiss` | Уволить пользователя |
| POST | `/api/v4/OrgStructure/users/{userId}/posts/{postId}/assign` | Назначить должность |
| DELETE | `/api/v4/OrgStructure/users/{userId}/posts/{postId}/cancel` | Отменить должность |
| POST | `/api/v4/OrgStructure/users/{userId}/roles/{roleId}/assign` | Назначить роль |
| DELETE | `/api/v4/OrgStructure/users/{userId}/roles/{roleId}/cancel` | Отменить роль |
| POST | `/api/v4/OrgStructure/set-user-properties` | Установить свойства пользователя |
| POST | `/api/v4/OrgStructure/set-user-status` | Установить статус пользователя |
| POST | `/api/v4/OrgStructure/set-user-picture` | Установить картинку пользователя |
| POST | `/api/v4/OrgStructure/users/by-sso-guids` | Пользователи по SSO GUIDs |
| POST | `/api/v4/OrgStructure/get-users-role-info` | Информация о ролях пользователя |
| POST | `/api/v4/OrgStructure/get-users-role-info-2` | Информация о ролях пользователя (v2) |

### Должности

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/OrgStructure/posts/register` | Создать должность |
| DELETE | `/api/v4/OrgStructure/posts/{id}/abolition` | Упразднить должность |
| POST | `/api/v4/OrgStructure/posts/{postId}/deputy-posts/{deputyPostId}/assign` | Назначить замещение |
| DELETE | `/api/v4/OrgStructure/posts/{postId}/deputy-posts/{deputyPostId}/cancel` | Отменить замещение |
| POST | `/api/v4/OrgStructure/posts/{postId}/roles/{roleId}/assign` | Назначить роль должности |
| DELETE | `/api/v4/OrgStructure/posts/{postId}/roles/{roleId}/cancel` | Отменить роль должности |
| POST | `/api/v4/OrgStructure/posts/by-sso-guids` | Должности по SSO GUIDs |

### Роли

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/OrgStructure/roles/register` | Создать роль |
| DELETE | `/api/v4/OrgStructure/roles/{id}/abolition` | Упразднить роль |
| POST | `/api/v4/OrgStructure/roles/by-sso-guids` | Роли по SSO GUIDs |

### Подразделения

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/OrgStructure/units/register` | Создать подразделение |
| DELETE | `/api/v4/OrgStructure/units/{id}/abolition` | Упразднить подразделение |
| POST | `/api/v4/OrgStructure/units/by-sso-guids` | Подразделения по SSO GUIDs |
| POST | `/api/v4/OrgStructure/set-unit-properties` | Установить свойства подразделения |

---

## Замещения пользователей (UserReplacements)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/UserReplacements/create` | Создать замещение |
| POST | `/api/v4/UserReplacements/users/{missingUserLogin}/deputy-users/{deputyUserLogin}/can-create-replacement` | Проверить возможность замещения |

---

## Наборы пользователей (UserSet)

| Метод | Путь | Описание |
|-------|------|----------|
| POST | `/api/v4/UserSet/up-user-set` | Обновить набор пользователей |
| POST | `/api/v4/UserSet/set-user-set-pattern` | Установить паттерн набора пользователей |

---

## См. также

- [[workflow-api.md]] - Workflow API
- [[auth-api.md]] - Authentication API
- [[types-reference.md]] - Справочник типов данных
- [[errors.md]] - Коды ошибок