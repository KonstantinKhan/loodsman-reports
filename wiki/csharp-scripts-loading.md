# Загрузка файлов в C# скриптах

## Директива #load

Загрузка кода из других файлов:

```csharp
#load "utils.csx"
#load "./helpers/functions.csx"
#load "../shared/common.csx"
```

## Структура модулей

### main.csx

```csharp
#load "utils.csx"
#load "data.csx"

// Использование функций из загруженных файлов
var result = ProcessData(GetData());
Console.WriteLine(result);
```

### utils.csx

```csharp
public static string FormatOutput(string input)
{
    return $"Output: {input}";
}
```

### data.csx

```csharp
public static string GetData()
{
    return "Sample data";
}

public static string ProcessData(string data)
{
    return FormatOutput(data);
}
```

## Организация файлов

```
project/
├── main.csx
├── scripts/
│   ├── utils.csx
│   └── helpers/
│       └── functions.csx
└── lib/
    └── common.csx
```

## Относительные пути

```csharp
#load "../shared/common.csx"
#load "./helpers/helper.csx"
#load "/absolute/path/to/script.csx"
```

## Циклические зависимости

Избегайте циклических загрузок:
```
A.csx loads B.csx
B.csx loads A.csx  // Ошибка!
```

## Связанные узлы

- [[csharp-scripts-references.md]] — зависимости
- [[csharp-scripts-structure.md]] — структура
- [[csharp-scripts-best-practices.md]] — best practices