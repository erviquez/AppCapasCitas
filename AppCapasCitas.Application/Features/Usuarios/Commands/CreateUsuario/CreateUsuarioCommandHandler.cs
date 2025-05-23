using System;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.VisualBasic;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, Response<RegistrationResponse>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<CreateUsuarioCommandHandler> _appLogger;
     private readonly IValidator<CreateUsuarioCommand> _validator;
    private readonly IEmailService _emailService;

    public CreateUsuarioCommandHandler(IAuthService authService, IAsyncRepository<Usuario> userRepository, IUnitOfWork unitOfWork, IAppLogger<CreateUsuarioCommandHandler> appLogger, IValidator<CreateUsuarioCommand> validator, IEmailService emailService)
    {
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
        _emailService = emailService;
    }

    public async Task<Response<RegistrationResponse>> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<RegistrationResponse>();
        await SendEmail(request);

        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        var userId = Guid.NewGuid();
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
            // 2. Registrar usuario en Identity
            var registrationRequest = new RegistrationRequest
            {
                Username = request.Username,
                Password = request.Password,
                Email = request.Email,
                RoleId = request.RoleId, // Asignar el ID del rol al usuario
            };
            var result = await _authService.Register(registrationRequest);
            if (result is null || !result.IsSuccess)
            {
                await ExecuteCompensations(compensations);
                response.IsSuccess = false;
                response.Message = "Falló el registro en Identity: " + result!.Message + " -> " + result!.Errors;
                return response;
                // Si el registro falla, lanzar una excepción para hacer rollback
                //throw new ApplicationException();
            }
            await _authService.CommitAsync(); // Asegurarse de que los cambios se guarden en Identity
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
                RolId = Guid.Parse(result.Data!.RoleId), // Asignar el ID del rol al usuario
                RolName = result.Data.RoleName, // Asignar el nombre del rol al usuario
            };
            // Asignar el ID del usuario creado en Identity al nuevo usuario
            usuario.IdentityId = new Guid(result.Data.UserId); // Asignar el ID de Identity al nuevo usuario
            _userRepository.AddEntity(usuario);
            userId = usuario.IdentityId;
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            response.IsSuccess = true;
            response.Message = "Usuario creado exitosamente";
            
            await SendEmail(request);

        }
        catch (Exception ex)
        {
            // Si ocurre un error, ejecutamos las compensaciones
            await _authService.DeleteUser(userId.ToString()); // Revertir el registro en Identity     
            await ExecuteCompensations(compensations);
            //await transaction.RollbackAsync();
            var message = "Falló la creación del usuario en la base de datos." + ex.InnerException?.Message;
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
    private async Task SendEmail(CreateUsuarioCommand request)
    {

        var email = new Email()
        {
            To = request.Email,
            Subject = "Mensaje de la Aplicación- Alta de usuario",
            Body = $"El usuario {request.Username} para {request.Nombre} {request.Apellido} ha sido creado con éxito"
        };
        var result = await _emailService.SendEmail(email);
        if (!result.IsSuccess)
        {
            _appLogger.LogError(result.Message!);
        }

            
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
