# IntelliSense в VS Code для C# скриптов

## Автодополнение кода

Omnisharp обеспечивает автодополнение:
- Для типов из .NET
- Для локальных переменных
- Для методов и свойств

```csharp
var list = new List<string>();
list.  // Показывает методы List<T>
```

## Навигация по коду

- **F12** — Go to Definition
- **Shift+F12** — Find All References
- **Ctrl+Click** — Navigate to Definition

## Диагностика ошибок

Ошибки отображаются:
- В коде (красные подчёркивания)
- В панели Problems (Ctrl+Shift+M)
- В строке состояния

## Quick Fixes

- **Ctrl+.** — Quick Fix
- Автоимпорт пространств имён
- Генерация методов

## Code Lenses

- Показывает количество использований
- Быстрый рефакторинг

## Форматирование

- **Shift+Alt+F** — Format Document
- **Ctrl+K, Ctrl+F** — Format Selection

## Настройка OmniSharp

```json
{
  "omnisharp.enableRoslynAnalyzers": true,
  "omnisharp.enableEditorConfigSupport": true,
  "omnisharp.enableImportCompletion": true
}
```

## Связанные узлы

- [[vscode-debug-scripts.md]] — отладка
- [[vscode-setup-for-csharp-scripts.md]] — настройка
- [[csharp-scripts-structure.md]] — структура кода