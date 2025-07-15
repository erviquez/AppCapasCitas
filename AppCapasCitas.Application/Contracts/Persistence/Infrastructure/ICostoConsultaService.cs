using System;

namespace AppCapasCitas.Application.Contracts.Persistence.Infrastructure;

public interface ICostoConsultaService
{
    Task<decimal> CalcularCostoCitaAsync(Guid medicoId, Guid especialidadId, Guid? hospitalId = null, Guid? tipoConsultaId = null);
    Task<decimal> ObtenerCostoBaseEspecialidadAsync(Guid especialidadId);
    Task<decimal?> ObtenerCostoEspecificoMedicoAsync(Guid medicoId, Guid especialidadId, Guid hospitalId);
}