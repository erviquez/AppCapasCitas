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


    /// <summary>
    /// Genera expedientes múltiples de médicos (versión client-side)
    /// </summary>
    Task<Response<ReporteResponse>> GenerarExpedientesMedicosMultiplesAsync(List<string> medicosIds);
   
    /// <summary>
    /// Genera expedientes múltiples de médicos (versión optimizada server-side)
    /// </summary>
   // Task<Response<ReporteResponse>> GenerarExpedientesMedicosMultiplesOptimizadoAsync(List<string> medicosIds);



    
}