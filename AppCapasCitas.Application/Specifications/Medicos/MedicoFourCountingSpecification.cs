using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Medicos;

public class MedicoFourCountingSpecification: BaseSpecification<Medico>
{
    public MedicoFourCountingSpecification(MedicoSpecificationParams medicoParams)
    :base(x =>
    (string.IsNullOrEmpty(medicoParams.Search) || x.UsuarioNavigation!.Nombre!.Contains(medicoParams.Search!)) &&
    (string.IsNullOrEmpty(medicoParams.IsActive) || x.UsuarioNavigation!.Activo.ToString() == medicoParams.IsActive))
    {

    }
}