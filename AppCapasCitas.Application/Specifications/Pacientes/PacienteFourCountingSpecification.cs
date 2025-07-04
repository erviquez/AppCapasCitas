using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Pacientes;

public class PacienteFourCountingSpecification: BaseSpecification<Paciente>
{
    public PacienteFourCountingSpecification(PacienteSpecificationParams pacienteParams)
    :base(x =>
    (string.IsNullOrEmpty(pacienteParams.Search) || x.UsuarioNavigation!.Nombre!.Contains(pacienteParams.Search!)) &&
    (string.IsNullOrEmpty(pacienteParams.IsActive) || x.UsuarioNavigation!.Activo.ToString() == pacienteParams.IsActive))
    {

    }
}
