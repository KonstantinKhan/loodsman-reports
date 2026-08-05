# Настройка VS Code для C# скриптов

## Установка расширения C#

1. Откройте VS Code
2. Установите расширение **C# Dev Kit** или **C#** (Microsoft)
3. Установите **.NET Install Tool**

## Установка .NET SDK

```bash
# macOS
brew install dotnet

# Linux
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh
chmod +x dotnet-install.sh
./dotnet-install.sh

# Проверка версии
dotnet --version
```

## Установка dotnet-script

```bash
dotnet tool install -g dotnet-script
```

## Базовая конфигурация workspace

Создайте `.vscode/settings.json`:

```json
{
  "omnisharp.enableRoslynAnalyzers": true,
  "omnisharp.enableEditorConfigSupport": true,
  "omnisharp.enableImportCompletion": true,
  "dotnet.server.useOmnisharp": true
}
```

## Настройка IntelliSense

Omnisharp автоматически обеспечивает:
- Автодополнение кода
- Навигацию по символам (F12)
- Подсветку ошибок
- Форматирование

## Связанные узлы

- [[vscode-debug-scripts.md]] — отладка
- [[vscode-intellisense-scripts.md]] — IntelliSense
- [[csharp-scripts-basics.md]] — основы