using AppCapasCitas.Application.Features.Especialidades.Commands.CreateEspecialidad;
using AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidad;
using AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidadCosto;
using AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadById;
using AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadList;
using AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;
using AppCapasCitas.DTO.Request.Especialidad;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EspecialidadController : ControllerBase
{
    IMediator _mediator;

    public EspecialidadController(IMediator mediator)
    {
        _mediator = mediator;
    }
    [HttpGet("{especialidadId}")]
    public async Task<IActionResult> GetById(Guid especialidadId)
    {
        var query = new GetEspecialidadByIdQuery(especialidadId);
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {
            return NotFound(result);
        }
        return Ok(result);
    }
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var query = new GetEspecialidadListQuery();
        var result = await _mediator.Send(query);
        if (result.IsSuccess == false)
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] EspecialidadRequest request)
    {
        // Validar que el usuario no esté vacío
        if (request is null)
        {
            return BadRequest("El ID del usuario no puede estar vacío.");
        }
        var command = new CreateEspecialidadCommand
        {            
            Nombre = request!.Nombre,
            Descripcion = request.Descripcion,
            CostoConsultaBase = request.CostoConsultaBase,
            Activo = request.Activo,
            UsuarioCreacionId = request.usuarioCreacionId
        };        
    
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
        {
            return BadRequest(result);
        }
        if (result.Data != Guid.Empty)
            await LogAccion(result.Data, request.usuarioCreacionId.ToString(), nameof(Create));
        return CreatedAtAction(nameof(GetById), new { especialidadId = result.Data }, result);
    }
    [HttpPut("Update")]
    public async Task<IActionResult> Update(EspecialidadUpdateRequest request)
    {
        // Validar que el usuario no esté vacío
        if (request is null)
        {
            return BadRequest("El ID del usuario no puede estar vacío.");
        }
        var command = new UpdateEspecialidadCommand
        {
            EspecialidadId = request!.EspecialidadId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            CostoConsultaBase = request.CostoConsultaBase,
            Activo = request.Activo,
            UsuarioModificacionId = request.usuarioCreacionId
        };
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
        {
            return BadRequest(result);
        }
        if (request.EspecialidadId!= Guid.Empty)
            await LogAccion(request.EspecialidadId, request.usuarioCreacionId.ToString(), nameof(Update));
        return Ok(result);
    }
    [HttpPut("UpdateCosto")]
    public async Task<IActionResult> UpdateCosto(EspecialidadCostoRequest request)
    {
         // Validar que el usuario no esté vacío
        if (request is null)
        {
            return BadRequest("El ID del usuario no puede estar vacío.");
        }
        var command = new UpdateEspecialidadCostoCommand
        {
            EspecialidadId = request!.EspecialidadId,
            CostoConsultaBase = request.CostoConsultaBase,
            UsuarioModificacionId = request.UsuarioModificacionId
        };
        var result = await _mediator.Send(command);
        if (result.IsSuccess == false)
        {
            return BadRequest(result);
        }
        if (request.EspecialidadId!= Guid.Empty)
            await LogAccion(request.EspecialidadId, request.UsuarioModificacionId.ToString(), nameof(UpdateCosto));
        return Ok(result);
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
