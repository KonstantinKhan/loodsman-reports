# Best practices для C# скриптов

## Организация кода

### Разделяйте логику

```csharp
// Хорошо
#load "utils.csx"
#load "handlers.csx"

var data = LoadData();
var result = ProcessData(data);
SaveResult(result);
```

### Используйте функции

```csharp
// Хорошо
void LogInfo(string message) => Console.WriteLine($"[INFO] {message}");

// Вместо дублирования
Console.WriteLine($"[INFO] {msg1}");
Console.WriteLine($"[INFO] {msg2}");
```

## Обработка ошибок

```csharp
try
{
    var data = File.ReadAllText("data.json");
    ProcessData(data);
}
catch (FileNotFoundException ex)
{
    Console.Error.WriteLine($"File not found: {ex.Message}");
    Environment.Exit(1);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Environment.Exit(1);
}
```

## Логирование

```csharp
// Простое логирование
void Log(string level, string message) => 
    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");

LogInfo("Starting script");
LogError("Something went wrong");
```

## Тестирование скриптов

```csharp
// Тестовые функции
void TestAdd()
{
    var result = Add(2, 3);
    if (result != 5) throw new Exception("Test failed");
}

TestAdd();
```

## Проверка аргументов

```csharp
if (Args.Length == 0)
{
    Console.WriteLine("Usage: script.csx <input>");
    Environment.Exit(1);
}
```

## Использование using

```csharp
using var reader = new StreamReader("file.txt");
var content = reader.ReadToEnd();
// Автоматический dispose
```

## Комментирование кода

```csharp
// Однострочный комментарий

/*
  Многострочный
  комментарий
*/

/// <summary>
/// XML документация
/// </summary>
```

## Производительность

- Кэшируйте результаты повторных операций
- Используйте `StringBuilder` для конкатенации строк
- Избегайте ненужных аллокаций

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-automation.md]] — автоматизация
- [[csharp-scripts-prototyping.md]] — прототипирование