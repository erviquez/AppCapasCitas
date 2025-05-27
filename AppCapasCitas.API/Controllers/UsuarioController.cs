using AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;
using AppCapasCitas.DTO.Response.Usuario;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsuarioController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpGet]
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
           
    }
}
