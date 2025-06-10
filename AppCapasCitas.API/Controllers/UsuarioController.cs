using System.Net;
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;
using AppCapasCitas.Application.Features.Usuarios.Queries.PaginationUsuario;
using AppCapasCitas.DTO.Response.Usuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class UsuarioController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<UsuarioResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<IReadOnlyList<UsuarioResponse>>> GetAll()
    {
        var query = new GetUsuarioListQuery();
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {
            return NotFound(result);
        }
        return Ok(result);
    }


    [HttpGet("pagination", Name = "GetPaginationUsuario")]
    [ProducesResponseType(typeof(PaginationVm<UsuarioResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<PaginationVm<UsuarioResponse>>> GetPaginationUsuario([FromQuery] PaginationUsuarioQuery paginationUsuarioQuery)
    {
        var paginationUsuario = await _mediator.Send(paginationUsuarioQuery);
        if (paginationUsuario.IsSuccess == false)
        {
            return NotFound(paginationUsuario);
        }
        return Ok(paginationUsuario);
    }


}

