using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidad;

public class UpdateEspecialidadCommandHandelr : IRequestHandler<UpdateEspecialidadCommand, Response<bool>>
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<UpdateEspecialidadCommandHandelr> _logger;
    private IValidator<UpdateEspecialidadCommand> _validator;

    public UpdateEspecialidadCommandHandelr(IAsyncRepository<Especialidad> especialidadRepository, IUnitOfWork unitOfWork, IAppLogger<UpdateEspecialidadCommandHandelr> logger, IValidator<UpdateEspecialidadCommand> validator)
    {
        _especialidadRepository = especialidadRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
    }

    public async Task<Response<bool>> Handle(UpdateEspecialidadCommand request, CancellationToken cancellationToken)
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

            // Actualizar los campos de la especialidad
            especialidad.Nombre = request.Nombre;
            especialidad.Descripcion = request.Descripcion;
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
            _logger.LogError("Error al actualizar la especialidad");
            response.IsSuccess = false;
            response.Message = $"Error al actualizar la especialidad: {ex.Message}";
        }
        return response;
        
        
    }
}
