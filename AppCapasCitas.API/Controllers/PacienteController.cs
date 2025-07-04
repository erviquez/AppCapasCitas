using System.Net;
using AppCapasCitas.Application.Features.Pacientes.Queries.GetPacienteById;
using AppCapasCitas.Application.Features.Pacientes.Queries.PaginationPaciente;
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.DTO.Request.Paciente;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class PacienteController : ControllerBase
{
    private readonly IMediator _mediator;

    public PacienteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("GetByIdentityId/{pacienteId}")]
    public async Task<IActionResult> GetByIdentityId(Guid pacienteId)
    {
        var query = new GetPacienteByIdQuery(pacienteId);
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
    [HttpGet("pagination", Name = "GetPaginationPaciente")]
    [ProducesResponseType(typeof(PaginationVm<PacienteResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<PacienteResponse>>> GetPaginationPaciente([FromQuery] PaginationPacienteQuery paginationPacienteQuery)
    {
        var paginationPaciente = await _mediator.Send(paginationPacienteQuery);
        if (paginationPaciente.IsSuccess == false)
        {
            return NotFound(paginationPaciente);
        }   
        return Ok(paginationPaciente);
    }


}


