using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Specifications.Medicos;
using AppCapasCitas.Domain.Models;

namespace AppCapasCitas.Application.Services;

public class CostoConsultaService : ICostoConsultaService
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IAsyncRepository<MedicoEspecialidadHospital> _medicoEspecialidadRepository;
    private readonly IAsyncRepository<TipoConsulta> _tipoConsultaRepository;

    public CostoConsultaService(
        IAsyncRepository<Especialidad> especialidadRepository,
        IAsyncRepository<MedicoEspecialidadHospital> medicoEspecialidadRepository,
        IAsyncRepository<TipoConsulta> tipoConsultaRepository)
    {
        _especialidadRepository = especialidadRepository;
        _medicoEspecialidadRepository = medicoEspecialidadRepository;
        _tipoConsultaRepository = tipoConsultaRepository;
    }

    public async Task<decimal> CalcularCostoCitaAsync(Guid medicoId, Guid especialidadId, Guid? hospitalId = null, Guid? tipoConsultaId = null)
    {
        var costoBase = await ObtenerCostoBaseEspecialidadAsync(especialidadId);

        decimal costoFinal = costoBase;

        if (hospitalId.HasValue)
        {
            var costoEspecifico = await ObtenerCostoEspecificoMedicoAsync(medicoId, especialidadId, hospitalId.Value);
            if (costoEspecifico.HasValue)
            {
                costoFinal = costoEspecifico.Value;
            }
        }
        if (tipoConsultaId.HasValue)
        {
            var tipoConsulta = await _tipoConsultaRepository.GetEntityAsync(x => x.Id == tipoConsultaId.Value);
            if (tipoConsulta != null)
            {
                costoFinal *= tipoConsulta.MultiplicadorCosto;
            }
        }

        return Math.Round(costoFinal, 2);
    }

    public async Task<decimal> ObtenerCostoBaseEspecialidadAsync(Guid especialidadId)
    {
        var especialidad = await _especialidadRepository.GetEntityAsync(x => x.Id == especialidadId);
        return especialidad?.CostoConsultaBase ?? 50.00M; // Valor por defecto
    }
    //Patrón Specification
    public async Task<decimal?> ObtenerCostoEspecificoMedicoAsync(Guid medicoId, Guid especialidadId, Guid hospitalId)
    {
        var spec = new MedicoEspecialidadHospitalSpecification(medicoId, especialidadId, hospitalId);
        var medicoEspecialidades = await _medicoEspecialidadRepository.GetAllWithSpec(spec);
        return medicoEspecialidades.FirstOrDefault()?.CostoConsultaEspecifico;
    }
    // public async Task<decimal?> ObtenerCostoBaseSpecificationMedicoAsync(Guid medicoId, Guid especialidadId, Guid hospitalId)
    // {
    //     var spec = new BaseSpecification<MedicoEspecialidadHospital>(x => 
    //         x.MedicoId == medicoId && 
    //         x.EspecialidadId == especialidadId && 
    //         x.HospitalId == hospitalId &&
    //         x.Activo);
    //     var medicoEspecialidades = await _medicoEspecialidadRepository.GetAllWithSpec(spec);
    //     return medicoEspecialidades.FirstOrDefault()?.CostoConsultaEspecifico;
    // }
    
    // public async Task<decimal?> ObtenerCostoMedicoEspecialidadHospitalAsync(Guid medicoId, Guid especialidadId, Guid hospitalId)
    // {
    //     var medicoEspecialidadHospital = await _medicoEspecialidadRepository.GetEntityAsync(
    //         x => x.MedicoId == medicoId && 
    //              x.EspecialidadId == especialidadId && 
    //              x.HospitalId == hospitalId && 
    //              x.Activo);
    //     return medicoEspecialidadHospital?.CostoConsultaEspecifico;
    // }
}
