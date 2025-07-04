using System.Net.Http.Headers;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Transversal.Common;
using Microsoft.Extensions.Options;

namespace AppCapasCitas.Infrastructure.Repositories.Shortner
{
    public class ShortnerService : IShortnerService
    {
        private readonly HttpClient _httpClient;
        private readonly IAppLogger<ShortnerService> _appLogger;
        private readonly ShortnerSettings _shortnerSettings;
        private readonly UrlsConfirmationSettings _urlsConfirmationSettings;
        public ShortnerService(HttpClient httpClient, IAppLogger<ShortnerService> appLogger, IOptions<ShortnerSettings> shortnerSettings, IOptions<UrlsConfirmationSettings> urlsConfirmationSettings)
        {
            _httpClient = httpClient;
            _appLogger = appLogger;
            _shortnerSettings = shortnerSettings.Value;
            _urlsConfirmationSettings = urlsConfirmationSettings.Value;
        }

        public async Task<Response<string>> ShortenUrlAsync(string url)
        {
            var response = new Response<string>();

            // var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(_shortnerSettings.BaseUrl),
                Headers =
                {
                    { "x-rapidapi-key", _shortnerSettings.ApiKey },
                    { "x-rapidapi-host", _shortnerSettings.Host },
                },
                Content = new FormUrlEncodedContent(new Dictionary<string, string>
                {
                    { "url", url },
                }),
            };
            var responseHttpClient = await _httpClient.SendAsync(request);
            responseHttpClient.EnsureSuccessStatusCode();
            var json = await responseHttpClient.Content.ReadAsStringAsync();

            using var doc = System.Text.Json.JsonDocument.Parse(json);
            if (doc.RootElement.TryGetProperty("result_url", out var resultUrl))
            {
                _appLogger.LogInformation($"URL acortada correctamente: {resultUrl.GetString()!}");
                var result = resultUrl.GetString() ?? url;
                response.IsSuccess = true;
                response.Data = result;
                return response;

            }
            response.IsSuccess = false;
            response.Message = "Error al acortar la URL";
            response.Data = url; // Retorna la URL original si falla            
            return response;
        }


        public async Task<Response<string>> CreateUrlAsync(string typeUrl,string[] parameters)
        {
            var response = new Response<string>();
            var url = string.Empty;
            if (parameters == null || parameters.Length == 0)
            {
                response.IsSuccess = false;
                response.Message = "No se proporcionaron parámetros para crear la URL";
                return response;
            }

            try
            {
                if (typeUrl == "UrlEmail")
                {
                    url = _urlsConfirmationSettings.UrlEmail ?? string.Empty;
                }
                else if (typeUrl == "UrlPhone")
                {
                    url = _urlsConfirmationSettings.UrlPhone ?? string.Empty;
                }
                else 
                {
                    response.IsSuccess = false;
                    response.Message = "Tipo de URL no soportado";
                    return response;
                }

                url = $"{url}?{string.Join("&", parameters)}";
                _appLogger.LogInformation($"URL creada: {url}");

                // Llamar al servicio de acortamiento de URL
                if (_shortnerSettings.IsEnabled && !string.IsNullOrEmpty(url) && !url.Contains("://localhost"))
                {
                    var result = await ShortenUrlAsync(url);
                    if (!result.IsSuccess)
                    {
                        response.IsSuccess = false;
                        response.Message = result.Message;
                        response.Data = url; // Retorna la URL original si falla
                        return response;
                    }
                    response.IsSuccess = true;
                    response.Data = url;
                    return response;
                }
            }
            catch (Exception ex)
            {
                _appLogger.LogError($"Error al acortar la URL: {ex.Message}");
            }
            _appLogger.LogError("Error al acortar la URL, se retornará la URL original");
            response.IsSuccess = false;
            response.Message = "Error al acortar la URL, se retornará la URL original";
            response.Data = url; // Retorna la URL original si falla            
            return response;
        }
    }
}