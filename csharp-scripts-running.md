# Запуск C# скриптов

## Командная строка

### dotnet-script

```bash
# Запуск скрипта
dotnet-script script.csx

# С передачей аргументов
dotnet-script script.csx -- arg1 arg2
```

### Доступ к аргументам

```csharp
// Аргументы доступны через Args
foreach (var arg in Args)
{
    Console.WriteLine($"Arg: {arg}");
}
```

## Работа с вводом-выводом

```csharp
// Вывод
Console.WriteLine("Output message");
Console.Write("Without newline");
Console.Error.WriteLine("Error message");

// Ввод
var input = Console.ReadLine();
Console.WriteLine($"You entered: {input}");
```

## Возврат кода выхода

```csharp
Environment.Exit(0);  // Успех
Environment.Exit(1);  // Ошибка
```

## Запуск с интерактивной консолью

```bash
dotnet-script interactive
```

## Отладка через VS Code

Подробнее: [[vscode-debug-scripts.md]]

## Связанные узлы

- [[csharp-scripts-basics.md]] — основы
- [[vscode-setup-for-csharp-scripts.md]] — настройка окружения