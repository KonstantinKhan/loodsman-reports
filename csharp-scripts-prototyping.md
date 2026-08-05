# Прототипирование с C# скриптами

## Быстрое экспериментирование

```csharp
// Тестируйте идеи без создания проекта
var list = new List<int> { 1, 2, 3, 4, 5 };
var evenNumbers = list.Where(x => x % 2 == 0).ToList();
Console.WriteLine(string.Join(", ", evenNumbers));
```

## Проверка алгоритмов

```csharp
// Сортировка пузырьком
int[] BubbleSort(int[] arr)
{
    var n = arr.Length;
    for (int i = 0; i < n - 1; i++)
    {
        for (int j = 0; j < n - i - 1; j++)
        {
            if (arr[j] > arr[j + 1])
            {
                (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
            }
        }
    }
    return arr;
}

var test = new[] { 64, 34, 25, 12, 22 };
var result = BubbleSort(test);
Console.WriteLine(string.Join(", ", result));
```

## API интеграция

```csharp
#r "nuget: System.Net.Http.Json"
using System.Net.Http.Json;

var client = new HttpClient();
// Тестируйте API без создания полноценного приложения
var response = await client.GetFromJsonAsync<dynamic>("https://api.github.com/users/github");
Console.WriteLine($"Name: {response.name}");
```

## LINQ запросы

```csharp
var products = new[]
{
    new { Name = "A", Price = 10, Category = "Electronics" },
    new { Name = "B", Price = 20, Category = "Books" },
    new { Name = "C", Price = 30, Category = "Electronics" }
};

// Группировка
var byCategory = products.GroupBy(p => p.Category);
foreach (var group in byCategory)
{
    Console.WriteLine($"{group.Key}: {string.Join(", ", group.Select(p => p.Name))}");
}

// Фильтрация и проекция
var expensiveElectronics = products
    .Where(p => p.Category == "Electronics" && p.Price > 15)
    .Select(p => new { p.Name, PriceWithTax = p.Price * 1.2m });
```

## Регулярные выражения

```csharp
using System.Text.RegularExpressions;

var text = "Email: test@example.com, Another: user@domain.org";
var emails = Regex.Matches(text, @"\b[\w.-]+@[\w.-]+\.\w+\b");
foreach (Match email in emails)
{
    Console.WriteLine(email.Value);
}
```

## JSON обработка

```csharp
#r "nuget: Newtonsoft.Json"
using Newtonsoft.Json;

var json = @"{""name"":""Test"",""value"":123}";
var obj = JsonConvert.DeserializeObject<dynamic>(json);
Console.WriteLine($"Name: {obj.name}, Value: {obj.value}");

var output = JsonConvert.SerializeObject(obj, Formatting.Indented);
Console.WriteLine(output);
```

## Итеративная разработка

```csharp
// Редактируйте и перезапускайте быстро
// Нет компиляции проекта — только скрипт
var data = LoadData();
// Добавьте логику
data = Transform(data);
// Проверьте результат
Console.WriteLine(JsonConvert.SerializeObject(data, Formatting.Indented));
```

## Преимущества прототипирования

- Быстрый запуск (dotnet-script script.csx)
- Отсутствие ceremony (нет Main, class, namespace)
- Лёгкий эксперимент с библиотеками
- Простое тестирование идей

## Связанные узлы

- [[csharp-scripts-best-practices.md]] — best practices
- [[csharp-scripts-automation.md]] — автоматизация
- [[csharp-scripts-basics.md]] — основы