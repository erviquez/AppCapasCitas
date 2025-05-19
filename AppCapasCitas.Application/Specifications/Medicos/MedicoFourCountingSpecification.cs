using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Medicos;

public class MedicoFourCountingSpecification: BaseSpecification<Medico>
{
    public MedicoFourCountingSpecification(MedicoSpecificationParams medicoParams)
    :base(x => 
        string.IsNullOrEmpty(medicoParams.Search) || 
        x.Usuario!.Nombre!.Contains(medicoParams.Search))
    {

    }
}