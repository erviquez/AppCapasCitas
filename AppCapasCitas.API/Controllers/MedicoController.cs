

using System.Net;
using System.Threading.Tasks;
using AppCapasCitas.API.VM.Response;
using AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByEntityId.GetMedicoById;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByIdentityId;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoList;
using AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;
using AppCapasCitas.Application.Features.Shared;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MedicoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var query = new GetMedicoListQuery();
            var result = await _mediator.Send(query);
            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }
            return Ok(result);


        }

        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var query = new GetMedicoByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }
            return Ok(result);
        }
        [HttpGet("GetByIdentityId/{identityId}")]
        public async Task<IActionResult> GetByIdentityId(Guid identityId)
        {
            var query = new GetMedicoByIdentityIdQuery(identityId);
            var result = await _mediator.Send(query);
            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("pagination",Name ="GetPaginationMedico")]
        [ProducesResponseType(typeof(PaginationVm<MedicoResponse>), (int) HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<MedicoResponse>>> GetPaginationMedico ([FromQuery] PaginationMedicoQuery paginationMedicoQuery)
        {
            var paginationMedico = await _mediator.Send(paginationMedicoQuery);
            return Ok(paginationMedico);
        }


        // [HttpDelete("{id}")]
        // public IActionResult Delete(int id)
        // {
        //     var medico = _context.Medico.Find(id);


        //     if (medico == null)
        //     {
        //         return NotFound();
        //     }
        //     medico.Activo = false;
        //     _context.SaveChanges();           
        //     return Ok("Se elimino el medico con exito");
        // }


        // [HttpPut("{id}")]
        // public IActionResult Update(int id, [FromBody] MedicoRequest medicoRequest)
        // {
        //      var medico = _context.Medico.Find(id);
        //     if (medico == null)
        //     {
        //         return NotFound();
        //     }
        //     // Validate Usuario exists
        //     if (medico.Usuario == null)
        //     {
        //         return BadRequest("El usuario asociado al médico no existe");
        //     }
        //     // Update all fields from the medicoResponse
        //     medico.CedulaProfesional = medicoRequest.CedulaProfesional;
        //     medico.Biografia = medicoRequest.Biografia;
        //     medico.FechaActualizacion = DateTime.Now; // Typically you'd update this to current time
        //     medico.ModificadoPor = "CurrentUser"; // Should be set to the actual user making the change

        //     try
        //     {
        //         _context.SaveChanges();
        //         return Ok("Se actualizó el médico con éxito");
        //     }
        //     catch (Exception ex)
        //     {
        //         // Log the exception
        //         return StatusCode(500, "Ocurrió un error al actualizar el médico" + ex.Message);
        //     }
        // }
        [HttpPost("CreateMedico")]
        public async Task<IActionResult> CreateMedico([FromBody] CreateMedicoCommand command)
        {            
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
