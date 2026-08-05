# C# скрипты: основы

## Что такое C# скрипты

C# скрипты — это файлы с расширением `.csx`, содержащие исполняемый код C# без обязательного объявления классов и методов. Это интерактивный способ запуска C# кода.

## Основные утилиты

- **dotnet-script** — кроссплатформенный интерпретатор C# скриптов
- **csi** — интерфейс командной строки C# (legacy, Windows)

## Установка dotnet-script

```bash
dotnet tool install -g dotnet-script
```

## Базовый пример

```csharp
// helloworld.csx
Console.WriteLine("Hello from C# script!");

// Использование переменных
var name = "World";
Console.WriteLine($"Hello, {name}!");

// Функции
void Greet(string greeting) => Console.WriteLine(greeting);
Greet("Welcome to C# scripts!");
```

## Особенности .csx файлов

- Топ-левый код выполняется последовательно
- Не требуется `namespace`, `class`, `Main`
- Поддерживаются `#r`, `#load` директивы
- Совместим с .NET SDK

## Связанные узлы

- [[csharp-scripts-running.md]] — запуск скриптов
- [[csharp-scripts-references.md]] — зависимости
- [[vscode-setup-for-csharp-scripts.md]] — настройка VS Code