using System.Net;
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;
using AppCapasCitas.Application.Features.Usuarios.Queries.PaginationUsuario;
using AppCapasCitas.DTO.Response.Usuario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.DTO.Request.Usuario;
using AppCapasCitas.Application.Features.Usuarios.Commands.ConvertUsuario;
using AppCapasCitas.Application.Features.Identity.Queries.GetRoles;
using AppCapasCitas.Transversal.Common.Identity;
using AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioByIdentityId;
using AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;
using AppCapasCitas.Application.Features.Usuarios.Commands.UpdateUsuario;
using AppCapasCitas.DTO.Helpers;


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
        
    [HttpGet("getRoles", Name = "GetRoles")]
    [ProducesResponseType(typeof(Response<List<Role>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<List<string>>>> GetRoles()
    {
        var query = new GetRolesQuery();
        var response = await _mediator.Send(query);
        if (response.IsSuccess == false)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpGet("getAll", Name = "GetAllUsuarios")]
    [ProducesResponseType(typeof(Response<IReadOnlyList<UsuarioResponse>>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<IReadOnlyList<UsuarioResponse>>>> GetAllUsuarios()
    {
        var paginationUsuario = await _mediator.Send(new GetUsuarioListQuery());
        if (paginationUsuario.IsSuccess == false)
        {
            return NotFound(paginationUsuario);
        }
        return Ok(paginationUsuario);
    }

    [HttpGet("getUser/{usuarioId}", Name = "GetUsuarioById")]
    [ProducesResponseType(typeof(Response<UsuarioResponse>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<UsuarioResponse>>> GetUsuario(Guid usuarioId)
    {
        var query = new GetUsuarioByIdentityIdQuery { UsuarioId = usuarioId };
        var response = await _mediator.Send(query);
        if (response.IsSuccess == false)
        {
            return NotFound(response);
        }
        return Ok(response);
    }

    [HttpPut("convert", Name = "ConvertUsuario")]
    [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<bool>>> ConvertUsuario([FromBody] UsuarioConvertRequest usuarioRequest)
    {
        var qry = new ConvertUsuarioCommand
        {
            UsuarioId = usuarioRequest.UsuarioId,
            UsuarioAccion = usuarioRequest.UsuarioAccion,
            RoleId = usuarioRequest.RoleId
        };        
        var response = await _mediator.Send(qry);
        if (response.IsSuccess == false)
        {
            return BadRequest(response);
        }
        if (usuarioRequest.UsuarioId != Guid.Empty)
            await LogAccion(usuarioRequest.UsuarioId, usuarioRequest.UsuarioAccion);
        return Ok(response);
        
    }
    [HttpPut]
    [ProducesResponseType(typeof(Response<bool>), (int)HttpStatusCode.OK)]
    public async Task<ActionResult<Response<bool>>> Update([FromBody] UsuarioRequest usuarioRequest)
    {

        var qry = new UpdateUsuarioCommand
        {
            UsuarioId = usuarioRequest.UsuarioId,
            Nombre = usuarioRequest.Nombre,
            Apellido = usuarioRequest.Apellido,
            Telefono = usuarioRequest.Telefono,
            Celular = usuarioRequest.Celular,
            Direccion = usuarioRequest.Direccion,
            Ciudad = usuarioRequest.Ciudad,
            Estado = usuarioRequest.Estado,
            CodigoPais = usuarioRequest.CodigoPais,
            IsActive = usuarioRequest.Activo,
            UsuarioAccion = usuarioRequest.UsuarioAccion
        };
        
        var response = await _mediator.Send(qry);
        if (response.IsSuccess == false)
        {
            return BadRequest(response);
        }
        if (usuarioRequest.UsuarioId != Guid.Empty)
            await LogAccion(usuarioRequest.UsuarioId, usuarioRequest.UsuarioAccion);
        return Ok(response);        
    }

    private async Task LogAccion(Guid usuarioId, string? usuarioCreacion = "system", [System.Runtime.CompilerServices.CallerMemberName] string? methodName = null)
    {
        var logActionCommand = new ActionLogUsuarioCommand
        {
            UsuarioId = usuarioId,
            TipoAccion = methodName ?? "Desconocido",
            UsuarioCreacion = usuarioCreacion
        };
        await _mediator.Send(logActionCommand);
    }

}

