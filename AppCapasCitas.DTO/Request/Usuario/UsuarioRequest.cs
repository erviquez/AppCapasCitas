using System;

namespace AppCapasCitas.DTO.Request.Usuario;

public class UsuarioRequest
{
    public Guid UserId { get; set; }
    public bool IsActive { get; set; }

}
