using System;

namespace AppCapasCitas.DTO.Response.Paciente;

public class AseguradoraResponse
{
    public Guid AseguradoraId { get; set; }
    public string? Nombre { get; set; }
    public string? Telefono { get; set; }
    public string? Direccion { get; set; }
    public string? Ciudad { get; set; }
    public string? CodigoPostal { get; set; }
    public string? Estado { get; set; }
    public string? Pais { get; set; }
    public string? Observaciones { get; set; }
}
