
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoByName;

public class GetMedicoByNameQuery:IRequest<Response<IReadOnlyList<MedicoResponse>>>
{
    public GetMedicoByNameQuery(string? nombre)
    {
        Nombre = nombre;

    }

    public string? Nombre { get; set; }
 
}
 