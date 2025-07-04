using System.ComponentModel.DataAnnotations;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public class Aseguradora : EntidadBaseAuditoria
{
    public string? Nombre { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Estado { get; set; }
    public string? Pais { get; set; }
    public string? Observaciones { get; set; }
//Relaciones
    [Required]
    public Guid PacienteId { get; set; }
    public virtual Paciente? PacienteNavigation { get; set; } 
}
