using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Medicos;

public class MedicoSpecification: BaseSpecification<Usuario>
{
    public MedicoSpecification(MedicoSpecificationParams medicoParams)
    :base(x => 
    (x.MedicoId >0) && // Asegura que el Paciente no sea nulo
        string.IsNullOrEmpty(medicoParams.Search) || 
        x.Nombre!.Contains(medicoParams.Search!))
    {
        AddInclude(x => x.Medico!);
        ApplyPaging(
            medicoParams.PageSize * (medicoParams.PageIndex -1),medicoParams.PageSize);
        AddOrderBy(p => p.MedicoId!); // Or another unique field if Id doesn't exist

        // Apply default ordering first to ensure consistent pagination
        // Then apply the requested sorting if specified
        if (!string.IsNullOrEmpty(medicoParams.Sort))
        {
            switch (medicoParams.Sort.ToLower())
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
                    AddOrderBy(p => p.MedicoId!); // Default ordering
                    break;
            }

        }
    }
}

