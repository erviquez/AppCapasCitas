
using AppCapasCitas.Infrastructure.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppCapasCitas.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PacienteController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PacienteController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // [HttpGet]
        // public async Task<IActionResult> GetAll()
        // {
        //     var result = await _mediator.Send(new GetPacienteListQuery());
            
        // }
    }

}
