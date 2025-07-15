using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.FrontEnd.Proxy.Interfaces;
using AppCapasCitas.Transversal.Common;
using System.IO.Compression;
using System.Text;
using System.Text.Json;

namespace AppCapasCitas.FrontEnd.Proxy.Implementaciones;

public class ReporteProxy : IReporteProxy
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReporteProxy> _logger;
    private readonly JsonSerializerOptions _jsonOptions;


    public ReporteProxy(HttpClient httpClient, ILogger<ReporteProxy> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

    public async Task<Response<ReporteResponse>> GenerarExpedientesMedicosMultiplesAsync(List<string> medicosIds)
    {
        try
        {
            if (medicosIds == null || !medicosIds.Any())
            {
                return new Response<ReporteResponse>
                {
                    IsSuccess = false,
                    Message = "Debe seleccionar al menos un médico"
                };
            }

            var request = new ReporteMultipleRequest
            {
                Ids = medicosIds,
                GenerarZip = medicosIds.Count > 1
            };

            var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("/api/reporte/medicos/expedientes-multiples", content);

            if (response.IsSuccessStatusCode)
            {
                var fileBytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ??
                              (medicosIds.Count > 1
                                  ? $"Expedientes_Medicos_{DateTime.Now:yyyyMMdd_HHmmss}.zip"
                                  : $"Expediente_Medico_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

                var contentType = medicosIds.Count > 1 ? "application/zip" : "application/pdf";

                return new Response<ReporteResponse>
                {
                    IsSuccess = true,
                    Data = new ReporteResponse
                    {
                        FileName = fileName,
                        ContentType = contentType,
                        FileContent = fileBytes,
                        Base64Content = Convert.ToBase64String(fileBytes),
                        TotalRecords = medicosIds.Count
                    },
                    Message = $"Se generaron {medicosIds.Count} expediente(s) exitosamente"
                };
            }

            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al generar expedientes múltiples: {response.StatusCode}"
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
    /////////////////////////////
        /// <summary>
        /// Genera expedientes múltiples de médicos y los comprime en ZIP
        /// </summary>
        // public async Task<Response<ReporteResponse>> GenerarExpedientesMedicosMultiplesAsync(List<string> medicosIds)
        // {
        //     try
        //     {
        //         if (!medicosIds?.Any() == true)
        //         {
        //             return new Response<ReporteResponse>
        //             {
        //                 IsSuccess = false,
        //                 Message = "Debe seleccionar al menos un médico"
        //             };
        //         }

        //         // Si es solo uno, generar expediente individual
        //         if (medicosIds.Count == 1)
        //         {
        //             return await GenerarReporteMedicoByIdAsync(medicosIds.First());
        //         }

        //         // Para múltiples, crear ZIP
        //         var expedientes = new List<(string fileName, byte[] content)>();
        //         var errores = new List<string>();

        //         // Generar cada expediente
        //         foreach (var medicoId in medicosIds)
        //         {
        //             try
        //             {
        //                 var expediente = await GenerarReporteMedicoByIdAsync(medicoId);

        //                 if (expediente.IsSuccess && expediente.Data?.FileContent != null)
        //                 {
        //                     expedientes.Add((expediente.Data.FileName, expediente.Data.FileContent));
        //                 }
        //                 else
        //                 {
        //                     errores.Add($"Error generando expediente para médico {medicoId}: {expediente.Message}");
        //                 }
        //             }
        //             catch (Exception ex)
        //             {
        //                 errores.Add($"Error procesando médico {medicoId}: {ex.Message}");
        //             }
        //         }

        //         if (!expedientes.Any())
        //         {
        //             return new Response<ReporteResponse>
        //             {
        //                 IsSuccess = false,
        //                 Message = $"No se pudo generar ningún expediente. Errores: {string.Join("; ", errores)}"
        //             };
        //         }

        //         // Crear archivo ZIP
        //         var zipBytes = CrearArchivoZip(expedientes);
        //         var zipFileName = $"Expedientes_Medicos_{DateTime.Now:yyyyMMdd_HHmmss}.zip";

        //         var response = new Response<ReporteResponse>
        //         {
        //             IsSuccess = true,
        //             Data = new ReporteResponse
        //             {
        //                 FileName = zipFileName,
        //                 ContentType = "application/zip",
        //                 FileContent = zipBytes,
        //                 Base64Content = Convert.ToBase64String(zipBytes),
        //                 TotalRecords = expedientes.Count
        //             }
        //         };

        //         // Agregar advertencias si hubo errores parciales
        //         if (errores.Any())
        //         {
        //             response.Message = $"Se generaron {expedientes.Count} expedientes. Advertencias: {string.Join("; ", errores)}";
        //         }
        //         else
        //         {
        //             response.Message = $"Se generaron {expedientes.Count} expedientes exitosamente";
        //         }

        //         return response;
        //     }
        //     catch (Exception ex)
        //     {
        //         return new Response<ReporteResponse>
        //         {
        //             IsSuccess = false,
        //             Message = $"Error generando expedientes múltiples: {ex.Message}"
        //         };
        //     }
        // }

        // /// <summary>
        // /// Genera expedientes múltiples usando una sola llamada al API
        // /// </summary>
        // public async Task<Response<ReporteResponse>> GenerarExpedientesMedicosMultiplesOptimizadoAsync(List<string> medicosIds)
        // {
        //     try
        //     {
        //         if (!medicosIds?.Any() == true)
        //         {
        //             return new Response<ReporteResponse>
        //             {
        //                 IsSuccess = false,
        //                 Message = "Debe seleccionar al menos un médico"
        //             };
        //         }

        //         var request = new ExpedientesMedicosMultiplesRequest
        //         {
        //             Ids = medicosIds,
        //             GenerarZip = medicosIds.Count > 1
        //         };

        //         var content = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
        //         var response = await _httpClient.PostAsync("/api/reporte/medicos/expedientes-multiples", content);

        //         if (response.IsSuccessStatusCode)
        //         {
        //             var fileBytes = await response.Content.ReadAsByteArrayAsync();
        //             var fileName = response.Content.Headers.ContentDisposition?.FileName?.Trim('"') ?? 
        //                           (medicosIds.Count > 1 
        //                             ? $"Expedientes_Medicos_{DateTime.Now:yyyyMMdd_HHmmss}.zip"
        //                             : $"Expediente_Medico_{DateTime.Now:yyyyMMdd_HHmmss}.pdf");

        //             var contentType = medicosIds.Count > 1 ? "application/zip" : "application/pdf";

        //             return new Response<ReporteResponse>
        //             {
        //                 IsSuccess = true,
        //                 Data = new ReporteResponse
        //                 {
        //                     FileName = fileName,
        //                     ContentType = contentType,
        //                     FileContent = fileBytes,
        //                     Base64Content = Convert.ToBase64String(fileBytes),
        //                     TotalRecords = medicosIds.Count
        //                 },
        //                 Message = $"Se generaron {medicosIds.Count} expediente(s) exitosamente"
        //             };
        //         }

        //         return new Response<ReporteResponse>
        //         {
        //             IsSuccess = false,
        //             Message = $"Error al generar expedientes múltiples: {response.StatusCode}"
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new Response<ReporteResponse>
        //         {
        //             IsSuccess = false,
        //             Message = $"Error: {ex.Message}"
        //         };
        //     }
        // }

        /// <summary>
        /// Crea un archivo ZIP con los expedientes
        /// </summary>
        // private byte[] CrearArchivoZip(List<(string fileName, byte[] content)> archivos)
        // {
        //     using var memoryStream = new MemoryStream();
        //     using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //     {
        //         foreach (var (fileName, content) in archivos)
        //         {
        //             var entry = archive.CreateEntry(fileName, CompressionLevel.Optimal);
        //             using var entryStream = entry.Open();
        //             entryStream.Write(content, 0, content.Length);
        //         }
        //     }
        //     return memoryStream.ToArray();
        // }



        /// //////////////////////////////
    }