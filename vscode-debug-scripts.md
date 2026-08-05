# Отладка C# скриптов в VS Code

## Конфигурация launch.json

Создайте `.vscode/launch.json`:

```json
{
  "version": "0.2.0",
  "configurations": [
    {
      "name": "Debug C# Script",
      "type": "coreclr",
      "request": "launch",
      "preLaunchTask": "build",
      "program": "${workspaceFolder}/script.csx",
      "args": [],
      "cwd": "${workspaceFolder}",
      "stopAtEntry": false,
      "console": "integratedTerminal"
    }
  ]
}
```

## Конфигурация tasks.json

Создайте `.vscode/tasks.json`:

```json
{
  "version": "2.0.0",
  "tasks": [
    {
      "label": "build",
      "command": "dotnet-script",
      "type": "process",
      "args": ["compile", "${workspaceFolder}/script.csx"],
      "problemMatcher": []
    }
  ]
}
```

## Использование точек останова

- **F9** — установить/убрать точку останова
- **F5** — запустить отладку
- **F10** — Step Over
- **F11** — Step Into
- **Shift+F11** — Step Out

## Инспекция переменных

- Наведите курсор на переменную
- Используйте панель VARIABLES
- Watch panel для выражений

## Debug Console

Выполнение выражений во время отладки:

```csharp
// В Debug Console
variableName
someMethod(arg)
```

## Связанные узлы

- [[vscode-intellisense-scripts.md]] — IntelliSense
- [[vscode-setup-for-csharp-scripts.md]] — настройка