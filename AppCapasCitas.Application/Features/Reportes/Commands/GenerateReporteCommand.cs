
using AppCapasCitas.DTO.Response.Reporte;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Reports.Commands.GenerateReport;

public class GenerateReporteCommand : IRequest<Response<ReporteResponse>>
{
    public string TipoReporte { get; set; } = string.Empty; // "Pacientes", "Citas", "Pagos"
    public object? Parametros { get; set; }
}