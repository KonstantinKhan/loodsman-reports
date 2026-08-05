# Автоматизация с C# скриптами

## Работа с файлами

### Чтение файла

```csharp
var content = File.ReadAllText("input.txt");
Console.WriteLine(content);

// По строкам
foreach (var line in File.ReadAllLines("input.txt"))
{
    Console.WriteLine(line);
}
```

### Запись в файл

```csharp
File.WriteAllText("output.txt", "Hello World");

// Добавление
File.AppendAllText("output.txt", "\nNew line");
```

### Поиск файлов

```csharp
var files = Directory.GetFiles("./", "*.txt");
foreach (var file in files)
{
    Console.WriteLine(file);
}

// Рекурсивный поиск
var allFiles = Directory.GetFiles("./", "*.csx", SearchOption.AllDirectories);
```

## Работа с процессами

### Запуск процесса

```csharp
using var process = new Process
{
    StartInfo = new ProcessStartInfo
    {
        FileName = "ls",
        Arguments = "-la",
        RedirectStandardOutput = true,
        UseShellExecute = false
    }
};

process.Start();
var output = process.StandardOutput.ReadToEnd();
process.WaitForExit();
Console.WriteLine(output);
```

### Проверка кода выхода

```csharp
if (process.ExitCode != 0)
{
    Console.Error.WriteLine($"Process failed: {process.ExitCode}");
    Environment.Exit(1);
}
```

## Сетевые запросы

### HTTP GET

```csharp
#r "nuget: System.Net.Http.Json"
using System.Net.Http.Json;

var client = new HttpClient();
var data = await client.GetFromJsonAsync<ApiResponse>("https://api.example.com/data");
Console.WriteLine(data);
```

### HTTP POST

```csharp
var payload = new { Name = "Test", Value = 123 };
var response = await client.PostAsJsonAsync("https://api.example.com/create", payload);
response.EnsureSuccessStatusCode();
```

## Автоматизация задач

### Резервное копирование

```csharp
var source = "./data";
var backup = $"./backups/backup-{DateTime.Now:yyyyMMdd-HHmmss}";
Directory.CreateDirectory(backup);

CopyDirectory(source, backup);

void CopyDirectory(string src, string dest)
{
    foreach (var dir in Directory.GetDirectories(src))
    {
        CopyDirectory(dir, Path.Combine(dest, Path.GetFileName(dir)));
    }
    
    foreach (var file in Directory.GetFiles(src))
    {
        File.Copy(file, Path.Combine(dest, Path.GetFileName(file)));
    }
}
```

### Пакетная обработка

```csharp
var files = Directory.GetFiles("./input", "*.csv");
foreach (var file in files)
{
    Console.WriteLine($"Processing {file}...");
    ProcessCsv(file);
}
```

## Связанные узлы

- [[csharp-scripts-best-practices.md]] — best practices
- [[csharp-scripts-prototyping.md]] — прототипирование
- [[csharp-scripts-basics.md]] — основы