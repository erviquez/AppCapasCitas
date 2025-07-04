using System;
using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public class Contacto : EntidadBase
{
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Parentesco { get; set; } = string.Empty;
    [Required]
    public Guid PacienteId { get; set; }
    public virtual Paciente? PacienteNavigation { get; set; } // Relación con Paciente (opcional)

}
