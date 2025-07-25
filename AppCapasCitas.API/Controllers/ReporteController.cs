using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Features.Reports.Commands.GenerateReport;
using AppCapasCitas.DTO.Configuration;
using AppCapasCitas.DTO.Request.Reporte;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AppCapasCitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReporteController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IReporteService _reporteService;

    public ReporteController(IMediator mediator, IReporteService reporteService)
    {
        _mediator = mediator;
        _reporteService = reporteService;
    }

    [HttpPost("pacientes")]
    public async Task<IActionResult> GenerarReportePacientes([FromBody] ReporteRequest request)
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "Pacientes",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);
        var file = File(response.Data!.FileContent, response.Data.ContentType, response.Data.FileName);
        return file;
    }

    [HttpPost("citas")]
    public async Task<IActionResult> GenerarReporteCitas([FromBody] ReporteCitaRequest request)
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "Citas",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);

        return File(response.Data!.FileContent, response.Data.ContentType, response.Data.FileName);
    }

    [HttpGet("pacientes/base64")]
    public async Task<IActionResult> GenerarReportePacientesBase64([FromQuery] ReporteRequest request)
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "Pacientes",
            Parametros = request
        };

        var response = await _mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPost("pacientes/personalizado")]
    public async Task<IActionResult> GenerarReportePacientesPersonalizado([FromBody] ReporteRequest request)
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "pacientesconfigurable",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);

        return File(response.Data!.FileContent, response.Data.ContentType, response.Data.FileName);
    }
    [HttpPost("medicos/personalizado")]
    public async Task<IActionResult> GenerarReporteMedicosPersonalizado([FromBody] ReporteRequest request)
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "medicos",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);   

        if (response.Data!.FileContent == null)
        {
            return NotFound("No se encontró el contenido del archivo.");
        }

        return File(response.Data.FileContent, response.Data.ContentType, response.Data.FileName);
    }
    [HttpGet("medico/Expediente/{id}")]
    public async Task<IActionResult> GenerarReporteMedicosPersonalizado( Guid id)
    {
        var request = new ReporteIdRequest { Id = id };
        var command = new GenerateReporteCommand
        {
            TipoReporte = "medicobyid",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);   

        if (response.Data!.FileContent == null)
        {
            return NotFound("No se encontró el contenido del archivo.");
        }

        return File(response.Data.FileContent, response.Data.ContentType, response.Data.FileName);
    }
    [HttpGet("medico/Expediente/")]
    public async Task<IActionResult> GenerarReporteMedicosPersonalizado( [FromBody] ReporteIdRequest  request )
    {
        var command = new GenerateReporteCommand
        {
            TipoReporte = "medicoexpediente",
            Parametros = request
        };

        var response = await _mediator.Send(command);

        if (!response.IsSuccess)
            return BadRequest(response);   

        if (response.Data!.FileContent == null)
        {
            return NotFound("No se encontró el contenido del archivo.");
        }

        return File(response.Data.FileContent, response.Data.ContentType, response.Data.FileName);
    }

    [HttpPost("medicos/expedientes-multiples")]
    public async Task<IActionResult> GenerarExpedientesMedicosMultiples([FromBody] ReporteMultipleRequest request)
    {
        try
        {
            var command = new GenerateReporteCommand
            {
                TipoReporte = "medicosmultiples",
                Parametros = request
            };

            var response = await _mediator.Send(command);

            if (!response.IsSuccess)
                return BadRequest(response);   

            if (response.Data!.FileContent == null)
            {
                return NotFound("No se encontró el contenido del archivo.");
            }

            return File(response.Data.FileContent, response.Data.ContentType, response.Data.FileName);
        }
        catch (Exception ex)
        {
            //_logger.LogError(ex, "Error al generar expedientes múltiples");
            return StatusCode(500, $"Error interno del servidor: {ex.Message}");
        }
    }


    [HttpGet("configuracion")]
    public IActionResult ObtenerConfiguracion([FromServices] IOptions<ReporteConfiguration> config)
    {
        return Ok(config.Value);
    }
}