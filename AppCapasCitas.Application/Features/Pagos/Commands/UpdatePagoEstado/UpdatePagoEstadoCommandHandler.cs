using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.UpdatePagoEstado;

public class UpdatePagoEstadoCommandHandler : IRequestHandler<UpdatePagoEstadoCommand, Response<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAppLogger<UpdatePagoEstadoCommandHandler> _appLogger;
    private readonly IValidator<UpdatePagoEstadoCommand> _validator;

    public UpdatePagoEstadoCommandHandler(
        IUnitOfWork unitOfWork,
        IAsyncRepository<Pago> pagoRepository,
        IAppLogger<UpdatePagoEstadoCommandHandler> appLogger,
        IValidator<UpdatePagoEstadoCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _pagoRepository = pagoRepository;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<bool>> Handle(UpdatePagoEstadoCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            // 1. Validación
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = validationResult.Errors;
                return response;
            }

            // 2. Buscar el pago
            var pago = await _pagoRepository.GetEntityAsync(x => x.Id == request.PagoId && x.Activo);
            if (pago == null)
            {
                response.IsSuccess = false;
                response.Message = "Pago no encontrado";
                return response;
            }

            // 3. Actualizar estado
            pago.Estado = request.Estado;
            if (!string.IsNullOrEmpty(request.Notas))
            {
                pago.Notas = request.Notas;
            }
            pago.FechaActualizacion = DateTime.Now;
            pago.ModificadoPor = "Sistema"; // TODO: Obtener del contexto

            _pagoRepository.UpdateEntity(pago,cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync(cancellationToken);

            response.Data = true;
            response.IsSuccess = true;
            response.Message = "Estado del pago actualizado exitosamente";

            _appLogger.LogInformation($"Estado del pago actualizado: {pago.Id} - Nuevo estado: {request.Estado}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            response.IsSuccess = false;
            response.Message = $"Error al actualizar el estado del pago: {ex.Message}";
            _appLogger.LogError(ex.Message, ex);
        }

        return response;
    }
}