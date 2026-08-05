# Зависимости C# скриптов

## Директива #r

Ссылки на сборки:

```csharp
// Ссылка на GAC
#r "System.Data"
#r "System.Net.Http"

// Ссылка на локальную сборку
#r "./libs/MyLib.dll"

// Абсолютный путь
#r "/usr/local/lib/lib.dll"
```

## NuGet-пакеты

```csharp
// Загрузка пакета из NuGet
#r "nuget: Newtonsoft.Json, 13.0.1"
#r "nuget: Serilog"
#r "nuget: CsvHelper"

// Использование пакета
using Newtonsoft.Json;
var json = JsonConvert.SerializeObject(new { Name = "Test" });
```

## Ссылки на .NET runtime

```csharp
// Microsoft.NETCore.App
#r "nuget: Microsoft.NETCore.App"
```

## Управление зависимостями

### Через командную строку

```bash
dotnet script init
# Создает script.csx и script.csproj
```

### script.csproj

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <ItemGroup>
    <PackageReference Include="Newtonsoft.Json" Version="13.0.1" />
    <PackageReference Include="Serilog" Version="3.0.1" />
  </ItemGroup>
</Project>
```

## Множественные зависимости

```csharp
#r "nuget: Newtonsoft.Json"
#r "nuget: Serilog"
#r "nuget: CsvHelper"

using Newtonsoft.Json;
using Serilog;
using CsvHelper;
```

## Связанные узлы

- [[csharp-scripts-loading.md]] — загрузка файлов
- [[csharp-scripts-basics.md]] — основы