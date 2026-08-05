# Структура C# скриптов

## Топ-левый код

```csharp
// Выполняется последовательно
Console.WriteLine("Starting...");

var x = 10;
var y = 20;
var sum = x + y;
Console.WriteLine($"Sum: {sum}");
```

## Функции

```csharp
// Объявление функций
int Add(int a, int b) => a + b;

int Multiply(int a, int b)
{
    return a * b;
}

// Вызов
var result = Add(5, 3);
Console.WriteLine(result);
```

## Классы

```csharp
public class Calculator
{
    public int Add(int a, int b) => a + b;
    
    public int Subtract(int a, int b) => a - b;
}

var calc = new Calculator();
Console.WriteLine(calc.Add(10, 5));
```

## Пространства имён

```csharp
// Можно использовать пространства имён
namespace MyScripts
{
    public class Helper
    {
        public static string Format(string s) => s.ToUpper();
    }
}

// Но это не обязательно для простых скриптов
```

## Организация большого скрипта

```csharp
// Импорты
#r "nuget: Newtonsoft.Json"
using Newtonsoft.Json;
using System.Collections.Generic;

// Константы
const string ConfigPath = "./config.json";

// Вспомогательные классы
public class Config { /* ... */ }

// Функции
Config LoadConfig() { /* ... */ }

// Основная логика
var config = LoadConfig();
Console.WriteLine(config.ToString());
```

## Советы по структуре

- Группируйте связанный код
- Используйте функции для повторяемой логики
- Выносите сложные типы в классы
- Разделяйте скрипты через `#load` если файл большой

## Связанные узлы

- [[csharp-scripts-loading.md]] — модульность
- [[csharp-scripts-best-practices.md]] — рекомендации
- [[csharp-scripts-basics.md]] — основы