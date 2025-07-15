using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Cita;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Queries.GetPagosByPaciente;

public class GetPagosByPacienteQueryHandler : IRequestHandler<GetPagosByPacienteQuery, ResponseGeneric<IEnumerable<PagoResponse>>>
{
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAsyncRepository<Cita> _citaRepository;
    private readonly IAppLogger<GetPagosByPacienteQueryHandler> _appLogger;

    public GetPagosByPacienteQueryHandler(
        IAsyncRepository<Pago> pagoRepository,
        IAsyncRepository<Paciente> pacienteRepository,
        IAsyncRepository<Cita> citaRepository,
        IAppLogger<GetPagosByPacienteQueryHandler> appLogger)
    {
        _pagoRepository = pagoRepository;
        _pacienteRepository = pacienteRepository;
        _citaRepository = citaRepository;
        _appLogger = appLogger;
    }

    public async Task<ResponseGeneric<IEnumerable<PagoResponse>>> Handle(GetPagosByPacienteQuery request, CancellationToken cancellationToken)
    {
        var response = new ResponseGeneric<IEnumerable<PagoResponse>>();
        
        try
        {
            var pagos = await _pagoRepository.GetAsync(x => x.PacienteId == request.PacienteId && x.Activo, cancellationToken: cancellationToken);
            var paciente = await _pacienteRepository.GetEntityAsync(x => x.Id == request.PacienteId);

            if (paciente == null)
            {
                response.IsSuccess = false;
                response.Message = "Paciente no encontrado";
                return response;
            }

            var pagoResponses = new List<PagoResponse>();
            var pagosOrdenados = pagos.ToList();
            
            foreach (var pago in pagos.OrderByDescending(p => p.FechaPago))
            {
                Cita? cita = null;
                if (pago.CitaId.HasValue)
                {
                    cita = await _citaRepository.GetEntityAsync(x => x.Id == pago.CitaId.Value);
                }

                pagoResponses.Add(new PagoResponse
                {
                    PagoId = pago.Id,
                    Monto = pago.Monto,
                    FechaPago = pago.FechaPago,
                    MetodoPago = pago.MetodoPago,
                    EstadoPago = pago.Estado,
                    Comprobante = pago.Comprobante,
                    Notas = pago.Notas,
                    PacienteId = pago.PacienteId,
                    PacienteNombre = $"{paciente.UsuarioNavigation!.Nombre} {paciente.UsuarioNavigation.Apellido}",
                    CitaId = pago.CitaId,
                    CitaFechaHora = cita?.FechaHora,
                    CitaMotivo = cita?.Motivo,
                    FechaCreacion = pago.FechaCreacion,
                    CreadoPor = pago.CreadoPor ?? "system", 
                });
            }

            response.Data = pagoResponses;
            response.IsSuccess = true;
            response.Message = $"Se encontraron {pagoResponses.Count} pagos para el paciente";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al obtener pagos del paciente: {ex.Message}";
            _appLogger.LogError(ex.Message, ex);
        }

        return response;
    }
}