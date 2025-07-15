using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Cita;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Queries.GetPagoById;

public class GetPagoByIdQueryHandler : IRequestHandler<GetPagoByIdQuery, Response<PagoResponse>>
{
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAsyncRepository<Cita> _citaRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetPagoByIdQueryHandler> _appLogger;

    public GetPagoByIdQueryHandler(
        IAsyncRepository<Pago> pagoRepository,
        IAsyncRepository<Paciente> pacienteRepository,
        IAsyncRepository<Cita> citaRepository,
        IMapper mapper,
        IAppLogger<GetPagoByIdQueryHandler> appLogger)
    {
        _pagoRepository = pagoRepository;
        _pacienteRepository = pacienteRepository;
        _citaRepository = citaRepository;
        _mapper = mapper;
        _appLogger = appLogger;
    }

    public async Task<Response<PagoResponse>> Handle(GetPagoByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<PagoResponse>();
        
        try
        {
            var pago = await _pagoRepository.GetEntityAsync(x => x.Id == request.PagoId && x.Activo);
            
            if (pago == null)
            {
                response.IsSuccess = false;
                response.Message = "Pago no encontrado";
                return response;
            }

            // Obtener información relacionada
            var paciente = await _pacienteRepository.GetEntityAsync(x => x.Id == pago.PacienteId);
            Cita? cita = null;
            if (pago.CitaId.HasValue)
            {
                cita = await _citaRepository.GetEntityAsync(x => x.Id == pago.CitaId.Value);
            }
            var pagoResponse = new PagoResponse
            {
                PagoId = pago.Id,
                Monto = pago.Monto,
                FechaPago = pago.FechaPago,
                MetodoPago = pago.MetodoPago,
                EstadoPago = pago.Estado,
                Comprobante = pago.Comprobante,
                Notas = pago.Notas,
                PacienteId = pago.PacienteId,
                PacienteNombre = $"{paciente?.UsuarioNavigation!.Nombre} {paciente?.UsuarioNavigation!.Apellido}",
                CitaId = pago.CitaId,
                CitaFechaHora = cita?.FechaHora,
                CitaMotivo = cita?.Motivo,
                FechaCreacion = pago.FechaCreacion,
                CreadoPor = pago.CreadoPor ?? "system", 
            };

            response.Data = pagoResponse;
            response.IsSuccess = true;
            response.Message = "Pago obtenido exitosamente";
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al obtener el pago: {ex.Message}";
            _appLogger.LogError(ex.Message, ex);
        }

        return response;
    }
}