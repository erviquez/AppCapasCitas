using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.DTO.Request.Pago;
using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.Transversal.Common;
using MediatR;
using System.Text.Json;

namespace AppCapasCitas.Application.Features.Reports.Commands.GenerateReport;

public class GenerateReporteCommandHandler : IRequestHandler<GenerateReporteCommand, Response<ReporteResponse>>
{
    private readonly IReporteService _reporteService;

    public GenerateReporteCommandHandler(IReporteService reporteService)
    {
        _reporteService = reporteService;
    }
    public async Task<Response<ReporteResponse>> Handle(GenerateReporteCommand request, CancellationToken cancellationToken)
    {
        try
        {
            switch (request.TipoReporte.ToLower())
            {
                case "medicosmultiples":
                    var expedientesMedicosMultiplesRequest = DeserializarParametros<ReporteMultipleRequest>(request.Parametros);
                    if (expedientesMedicosMultiplesRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de medicos nultiples"
                        };
                    }
                    return await _reporteService.GenerarMultiplesExpedientesMedicosAsync(expedientesMedicosMultiplesRequest);


                case "medicobyid":
                    var medicoIdRequest = DeserializarParametros<ReporteIdRequest>(request.Parametros);
                    if (medicoIdRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de medicos"
                        };
                    }
                    return await _reporteService.GenerarExpedienteMedicoAsync(medicoIdRequest);
                case "medicos":
                    var medicoConfRequest = DeserializarParametros<ReporteRequest>(request.Parametros);
                    if (medicoConfRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de medicos"
                        };
                    }
                    return await _reporteService.GenerarReporteConfigurableMedicosAsync(medicoConfRequest);
                
                case "pacientesconfigurable":
                    var pacienteConfRequest = DeserializarParametros<ReporteRequest>(request.Parametros);
                    if (pacienteConfRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de pacientes"
                        };
                    }
                    return await _reporteService.GenerarReporteConfigurablePacientesAsync(pacienteConfRequest);
                case "pacientes":
                    var pacienteRequest = DeserializarParametros<ReporteRequest>(request.Parametros);
                    if (pacienteRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de pacientes"
                        };
                    }
                    return await _reporteService.GenerarReportePacientesAsync(pacienteRequest);

                case "citas":
                    var citaRequest = DeserializarParametros<ReporteCitaRequest>(request.Parametros);
                    if (citaRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de citas"
                        };
                    }
                    return await _reporteService.GenerarReporteCitasAsync(citaRequest);

                case "pagos":
                    var pagoRequest = DeserializarParametros<ReportePagoRequest>(request.Parametros);
                    if (pagoRequest == null)
                    {
                        return new Response<ReporteResponse>
                        {
                            IsSuccess = false,
                            Message = "Parámetros inválidos para reporte de pagos"
                        };
                    }
                    return await _reporteService.GenerarReportePagosAsync(pagoRequest);

                case "especialidades":
                    return await _reporteService.GenerarReporteEspecialidadesAsync();

                default:
                    return new Response<ReporteResponse>
                    {
                        IsSuccess = false,
                        Message = "Tipo de reporte no válido"
                    };
            }
        }
        catch (JsonException ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error al procesar parámetros JSON: {ex.Message}"
            };
        }
        catch (Exception ex)
        {
            return new Response<ReporteResponse>
            {
                IsSuccess = false,
                Message = $"Error inesperado: {ex.Message}"
            };
        }
    }

    private T? DeserializarParametros<T>(object? parametros) where T : class, new()
    {
        try
        {
            if (parametros == null)
            {
                return new T(); // Retorna objeto con valores por defecto
            }

            string json;
            
            // Si ya es string, usarlo directamente
            if (parametros is string parametroString)
            {
                json = parametroString;
            }
            // Si es otro tipo, serializarlo primero
            else
            {
                json = JsonSerializer.Serialize(parametros);
            }

            // Validar que no esté vacío
            if (string.IsNullOrWhiteSpace(json) || json == "{}")
            {
                return new T();
            }

            return JsonSerializer.Deserialize<T>(json);
        }
        catch (JsonException)
        {
            // Si falla la deserialización, retornar null
            return null;
        }
    }

    // public async Task<Response<ReporteResponse>> Handle(GenerateReporteCommand request, CancellationToken cancellationToken)
    // {
    //     var x = 1;


    //     switch (request.TipoReporte.ToLower())
    //     {
    //         case "pacientes":
    //             return await _reporteService.GenerarReportePacientesAsync(
    //                 JsonSerializer.Deserialize<ReportePacienteRequest>(request.Parametros?.ToString() ?? "{}")!
    //             );

    //         case "citas":
    //             return await _reporteService.GenerarReporteCitasAsync(
    //                 JsonSerializer.Deserialize<ReporteCitaRequest>(request.Parametros?.ToString() ?? "{}")!
    //             );

    //         case "pagos":
    //             return await _reporteService.GenerarReportePagosAsync(
    //                 JsonSerializer.Deserialize<ReportePagoRequest>(request.Parametros?.ToString() ?? "{}")!
    //             );

    //         case "especialidades":
    //             return await _reporteService.GenerarReporteEspecialidadesAsync();

    //         default:
    //             return new Response<ReporteResponse>
    //             {
    //                 IsSuccess = false,
    //                 Message = "Tipo de reporte no válido"
    //             };
    //  }
    //     // return request.TipoReporte.ToLower() switch
    //     // {
    //     //     "pacientes" => await _reporteService.GenerarReportePacientesAsync(
    //     //         JsonSerializer.Deserialize<ReportePacienteRequest>(request.Parametros?.ToString() ?? "{}")!),
    //     //     "citas" => await _reporteService.GenerarReporteCitasAsync(
    //     //         JsonSerializer.Deserialize<ReporteCitaRequest>(request.Parametros?.ToString() ?? "{}")!),
    //     //     "pagos" => await _reporteService.GenerarReportePagosAsync(
    //     //         JsonSerializer.Deserialize<ReportePagoRequest>(request.Parametros?.ToString() ?? "{}")!),
    //     //     "especialidades" => await _reporteService.GenerarReporteEspecialidadesAsync(),
    //     //     _ => new Response<ReporteResponse>
    //     //     {
    //     //         IsSuccess = false,
    //     //         Message = "Tipo de reporte no válido"
    //     //     }
    //     // };
    // }
}

// Remove this class if not used elsewhere, as the correct class is ReporteResponse.