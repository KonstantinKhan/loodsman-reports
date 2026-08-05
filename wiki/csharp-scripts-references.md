# Создание проекта

## Необходимые инструменты

- Visual Studio 2026 — https://visualstudio.microsoft.com/ru/vs/community/
- .NET SDK 8 — https://dotnet.microsoft.com/ru-ru/download/dotnet/8.0

## Тип проекта

Ограничений по типу проекта для пользовательских приложений нет, но самым стабильным и удобным в разработке является **консольное приложение на C#**.

Версию .NET нужно выбирать исходя из текущих требований к серверной части ЛОЦМАН:PLM — на момент написания это **.NET 8**.

`.csproj` минимального проекта:

```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>
</Project>
```

Проект типа «Консольное приложение» после создания содержит один файл `Program.cs` с методом `Main` — точкой входа в приложение.

## Внешние зависимости

Для описанного в этом разделе шаблона **дополнительные NuGet-пакеты не требуются** — используются только сборки базового класса .NET:

- `System.Text.Json` — сериализация/десериализация JSON (конфигурация, вывод отчёта);
- `System.Net.Http` / `System.Net.Http.Json` — HTTP-клиент для WebAPI.

Если конкретному отчёту нужны сторонние библиотеки — они подключаются стандартным способом через NuGet (`<PackageReference>` в `.csproj` или менеджер пакетов Visual Studio).

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура и классы шаблона
- [[csharp-scripts-basics.md]] — основы
