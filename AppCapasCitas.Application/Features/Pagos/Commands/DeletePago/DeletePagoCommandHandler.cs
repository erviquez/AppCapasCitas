using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.DeletePago;

public class DeletePagoCommandHandler : IRequestHandler<DeletePagoCommand, Response<bool>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAppLogger<DeletePagoCommandHandler> _appLogger;

    public DeletePagoCommandHandler(
        IUnitOfWork unitOfWork,
        IAsyncRepository<Pago> pagoRepository,
        IAppLogger<DeletePagoCommandHandler> appLogger)
    {
        _unitOfWork = unitOfWork;
        _pagoRepository = pagoRepository;
        _appLogger = appLogger;
    }

    public async Task<Response<bool>> Handle(DeletePagoCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        
        try
        {
            var pago = await _pagoRepository.GetEntityAsync(x => x.Id == request.PagoId && x.Activo);
            if (pago == null)
            {
                response.IsSuccess = false;
                response.Message = "Pago no encontrado";
                return response;
            }

            // Soft delete
            pago.Activo = false;
            pago.FechaActualizacion = DateTime.Now;
            pago.ModificadoPor = "Sistema"; // TODO: Obtener del contexto

            _pagoRepository.UpdateEntity(pago,cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync(cancellationToken);

            response.Data = true;
            response.IsSuccess = true;
            response.Message = "Pago eliminado exitosamente";

            _appLogger.LogInformation($"Pago eliminado: {pago.Id}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            response.IsSuccess = false;
            response.Message = $"Error al eliminar el pago: {ex.Message}";
            _appLogger.LogError(ex.Message, ex);
        }

        return response;
    }
}