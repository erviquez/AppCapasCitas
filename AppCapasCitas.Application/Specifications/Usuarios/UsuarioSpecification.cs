using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Usuarios;

public class UsuarioSpecification : BaseSpecification<Usuario>
{
    public UsuarioSpecification(UsuarioSpecificationParams usuarioParams)
        : base(x =>
            (string.IsNullOrEmpty(usuarioParams.Search) ||
            (x.Nombre + " " + x.Apellido).Contains(usuarioParams.Search) ||
            x.Nombre!.Contains(usuarioParams.Search) ||
            x.Apellido!.Contains(usuarioParams.Search) ||
            x.Email!.Contains(usuarioParams.Search)) &&            
            (usuarioParams.IsActive == null || 
            (usuarioParams.IsActive.ToLower() == "true" && x.Activo) ||
            (usuarioParams.IsActive.ToLower() == "false" && !x.Activo))
            )

    {
        ApplyPaging(
            usuarioParams.PageSize * (usuarioParams.PageIndex - 1), usuarioParams.PageSize);
        AddOrderBy(p => p.IdentityId!); // Or another unique field if Id doesn't exist

        // Apply default ordering first to ensure consistent pagination
        // Then apply the requested sorting if specified
        if (!string.IsNullOrEmpty(usuarioParams.Sort))
        {
            switch (usuarioParams.Sort.ToLower())
            {
                case "nombreasc":
                    AddOrderBy(p => p.Nombre!);
                    break;
                case "nombredesc":
                    AddOrderByDescending(p => p.Nombre!);
                    break;
                case "apellidoasc":
                    AddOrderBy(p => p.Apellido!);
                    break;
                case "apellidodesc":
                    AddOrderByDescending(p => p.Apellido!);
                    break;
                case "createddateasc":
                    AddOrderBy(p => p.FechaCreacion!);
                    break;
                case "createddatedesc":
                    AddOrderByDescending(p => p.FechaCreacion!);
                    break;
                // Default case already handled by initial ordering
                default:
                    AddOrderBy(p => p.IdentityId!); // Default ordering
                    break;
            }

        }


    }


}
