using System;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Specifications.Medicos;


public class MedicoEspecialidadHospitalSpecification : BaseSpecification<MedicoEspecialidadHospital>
{
    public MedicoEspecialidadHospitalSpecification(Guid medicoId, Guid especialidadId, Guid hospitalId)
        : base(x => 
            x.MedicoId == medicoId && 
            x.EspecialidadId == especialidadId && 
            x.HospitalId == hospitalId &&
            x.Activo)
    {
        AddInclude(x => x.MedicoNavigation!);
        AddInclude(x => x.EspecialidadNavigation!);
        AddInclude(x => x.HospitalNavigation!);
        AddInclude(x => x.CargoNavigation!);
    }
}