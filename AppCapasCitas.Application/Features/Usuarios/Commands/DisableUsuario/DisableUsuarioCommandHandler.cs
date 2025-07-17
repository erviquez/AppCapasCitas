
using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.DisableUsuario;

public class DisableUsuarioCommandHandler : IRequestHandler<DisableUsuarioCommand, Response<bool>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _usuarioRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<DisableUsuarioCommand> _appLogger;

    public DisableUsuarioCommandHandler(IAuthService authService, IAsyncRepository<Usuario> usuarioRepository, IUnitOfWork unitOfWork, IAppLogger<DisableUsuarioCommand> appLogger)
    {
        _authService = authService;
        _usuarioRepository = usuarioRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
    }

    public async Task<Response<bool>> Handle(DisableUsuarioCommand request, CancellationToken cancellationToken)
    {
        Response<bool> response = new();
        string message;
        try
        {
            // Validar que el usuario existe
            var usuarioIdentityBD = await _authService.GetApplicationUser(request.IdentityId.ToString());
            if (usuarioIdentityBD is null)
            {
                message = "No se encontró el usuario en Identity";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
                return response;
            }

            var usuarioActualizado = new AuthRequest()
            {
                UsuarioId = request.IdentityId,
                Active = request.Active
            };
            var resultUpdateIdentity = await _authService.UpdateApplicationUser(usuarioActualizado);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var usuarioBD = await _usuarioRepository.GetEntityAsync(x => x.Id == request.IdentityId);
            if (usuarioBD is null)
            {
                //Rolback si no se encuentra el usuario
                var activo = request.Active? false : true; 
                usuarioActualizado = new AuthRequest()
                {
                    UsuarioId = request.IdentityId,
                    Active = activo
                };
                // Intentar revertir el cambio en Identity
                resultUpdateIdentity = await _authService.UpdateApplicationUser(usuarioActualizado);                
                message = "No se encontró el usuario en la base de datos, se realizo rollback a identity";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
                return response;
            }
            // Actualizar el estado del usuario en la base de datos
            usuarioBD.Activo = request.Active;   
            var usuarioUpdate =  await _usuarioRepository.UpdateAsync(usuarioBD, cancellationToken);
            if (usuarioUpdate is null)
            {
                message = resultUpdateIdentity.Message!;
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
                response.Errors = resultUpdateIdentity.Errors;
            }
            else
            {
                message = "Usuario: " + (request.Active ? "Activo" : "Inactivo");
                _appLogger.LogInformation(message);
                response.IsSuccess = true;
                response.Message = message;
            }
        }
        catch (Exception ex)
        {
            message = "Falló la actualización del usuario en la base de datos." + ex.InnerException?.Message;
            _appLogger.LogError(message);
            response.IsSuccess = false;
            response.Message =message;
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure("Error", ex.InnerException?.Message ?? "Error desconocido")
            };
        
        }
        return response;


    }
}
