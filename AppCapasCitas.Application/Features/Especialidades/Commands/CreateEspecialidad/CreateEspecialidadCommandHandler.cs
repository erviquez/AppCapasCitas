using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.CreateEspecialidad;

public class CreateEspecialidadCommandHandler : IRequestHandler<CreateEspecialidadCommand, Response<Guid>>
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<CreateEspecialidadCommandHandler> _appLogger;
    private readonly IValidator<CreateEspecialidadCommand> _validator;
    public CreateEspecialidadCommandHandler(
        IAsyncRepository<Especialidad> especialidadRepository,
        IUnitOfWork unitOfWork,
        IAppLogger<CreateEspecialidadCommandHandler> appLogger,
        IValidator<CreateEspecialidadCommand> validator)
    {
        _especialidadRepository = especialidadRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
    }
    public async Task<Response<Guid>> Handle(CreateEspecialidadCommand request, CancellationToken cancellationToken)
    {
        Response<Guid> response = new();
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

            // Crear la especialidad
            var especialidad = new Especialidad
            {
                Nombre = request.Nombre,
                Descripcion = request.Descripcion,
                CostoConsultaBase = request.CostoConsultaBase,
                FechaCreacion = DateTime.Now,
                CreadoPor = request.UsuarioCreacionId == Guid.Empty || request.UsuarioCreacionId is null
                    ? "system" // Asignar un valor por defecto si no se especifica
                    : request.UsuarioCreacionId.ToString()
            };

            // Guardar en la base de datos
            _especialidadRepository.AddEntity(especialidad);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            response.Data = especialidad.Id;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            _appLogger.LogError("Error al crear la especialidad");
            response.IsSuccess = false;
            response.Message = $"Error al crear la especialidad: {ex.Message}";
        }
        return response;
    }
}
