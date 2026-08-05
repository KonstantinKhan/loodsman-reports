// Простой отчёт: список объектов типа "Деталь" внутри выбранной "Сборочной единицы".
//
// ВАЖНО: в wiki (reports-api.md / objects-api.md / links-api.md / types-reference.md)
// точная форма запроса/ответа метода get-linked-objects-for-objects и точные имена
// полей ObjectDto не задокументированы (справочники — заглушки без описания полей).
// Ниже — первая, пробная версия: места-предположения помечены TODO. При первом
// реальном запуске сверьте фактический JSON (он печатается в stderr при ошибке
// разбора) и поправьте имена полей.

using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

const string TargetTypeName = "Деталь";

void Log(string level, string message)
{
    Console.Error.WriteLine($"[{DateTime.Now:HH:mm:ss}] [{level}] {message}");
}

if (Args.Count < 3)
{
    Console.Error.WriteLine("Usage: dotnet-script report.csx -- <apiKey> <baseUrl> <assemblyObjectId>");
    Console.Error.WriteLine("Example: dotnet-script report.csx -- MY_API_KEY https://loodsman.example.com 12345");
    Environment.Exit(1);
}

var apiKey = Args[0];
var baseUrl = Args[1].TrimEnd('/');
var assemblyObjectId = Args[2];

await RunAsync();

async Task RunAsync()
{
    try
    {
        using var http = new HttpClient();
        http.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", apiKey);

        // TODO: сверить с реальным swagger — форма тела запроса предположительная.
        var requestBody = new
        {
            objectIds = new[] { assemblyObjectId }
        };

        var requestJson = JsonSerializer.Serialize(requestBody);
        using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var url = $"{baseUrl}/api/v4/ObjectInfo/get-linked-objects-for-objects";
        Log("INFO", $"POST {url}");

        using var response = await http.PostAsync(url, content);
        var responseText = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            Log("ERROR", $"HTTP {(int)response.StatusCode}: {responseText}");
            Environment.Exit(1);
            return;
        }

        JsonDocument doc;
        try
        {
            doc = JsonDocument.Parse(responseText);
        }
        catch (JsonException ex)
        {
            Log("ERROR", $"Не удалось разобрать ответ как JSON: {ex.Message}");
            Console.Error.WriteLine(responseText);
            Environment.Exit(1);
            return;
        }

        using (doc)
        {
            var root = doc.RootElement;

            // Конверт ответа обычно содержит result/errorCode/errorMessage
            // (см. types-reference.md — паттерн Output DTO / Envelope).
            if (root.TryGetProperty("errorCode", out var errorCode) &&
                errorCode.ValueKind != JsonValueKind.Null &&
                !string.IsNullOrEmpty(errorCode.GetString()))
            {
                var errorMessage = root.TryGetProperty("errorMessage", out var em) ? em.GetString() : "";
                Log("ERROR", $"API вернул ошибку {errorCode.GetString()}: {errorMessage}");
                Environment.Exit(1);
                return;
            }

            var itemsElement = root.TryGetProperty("result", out var result) ? result : root;

            if (itemsElement.ValueKind != JsonValueKind.Array)
            {
                Log("ERROR", "Ожидался массив объектов в ответе, но получена другая структура.");
                Console.Error.WriteLine(responseText);
                Environment.Exit(1);
                return;
            }

            var found = 0;
            foreach (var item in itemsElement.EnumerateArray())
            {
                // TODO: сверить реальные имена полей (typeName/type/name/id) с ответом сервера.
                var typeName = TryGetString(item, "typeName") ?? TryGetString(item, "type");
                if (!string.Equals(typeName, TargetTypeName, StringComparison.Ordinal))
                {
                    continue;
                }

                var id = TryGetString(item, "id") ?? TryGetString(item, "objectId") ?? "?";
                var name = TryGetString(item, "name") ?? "(без имени)";

                Console.WriteLine($"{id} — {name}");
                found++;
            }

            Log("INFO", $"Найдено объектов типа \"{TargetTypeName}\": {found}");
        }
    }
    catch (HttpRequestException ex)
    {
        Log("ERROR", $"Ошибка запроса к серверу: {ex.Message}");
        Environment.Exit(1);
    }
}

string TryGetString(JsonElement element, string propertyName)
{
    if (element.ValueKind == JsonValueKind.Object &&
        element.TryGetProperty(propertyName, out var value) &&
        value.ValueKind == JsonValueKind.String)
    {
        return value.GetString();
    }
    return null;
}
