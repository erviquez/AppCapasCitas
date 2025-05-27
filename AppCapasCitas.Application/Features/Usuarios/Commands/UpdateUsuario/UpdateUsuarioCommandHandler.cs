using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.UpdateUsuario;

public class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, Response<RegistrationResponse>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<UpdateUsuarioCommandHandler> _appLogger;
    private readonly IValidator<UpdateUsuarioCommand> _validator;

    public UpdateUsuarioCommandHandler(IAuthService authService, IAsyncRepository<Usuario> userRepository, IUnitOfWork unitOfWork, IAppLogger<UpdateUsuarioCommandHandler> appLogger, IValidator<UpdateUsuarioCommand> validator)
    {
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<RegistrationResponse>> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<RegistrationResponse>();
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        var userId = string.Empty;
        var compensations = new List<Func<Task>>();
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
            // 2. Validar si existe usuario en Identity
            var userIdentity = await _authService.GetApplicationUser(request.UserId.ToString());
            if (!userIdentity.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe en Identity";
                return response;
            }
            var userApp = await _userRepository.GetEntityAsync(x => x.IdentityId == request.UserId);
            if (userApp is null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario no existe en la aplicación";
                return response;
            }

            // 3. Actualizar usuario en Identity
            var authRequest = new AuthRequest()
            {
                Id = request.UserId.ToString(),
                Email = request.Email,
                Username = request.Username,
                Password = request.Password
            };
            var resultUpdateUserIdentity = await _authService.UpdateApplicationUser(authRequest);
            
            if (!resultUpdateUserIdentity.IsSuccess)
            {
                //await ExecuteCompensations(compensations);
                response.IsSuccess = false;
                response.Message = "Falló la actualizacion del usuario en Identity: " + resultUpdateUserIdentity!.Message + " -> " + resultUpdateUserIdentity!.Errors;
                return response;
                // Si el registro falla, lanzar una excepción para hacer rollback
                //throw new ApplicationException();
            }
            await _authService.CommitAsync(); // Asegurarse de que los cambios se guarden en Identity
            var getRoleName = await _authService.GetRoleIdByRoleId(request.RoleId);
            var usuario = new Usuario
            {
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Telefono = request.Telefono,
                Email = request.Email,
                Celular = request.Celular,
                Direccion = request.Direccion,
                Ciudad = request.Ciudad,
                Estado = request.Estado,
                CodigoPais = request.CodigoPais,
                Pais = request.Pais,
                Activo = true,
                FechaCreacion = DateTime.UtcNow,
                CreadoPor = "system", // Fallback para creación automática
                RolId = Guid.Parse(request.RoleId), // Asignar el ID del rol al usuario
                RolName = await _authService.GetRoleIdByRoleId(request.RoleId) // Asignar el nombre del rol al usuario
            };
            // Asignar el ID del usuario creado en Identity al nuevo usuario
           
            _userRepository.AddEntity(usuario);
            userId = request.UserId.ToString();
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            response.IsSuccess = true;
            response.Message = "Usuario creado exitosamente";

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
        finally
        {
            await transaction.DisposeAsync();
        }
        return response;
    }

    private async Task ExecuteCompensations(List<Func<Task>> compensations)
    {
        foreach (var compensation in compensations)
        {
            try
            {
                await compensation();
            }
            catch (Exception ex)
            {
                _appLogger.LogError($"Error ejecutando compensación - {ex.Message}");
            }
        }
    }
}