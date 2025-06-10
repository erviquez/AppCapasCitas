using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Usuarios;

public class UsuarioForCountingSpecification:BaseSpecification<Usuario>
{
    public UsuarioForCountingSpecification(UsuarioSpecificationParams usuarioParams)
        : base(x =>
            (string.IsNullOrEmpty(usuarioParams.Search) ||
            (x.Nombre + " " + x.Apellido).Contains(usuarioParams.Search) ||
            x.Nombre!.Contains(usuarioParams.Search) ||
            x.Apellido!.Contains(usuarioParams.Search) ||
            x.Email!.Contains(usuarioParams.Search)) &&
            // x.Activo ==Convert.ToBoolean(usuarioParams.IsActive)    &&
            (usuarioParams.IsActive == null || 
            (usuarioParams.IsActive.ToLower() == "true" && x.Activo) ||
            (usuarioParams.IsActive.ToLower() == "false" && !x.Activo))
            )
    {

        
    }
    
}
