using System.Net.Http.Json;

namespace CSharp
{
    /// <summary>
    /// Клиент для работы с API системы Loodsman
    /// </summary>
    public sealed class LoodsmanApiClient : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly AppConfiguration _config;

        public LoodsmanApiClient(AppConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri($"{_config.AppServerHost}/api/v{_config.ApiVersion}/"),
                Timeout = TimeSpan.FromSeconds(_config.RequestTimeoutSeconds)
            };

            _httpClient.DefaultRequestHeaders.Add("web-loodsman-session", _config.SessionId);
        }

        /// <summary>
        /// GET /api/v{version}/ObjectInfo/get-prop-objects?objectList={idVersion}
        /// </summary>
        public async Task<List<ObjectInfo>> GetObjectInfoAsync(int idVersion)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<ObjectInfo>>(
                    $"ObjectInfo/get-prop-objects?objectList={idVersion}");

                return result ?? new List<ObjectInfo>();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Ошибка при получении информации об объекте {idVersion}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Превышено время ожидания при получении информации об объекте {idVersion}", ex);
            }
        }

        /// <summary>
        /// GET /api/v{version}/ObjectInfo/get-linked-fast?idVersion={idVersion}&amp;linkType={linkType}
        /// </summary>
        public async Task<List<ObjectInfo>> GetLinkedObjectsAsync(int idVersion, string linkType)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<ObjectInfo>>(
                    $"ObjectInfo/get-linked-fast?idVersion={idVersion}&linkType={Uri.EscapeDataString(linkType)}");

                return result ?? new List<ObjectInfo>();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Ошибка при получении связанных объектов для версии {idVersion}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Превышено время ожидания при получении связанных объектов для версии {idVersion}", ex);
            }
        }

        /// <summary>
        /// GET /api/v{version}/ObjectInfo/get-info-about-version-mode-3?idVersion={idVersion}
        /// </summary>
        public async Task<List<Attributes>> GetVersionAttributesAsync(int idVersion)
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<Attributes>>(
                    $"ObjectInfo/get-info-about-version-mode-3?idVersion={idVersion}");

                return result ?? new List<Attributes>();
            }
            catch (HttpRequestException ex)
            {
                throw new InvalidOperationException($"Ошибка при получении атрибутов версии {idVersion}", ex);
            }
            catch (TaskCanceledException ex)
            {
                throw new TimeoutException($"Превышено время ожидания при получении атрибутов версии {idVersion}", ex);
            }
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
        }
    }
}