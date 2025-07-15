
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Medicos;

public class MedicoSpecification: BaseSpecification<Medico>
{
    public MedicoSpecification(MedicoSpecificationParams medicoParams)
    :base(x =>
    (string.IsNullOrEmpty(medicoParams.Search) || x.UsuarioNavigation!.Nombre!.Contains(medicoParams.Search!)) &&
    (string.IsNullOrEmpty(medicoParams.IsActive) || x.UsuarioNavigation!.Activo.ToString() == medicoParams.IsActive)
)
    {
        AddInclude(x => x.UsuarioNavigation!);
        AddInclude(x => x.MedicoEspecialidadHospitales!);
        AddInclude(x => x.HorariosTrabajo!);
        ApplyPaging(
            medicoParams.PageSize * (medicoParams.PageIndex -1),medicoParams.PageSize);
        // AddOrderBy(p => p.UsuarioNavigation != null ? p.Id : 0); // Or another unique field if Id doesn't exist
        AddOrderBy(p => p.Id);
        // Apply default ordering first to ensure consistent pagination
        // Then apply the requested sorting if specified
        if (!string.IsNullOrEmpty(medicoParams.Sort))
        {
            switch (medicoParams.Sort.ToLower())
            {
                case "nombreasc":
                    AddOrderBy(p => p.UsuarioNavigation!.Nombre!);
                    break;
                case "nombredesc":
                    AddOrderByDescending(p => p.UsuarioNavigation!.Nombre!);
                    break;
                case "apellidoasc":
                    AddOrderBy(p => p.UsuarioNavigation!.Apellido!);
                    break;
                case "apellidodesc":
                    AddOrderByDescending(p => p.UsuarioNavigation!.Apellido!);
                    break;
                case "createddateasc":
                    AddOrderBy(p => p.UsuarioNavigation!.FechaCreacion!);
                    break;
                case "createddatedesc":
                    AddOrderByDescending(p => p.UsuarioNavigation!.FechaCreacion!);
                    break;
                // Default case already handled by initial ordering
                default:
                    AddOrderBy(p => p.UsuarioNavigation != null ? p.Id : 0); // Default ordering
                    break;
            }

        }
    }
}

