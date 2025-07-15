using AppCapasCitas.DTO.Request.Reporte;
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface IReporteProxy
{
    Task<Response<ReporteResponse>> GenerarReportePacientesAsync(ReporteRequest request);
    Task<Response<ReporteResponse>> GenerarReporteCitasAsync(ReporteCitaRequest request);
    Task<Response<ReporteResponse>> GenerarReportePacientesBase64Async(ReporteRequest request);
    Task<Response<object>> ObtenerConfiguracionReporteAsync();
    Task<Response<ReporteResponse>> GenerarReporteMedicosPersonalizadoAsync(ReporteRequest request);
    Task<Response<ReporteResponse>> GenerarReporteMedicoByIdAsync(string medicoId);
}