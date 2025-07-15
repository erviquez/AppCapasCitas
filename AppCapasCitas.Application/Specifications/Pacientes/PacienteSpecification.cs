using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Pacientes;

public class PacienteSpecification: BaseSpecification<Paciente>
{
    public PacienteSpecification(PacienteSpecificationParams pacienteParams)
    :base(x =>
    (string.IsNullOrEmpty(pacienteParams.Search) || x.UsuarioNavigation!.Nombre!.Contains(pacienteParams.Search!)) &&
    (string.IsNullOrEmpty(pacienteParams.IsActive) || x.UsuarioNavigation!.Activo.ToString() == pacienteParams.IsActive)
)
    {
        AddInclude(x => x.UsuarioNavigation!);
        AddInclude(x => x.TipoSangreNavigation!);
        AddInclude(x => x.Aseguradoras!);
        AddInclude(x => x.Contactos!);

        

        //AddInclude(x => x.PacienteEspecialidadHospitales!);
        ApplyPaging(
            pacienteParams.PageSize * (pacienteParams.PageIndex -1),pacienteParams.PageSize);
        // AddOrderBy(p => p.UsuarioNavigation != null ? p.Id : 0); // Or another unique field if Id doesn't exist
        AddOrderBy(p => p.Id);
        // Apply default ordering first to ensure consistent pagination
        // Then apply the requested sorting if specified
        if (!string.IsNullOrEmpty(pacienteParams.Sort))
        {
            switch (pacienteParams.Sort.ToLower())
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
