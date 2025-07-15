using System;
using AppCapasCitas.DTO.Request.Pago;
using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.Application.Contracts.Persistence.Infrastructure;

public interface IReporteService
{
    Task<Response<ReporteResponse>> GenerarReportePacientesAsync(ReporteRequest request);
    Task<Response<ReporteResponse>> GenerarReporteConfigurablePacientesAsync(ReporteRequest request);
    Task<Response<ReporteResponse>> GenerarReporteCitasAsync(ReporteCitaRequest request);
    Task<Response<ReporteResponse>> GenerarReportePagosAsync(ReportePagoRequest request);
    Task<Response<ReporteResponse>> GenerarReporteEspecialidadesAsync();
    Task<Response<ReporteResponse>> GenerarReporteConfigurableMedicosAsync(ReporteRequest request);

    Task<Response<ReporteResponse>> GenerarExpedienteMedicoAsync(ReporteIdRequest request);
}