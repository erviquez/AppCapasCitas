using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, Response<bool>>
{

    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<UpdateUsuarioCommandHandler> _appLogger;
    private readonly IValidator<UpdateUsuarioCommand> _validator;

    public UpdateUsuarioCommandHandler(IAsyncRepository<Usuario> userRepository, IUnitOfWork unitOfWork, IAppLogger<UpdateUsuarioCommandHandler> appLogger, IValidator<UpdateUsuarioCommand> validator)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<bool>> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();      
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

            var usuarioBd = await _userRepository.GetEntityAsync(x => x.Id == request.UsuarioId,null,false,cancellationToken );
            if (usuarioBd is null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe en la aplicación";
                return response;
            }
            // 2. Actualizar usuario
            usuarioBd.Nombre = request.Nombre;
            usuarioBd.Apellido = request.Apellido;
            usuarioBd.Telefono = request.Telefono;
            usuarioBd.Celular = request.Celular;
            usuarioBd.Direccion = request.Direccion;
            usuarioBd.Ciudad = request.Ciudad;
            usuarioBd.CodigoPais = request.CodigoPais;
            usuarioBd.Pais = request.Pais;
            usuarioBd.Activo = request.IsActive;
            usuarioBd.ModificadoPor = request.UsuarioAccion;     
              
            await _unitOfWork.SaveChangesAsync();
            response.IsSuccess = true;
            response.Data = true;
            response.Message = "Usuario editado exitosamente";

        }
        catch (Exception ex)
        {
            var message = "Falló la actualización del usuario en la base de datos.";
            _appLogger.LogError(message);
            response.IsSuccess = false;
            response.Message = message;
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure("Error", ex.InnerException?.Message ?? "Error desconocido")
            };
        }

        return response;
    }

}