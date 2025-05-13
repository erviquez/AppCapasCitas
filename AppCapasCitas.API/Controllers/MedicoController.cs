using AppCapasCitas.API.Data;
using AppCapasCitas.API.Models;
using AppCapasCitas.API.VM.Request;
using AppCapasCitas.API.VM.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppCapasCitas.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MedicoController : ControllerBase
    {
        private readonly CitasDbContext _context;
        public MedicoController(CitasDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var medicos = _context.Medico
                .Include(m => m.Usuario)
                .Where(m => m.Activo)
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    MedicoId = m.Usuario!.MedicoId ?? 0,
                    Nombre = m.Usuario!.Nombre,
                    Apellido = m.Usuario!.Apellido,
                    Telefono = m.Usuario!.Telefono,
                    Celular = m.Usuario!.Celular,
                    Direccion = m.Usuario!.Direccion,
                    Ciudad = m.Usuario!.Ciudad,
                    CodigoPais = m.Usuario!.CodigoPais,
                    Pais = m.Usuario!.Pais,
                    Estado = m.Usuario!.Estado,
                    UltimoLogin = m.Usuario!.UltimoLogin,
                    FechaCreacion = m.Usuario!.FechaCreacion,
                    FechaActualizacion = m.Usuario!.FechaActualizacion,
                    CreadoPor = m.Usuario!.CreadoPor,
                    ModificadoPor = m.Usuario!.ModificadoPor,                    
                    Email = m.Usuario!.Email,
                    CedulaProfesional = m.CedulaProfesional!,
                    Biografia = m.Biografia!,
                    // EspecialidadId = m.Especialidad!.Id,

                    // NombreEspecialidad = m.Especialidad!.Nombre,
                    
                })
                .ToList();
            return Ok(medicos);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var medico = _context.Medico
            
                .Include(m => m.Usuario)
               // .Include(m => m.Especialidad)
                .Select(m => new MedicoResponse
                {
                    Id = m.Id,
                    MedicoId = m.Usuario!.MedicoId ?? 0,
                    Nombre = m.Usuario!.Nombre,
                    Apellido = m.Usuario!.Apellido,
                    Telefono = m.Usuario!.Telefono,
                    Celular = m.Usuario!.Celular,
                    Direccion = m.Usuario!.Direccion,
                    Ciudad = m.Usuario!.Ciudad,
                    CodigoPais = m.Usuario!.CodigoPais,
                    Pais = m.Usuario!.Pais,
                    Estado = m.Usuario!.Estado,
                    UltimoLogin = m.Usuario!.UltimoLogin,
                    FechaCreacion = m.FechaCreacion,
                    FechaActualizacion = m.FechaActualizacion,
                    CreadoPor = m.CreadoPor,
                    ModificadoPor = m.ModificadoPor,                    
                    Email = m.Usuario!.Email,
                    CedulaProfesional = m.CedulaProfesional!,
                    Biografia = m.Biografia!,
                    // EspecialidadId = m.Especialidad!.Id,
                    // NombreEspecialidad = m.Especialidad!.Nombre,
                    
                })
                .FirstOrDefault(m => m.Id == id)
                
                ;
                
            if (medico == null)
            {
                return NotFound();
            }

            return Ok(medico);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var medico = _context.Medico.Find(id);

                
            if (medico == null)
            {
                return NotFound();
            }
            medico.Activo = false;
            _context.SaveChanges();           
            return Ok("Se elimino el medico con exito");
        }


        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] MedicoRequest medicoRequest)
        {
             var medico = _context.Medico.Find(id);
            if (medico == null)
            {
                return NotFound();
            }
            // Validate Usuario exists
            if (medico.Usuario == null)
            {
                return BadRequest("El usuario asociado al médico no existe");
            }
            // Update all fields from the medicoResponse
            medico.CedulaProfesional = medicoRequest.CedulaProfesional;
            medico.Biografia = medicoRequest.Biografia;
            medico.FechaActualizacion = DateTime.Now; // Typically you'd update this to current time
            medico.ModificadoPor = "CurrentUser"; // Should be set to the actual user making the change

            try
            {
                _context.SaveChanges();
                return Ok("Se actualizó el médico con éxito");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Ocurrió un error al actualizar el médico" + ex.Message);
            }
        }
        [HttpPost]
        public IActionResult Create([FromBody] MedicoRequest medicoRequest)
        {
            var medico = new Medico
            { // create all fields from the medicoResponse
   
                CedulaProfesional = medicoRequest.CedulaProfesional,
                Biografia = medicoRequest.Biografia,
                Activo = true,
                FechaCreacion = DateTime.Now, // Typically you'd update this to current time
                CreadoPor = "CurrentUser" // Should be set to the actual user making the changenge
            };           

            try
            {
                _context.Medico.AddAsync(medico);
                _context.SaveChanges();
                return Ok("Se Agrego el médico con éxito");
            }
            catch (Exception ex)
            {
                // Log the exception
                return StatusCode(500, "Ocurrió un error al actualizar el médico" + ex.Message);
            }
        }






    }
}
