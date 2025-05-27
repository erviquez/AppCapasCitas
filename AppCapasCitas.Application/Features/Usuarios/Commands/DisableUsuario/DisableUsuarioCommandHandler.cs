
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.DisableUsuario;

public class DisableUsuarioCommandHandler : IRequestHandler<DisableUsuarioCommand, Response<bool>>
{
    private readonly IAuthService _authService;
    
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<DisableUsuarioCommand> _appLogger;

    public DisableUsuarioCommandHandler(IAuthService authService, IUnitOfWork unitOfWork, IAppLogger<DisableUsuarioCommand> appLogger)
    {
        _authService = authService;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
    }

    public async Task<Response<bool>> Handle(DisableUsuarioCommand request, CancellationToken cancellationToken)
    {
        Response<bool> response = new();
        string message = string.Empty;
        try
        {
            var usuarioIdentityBD = await _authService.GetApplicationUser(request.IdentityId.ToString());
            if (usuarioIdentityBD is null)
            {
                message = "No se encontró el usuario";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
            }
            var usuarioActualizado = new AuthRequest()
            {
                Id = request.IdentityId.ToString(),
                Active = request.Active
            };
            var resultUpdate = await _authService.UpdateApplicationUser(usuarioActualizado);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (!response.IsSuccess)
            {
                message = resultUpdate.Message!;
                response.IsSuccess = false;
                response.Message = message;
                response.Errors = resultUpdate.Errors;
            }
            response.IsSuccess = true;
            response.Message = resultUpdate.IsSuccess ? "Usuario Activado" : "Usuario desactivado";
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
