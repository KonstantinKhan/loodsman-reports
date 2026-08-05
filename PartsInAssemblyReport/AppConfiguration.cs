using System.Text.Json;
using System.Text.Json.Serialization;

namespace PartsInAssemblyReport
{
    public class AppConfiguration
    {
        public string ApiVersion { get; set; } = "4";
        public int RequestTimeoutSeconds { get; set; } = 60;
        public string AppServerHost { get; set; } = "http://localhost:8076";
        public string SessionId { get; set; } = string.Empty;

        [JsonPropertyName("object_ids")]
        public List<int> ObjectIds { get; set; } = new();

        [JsonPropertyName("params")]
        public Dictionary<string, object?> Params { get; set; } = new();

        public string GetStringParameterByName(string parameterName)
        {
            if (Params.TryGetValue(parameterName, out object? parameterValue))
                return parameterValue?.ToString() ?? "";

            return "";
        }

        public static AppConfiguration GetConfiguration(string[] arguments, string? userData)
        {
            try
            {
                var config = DeserializeFromJson(userData);
                ApplyCommandLineArguments(config, arguments);
                return config;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Ошибка при разборе конфигурации приложения", ex);
            }
        }

        private static AppConfiguration DeserializeFromJson(string? userData)
        {
            if (string.IsNullOrWhiteSpace(userData))
                return new AppConfiguration();

            var json = userData.TrimStart('\ufeff').Trim();

            return JsonSerializer.Deserialize<AppConfiguration>(json)
                   ?? throw new InvalidOperationException("JSON конфигурации пустой или некорректный");
        }

        private static void ApplyCommandLineArguments(AppConfiguration config, string[] arguments)
        {
            if (TryGetArgumentValue(arguments, "-a", out var host))
                config.AppServerHost = host;

            if (TryGetArgumentValue(arguments, "--session", out var sessionId))
                config.SessionId = sessionId;
        }

        private static bool TryGetArgumentValue(string[] arguments, string argumentKey, out string value)
        {
            value = string.Empty;

            for (int i = 0; i < arguments.Length - 1; i++)
            {
                if (string.Equals(arguments[i], argumentKey, StringComparison.OrdinalIgnoreCase))
                {
                    value = arguments[i + 1];
                    return true;
                }
            }

            return false;
        }
    }
}
