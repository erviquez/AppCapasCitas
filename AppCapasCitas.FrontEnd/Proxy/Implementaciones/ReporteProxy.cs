using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class ReporteProxy : IReporteProxy
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ReporteProxy(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<Response<ReporteResponse>> GenerarReportePacientesAsync(ReporteRequest request)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/reporte/pacientes", content);
            
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
                              $"Reporte_Pacientes_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = "application/pdf",
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes)
                    }
                };
            }

            var errorContent = await response.Content.ReadAsStringAsync();
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<Response<ReporteResponse>> GenerarReportePacientesPersonalizadoAsync(ReporteRequest request)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/reporte/pacientes/personalizado", content);
            
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
                              $"Reporte_Pacientes_Personalizado_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = "application/pdf",
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes)
                    }
                };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte personalizado: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

        public async Task<Response<ReporteResponse>> GenerarReporteMedicosPersonalizadoAsync(ReporteRequest request)
    {
        try
        {
            request.FormatoSalida = "PDF"; // Aseguramos que el formato sea PDF
            request.FechaDesde = request.FechaDesde ?? DateTime.MinValue; // Aseguramos que FechaDesde tenga un valor
            request.FechaHasta = request.FechaHasta ?? DateTime.MaxValue; // Aseguramos que FechaHasta tenga un valor
            request.IncluirInactivos = request.IncluirInactivos; // Mantenemos el valor de IncluirInactivos
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/reporte/medicos/personalizado", content);
            
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
                              $"Reporte_Medicos_Personalizado_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = "application/pdf",
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes)
                    }
                };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte personalizado: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
    public async Task<Response<ReporteResponse>> GenerarReporteMedicoByIdAsync(string medicoId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"/api/reporte/medico/Expediente/{medicoId}");
            
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
                              $"Reporte_Medico_{medicoId}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = "application/pdf",
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes)
                    }
                };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte del médico: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
    

    public async Task<Response<ReporteResponse>> GenerarReporteCitasAsync(ReporteCitaRequest request)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/reporte/citas", content);
            
            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
                              $"Reporte_Citas_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = "application/pdf",
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes)
                    }
                };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte de citas: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<Response<ReporteResponse>> GenerarReportePacientesBase64Async(ReporteRequest request)
    {
        try
        {
            var queryString = $"?FiltroNombre={request.FiltroNombre}&IncluirInactivos={request.IncluirInactivos}";
            if (request.FechaDesde.HasValue)
                queryString += $"&FechaDesde={request.FechaDesde:yyyy-MM-dd}";
            if (request.FechaHasta.HasValue)
                queryString += $"&FechaHasta={request.FechaHasta:yyyy-MM-dd}";

            var response = await _httpClient.GetAsync($"/api/reporte/pacientes/base64{queryString}");
            
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<Response<ReporteResponse>>(jsonResponse, _jsonOptions);
                return result ?? new Response<ReporteResponse> { IsSuccess = false, Message = "Error al deserializar respuesta" };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar reporte Base64: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }

    public async Task<Response<object>> ObtenerConfiguracionReporteAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/api/reporte/configuracion");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var config = JsonSerializer.Deserialize<object>(content, _jsonOptions);
                
                return new Response<object>
                {
                    IsSuccess = true,
                    Data = config
                };
            }

            return new Response<object>
            {
                IsSuccess = false,
                Message = $"Error al obtener configuración: {response.StatusCode}"
            };
        }
        catch (Exception ex)
        {
            return new Response<object>
            {
                IsSuccess = false,
                Message = $"Error: {ex.Message}"
            };
        }
    }
}