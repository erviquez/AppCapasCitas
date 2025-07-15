using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Pagos.Commands.CreatePago;

public class CreatePagoCommandHandler : IRequestHandler<CreatePagoCommand, Response<Guid>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsyncRepository<Pago> _pagoRepository;
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAsyncRepository<Cita> _citaRepository;
    private readonly IAppLogger<CreatePagoCommandHandler> _appLogger;
    private readonly IValidator<CreatePagoCommand> _validator;

    public CreatePagoCommandHandler(
        IUnitOfWork unitOfWork,
        IAsyncRepository<Pago> pagoRepository,
        IAsyncRepository<Paciente> pacienteRepository,
        IAsyncRepository<Cita> citaRepository,
        IAppLogger<CreatePagoCommandHandler> appLogger,
        IValidator<CreatePagoCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _pagoRepository = pagoRepository;
        _pacienteRepository = pacienteRepository;
        _citaRepository = citaRepository;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<Guid>> Handle(CreatePagoCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<Guid>();
        
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

            // 2. Validar que existe el paciente
            var paciente = await _pacienteRepository.GetEntityAsync(x => x.Id == request.PacienteId);
            if (paciente == null)
            {
                response.IsSuccess = false;
                response.Message = "El paciente especificado no existe";
                return response;
            }

            // 3. Validar que existe la cita (si se especifica)
            Cita? cita = null;
            if (request.CitaId.HasValue)
            {
                cita = await _citaRepository.GetEntityAsync(x => x.Id == request.CitaId.Value);
                if (cita == null)
                {
                    response.IsSuccess = false;
                    response.Message = "La cita especificada no existe";
                    return response;
                }

                // Validar que la cita pertenece al paciente
                if (cita.PacienteId != request.PacienteId)
                {
                    response.IsSuccess = false;
                    response.Message = "La cita no pertenece al paciente especificado";
                    return response;
                }
            }

            // 4. Crear el pago
            var pagoId = Guid.NewGuid();
            var pago = new Pago
            {
                Id = pagoId,
                Monto = request.Monto,
                FechaPago = request.FechaPago,
                MetodoPago = request.MetodoPago,
                Estado = "Completado",
                Comprobante = request.Comprobante,
                Notas = request.Notas,
                PacienteId = request.PacienteId,
                CitaId = request.CitaId,
                Activo = true,
                FechaCreacion = DateTime.Now,
                CreadoPor = request.UsuarioCreacionId == Guid.Empty || request.UsuarioCreacionId is null
                    ? "system" // Asignar un valor por defecto si no se especifica
                    : request.UsuarioCreacionId.ToString()
            };

            _pagoRepository.AddEntity(pago,cancellationToken);
            await _unitOfWork.SaveChangesAsync();
            await transaction.CommitAsync(cancellationToken);

            response.Data = pagoId;
            response.IsSuccess = true;
            response.Message = "Pago registrado exitosamente";

            _appLogger.LogInformation($"Pago creado exitosamente: {pagoId}");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            response.IsSuccess = false;
            response.Message = $"Error al crear el pago: {ex.Message}";
            _appLogger.LogError(ex.Message, ex);
        }

        return response;
    }
}