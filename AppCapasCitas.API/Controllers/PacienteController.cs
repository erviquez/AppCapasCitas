
// using AppCapasCitas.API.VM.Response;
// using AppCapasCitas.Infrastructure.Data;
// using Microsoft.AspNetCore.Mvc;

// namespace AppCapasCitas.API.Controllers
// {
//     [Route("api/v1/[controller]")]
//     [ApiController]
//     public class PacienteController : ControllerBase
//     {
//         private readonly InfrastructureDbContext _context;

//         public PacienteController(InfrastructureDbContext context)
//         {
//             _context = context;
//         }

//         [HttpGet]
//         public IActionResult GetPacientes()
//         {
//             var pacientes = _context.Paciente
//                 .Where(p => p.Activo)
//                 .Select(p => new PacienteResponse
//                 {
//                     Id = p.Id,
//                     PacienteId = p.Usuario!.PacienteId ?? 0,
//                     Nombre = p.Usuario!.Nombre,
//                     Apellido = p.Usuario!.Apellido,
//                     Telefono = p.Usuario!.Telefono,
//                     Celular = p.Usuario!.Celular,
//                     Direccion = p.Usuario!.Direccion,
//                     Ciudad = p.Usuario!.Ciudad,
//                     CodigoPais = p.Usuario!.CodigoPais,
//                     Pais = p.Usuario!.Pais,
//                     Estado = p.Usuario!.Estado,
//                     UltimoLogin = p.Usuario!.UltimoLogin,
//                     FechaCreacion = p.Usuario!.FechaCreacion,
//                     FechaActualizacion = p.Usuario!.FechaActualizacion,
//                     CreadoPor = p.Usuario!.CreadoPor,
//                     ModificadoPor = p.Usuario!.ModificadoPor,                    
//                     Email = p.Usuario!.Email,
//                     FechaNacimiento = p.FechaNacimiento,
//                     Genero = p.Genero!,
//                     Alergias = p.Alergias,
//                     EnfermedadesCronicas = p.EnfermedadesCronicas,
//                     MedicamentosActuales = p.MedicamentosActuales,
//                     Activo = p.Activo
//                 })
            
//             .ToList();
//             if (pacientes == null || !pacientes.Any())
//             {
//                 return NotFound(new { Message = "No se encontraron pacientes." });
//             }
//             // Aquí iría la lógica para obtener los pacientes
//             return Ok(pacientes);
//         }
//     }
// }
