using System;

namespace AppCapasCitas.DTO.Response.Paciente;

public class ContactoResponse
{
    public Guid ContactoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Parentesco { get; set; } = string.Empty;

}
