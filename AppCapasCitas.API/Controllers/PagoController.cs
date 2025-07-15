using AppCapasCitas.Application.Features.Pagos.Commands.CreatePago;
using AppCapasCitas.Application.Features.Pagos.Commands.DeletePago;
using AppCapasCitas.Application.Features.Pagos.Commands.UpdatePagoEstado;
using AppCapasCitas.Application.Features.Pagos.Queries.GetPagoById;
using AppCapasCitas.Application.Features.Pagos.Queries.GetPagosByPaciente;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PagoController : ControllerBase
{
    private readonly IMediator _mediator;

    public PagoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> CreatePago([FromBody] CreatePagoCommand command)
    {
        var response = await _mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpGet("{pagoId}")]
    public async Task<IActionResult> GetPagoById(Guid pagoId)
    {
        var query = new GetPagoByIdQuery(pagoId);
        var response = await _mediator.Send(query);
        return response.IsSuccess ? Ok(response) : NotFound(response);
    }

    [HttpGet("paciente/{pacienteId}")]
    public async Task<IActionResult> GetPagosByPaciente(Guid pacienteId)
    {
        var query = new GetPagosByPacienteQuery(pacienteId);
        var response = await _mediator.Send(query);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpPut("estado")]
    public async Task<IActionResult> UpdatePagoEstado([FromBody] UpdatePagoEstadoCommand command)
    {
        var response = await _mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }

    [HttpDelete("{pagoId}")]
    public async Task<IActionResult> DeletePago(Guid pagoId)
    {
        var command = new DeletePagoCommand(pagoId);
        var response = await _mediator.Send(command);
        return response.IsSuccess ? Ok(response) : BadRequest(response);
    }
}