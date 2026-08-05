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

> **На заметку**: служба выполнения скриптов (`GlobalScriptService`) при сборке загруженного скрипта восстанавливает NuGet-зависимости только из своего локального офлайн-источника (`...\GlobalScriptService\data\uploads\<guid>\nuget-packages`) — доступа в интернет к nuget.org там нет. Любой внешний `PackageReference` в реальности даёт `NU1301` и падает сборка на сервере, даже если локально у разработчика всё собиралось без проблем (там NuGet.org доступен). Поэтому "дополнительные пакеты не нужны" — не просто рекомендация, а жёсткое ограничение окружения: где возможно, обходитесь BCL.

## Связанные узлы

- [[csharp-scripts-structure.md]] — структура и классы шаблона
- [[csharp-scripts-basics.md]] — основы
