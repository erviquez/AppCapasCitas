using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidadCosto;

public class UpdateEspecialidadCostoCommandHandler : IRequestHandler<UpdateEspecialidadCostoCommand, Response<bool>>
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<UpdateEspecialidadCostoCommandHandler> _appLogger;
    private readonly IValidator<UpdateEspecialidadCostoCommand> _validator;

    public UpdateEspecialidadCostoCommandHandler(IAsyncRepository<Especialidad> especialidadRepository, IUnitOfWork unitOfWork, IAppLogger<UpdateEspecialidadCostoCommandHandler> appLogger, IValidator<UpdateEspecialidadCostoCommand> validator)
    {
        _especialidadRepository = especialidadRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<bool>> Handle(UpdateEspecialidadCostoCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        try
        {
            // Validar el comando
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Errores de validación";
                response.Errors = validationResult.Errors;
                return response;
            }

            // Obtener la especialidad existente
            var especialidad = await _especialidadRepository.GetByIdAsync(request.EspecialidadId, cancellationToken);
            if (especialidad == null)
            {
                response.IsSuccess = false;
                response.Message = $"No se encontró la especialidad con el Id {request.EspecialidadId}.";
                return response;
            }

            // Actualizar el costo de consulta base
            especialidad.CostoConsultaBase = request.CostoConsultaBase;
            especialidad.FechaActualizacion = DateTime.Now;
            especialidad.ModificadoPor = request.UsuarioModificacionId == Guid.Empty || request.UsuarioModificacionId is null
                ? "system" // Asignar un valor por defecto si no se especifica
                : request.UsuarioModificacionId.ToString();

            // Guardar los cambios en la base de datos
            _especialidadRepository.UpdateEntity(especialidad);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            response.Data = true;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al actualizar el costo de la especialidad: {ex.Message}";
            _appLogger.LogError("Error al actualizar el costo de la especialidad: {Message}", ex.Message);
            response.Errors = null;
        }
        return response;
    }
}
