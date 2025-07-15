
using System.Net;
using AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;
using AppCapasCitas.Application.Features.Medicos.Commands.EditMedico;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoById;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByName;
using AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoList;
using AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;
using AppCapasCitas.Application.Features.Shared;
using AppCapasCitas.DTO.Request.Medico;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
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
        [ProducesResponseType(typeof(Response<IReadOnlyList<MedicoResponse>>), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<IReadOnlyList<MedicoResponse>>> GetAll()
        {
            var query = new GetMedicoListQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        [HttpGet("GetByName/{nombre}")]
        public async Task<ActionResult<IReadOnlyList<MedicoResponse>>> GetByName(string nombre)
        {
            var query = new GetMedicoByNameQuery(nombre);
            var result = await _mediator.Send(query);
            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("GetById/{medicoId}")]
        public async Task<IActionResult> GetById(Guid medicoId)
        {
            var query = new GetMedicoByIdQuery(medicoId);
            var result = await _mediator.Send(query);
            if (result.IsSuccess == false)
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpGet("pagination", Name = "GetPaginationMedico")]
        [ProducesResponseType(typeof(PaginationVm<MedicoResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<PaginationVm<MedicoResponse>>> GetPaginationMedico([FromQuery] PaginationMedicoQuery paginationMedicoQuery)
        {
            var paginationMedico = await _mediator.Send(paginationMedicoQuery);
            if (paginationMedico.IsSuccess == false)
            {
                return NotFound(paginationMedico);
            }   
            return Ok(paginationMedico);
        }

        [HttpPut]
        public async Task<ActionResult<Response<bool>>> Update([FromBody] MedicoRequest medicoRequest)
        {
            var command = new EditMedicoCommand
            {
                MedicoId = medicoRequest.MedicoId,
                CedulaProfesional = medicoRequest.CedulaProfesional,
                Biografia = medicoRequest.Biografia,
            };
            var response = await _mediator.Send(command);
            if (response.IsSuccess == false)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
        
        [HttpPost("CreateMedico")]
        public async Task<IActionResult> CreateMedico([FromBody] CreateMedicoCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        //
   
        // [HttpDelete("DisableMedico")]
        // public async Task<IActionResult> DeleteMedico([FromBody] DisableMedicoCommand command)
        // {
        //     var result = await _mediator.Send(command);
        //     return Ok(result);
        // }

        // [HttpPost("UpdateMedico")]
        // public async Task<IActionResult> UpdateMedico([FromBody] EditMedicoCommand command)
        // {            
        //     var result = await _mediator.Send(command);
        //     return Ok(result);
        // }
    }
}
