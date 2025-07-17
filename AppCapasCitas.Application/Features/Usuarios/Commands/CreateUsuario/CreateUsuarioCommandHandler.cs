using System.Net;
using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Templates.Html;
using AppCapasCitas.Transversal.Common.Templates.Sms;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;

public class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, Response<RegistrationResponse>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAppLogger<CreateUsuarioCommandHandler> _appLogger;
    private readonly IValidator<CreateUsuarioCommand> _validator;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly IShortnerService _shortnerService;

    public CreateUsuarioCommandHandler(IAuthService authService, IAsyncRepository<Usuario> userRepository, IUnitOfWork unitOfWork, IAppLogger<CreateUsuarioCommandHandler> appLogger, IValidator<CreateUsuarioCommand> validator, IEmailService emailService, ISmsService smsService, IShortnerService shortnerService)
    {
        _authService = authService;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _appLogger = appLogger;
        _validator = validator;
        _emailService = emailService;
        _smsService = smsService;
        _shortnerService = shortnerService;
    }

    public async Task<Response<RegistrationResponse>> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<RegistrationResponse>();
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
                Celular = request.Celular,
            };
            var result = await _authService.Register(registrationRequest);
            if (result is null || !result.IsSuccess)
            {
                await ExecuteCompensations(compensations);
                response.IsSuccess = false;
                response.Message = "Falló el registro en Identity: " + result!.Message + " -> " + result!.Errors;
                return response;
            }
            // Agregar compensación para eliminar el usuario en caso de error posterior
            compensations.Add(() => _authService.DeleteUser(result.Data!.Id.ToString()));
            await _authService.CommitAsync(); // Asegurarse de que los cambios se guarden en Identity
            var usuario = new Usuario
            {
                Id = result.Data!.Id, // Asignar el ID del usuario creado en Identity
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
                RoleId = Guid.Parse(result.Data!.RoleId), // Asignar el ID del rol al usuario
                RolName = result.Data.RoleName, // Asignar el nombre del rol al usuario
            };
            //si existe UsuarioCreacion
            if (request.UsuarioCreacionId == Guid.Empty || request.UsuarioCreacionId is null)
            {
                usuario.CreadoPor = "system"; // Asignar el ID del usuario que crea el nuevo usuario
            }
            _userRepository.AddEntity(usuario);
            userId = usuario.Id;
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            response.Data = new RegistrationResponse
            {
                Id = userId,
                Username = request.Username,
                Email = request.Email,
                Token = result.Data.Token,
                RefreshToken = result.Data.RefreshToken,
                RoleId = result.Data.RoleId,
                RoleName = result.Data.RoleName,
                Success = true
            };
            response.IsSuccess = true;
            response.Message = "Usuario creado exitosamente";

            await SendEmail(response, request);
            await SendSms(usuario, result.Data.RefreshToken);

        }
        catch (Exception ex)
        {
            // Si ocurre un error, ejecutamos las compensaciones
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
    private async Task SendEmail(Response<RegistrationResponse> response, CreateUsuarioCommand request)
    {
        if (response.Data is null)
        {
            _appLogger.LogError("No se pudo enviar el correo electrónico porque la respuesta no contiene datos.");
            return;
        }
        var shortner = await CreateEmailConfirmationUrl(response.Data!.Id.ToString(),  response.Data.RefreshToken);

        var email = new Email()
        {
            To = response.Data!.Email,
            Subject = "Mensaje de la Aplicación- Alta de usuario",
            Body = EmailTemplates.GetTemplateAltaUsuarioConfirmEmail(
                $"{request.Nombre!} {request.Apellido}",
                request.Email,
                request.Username,
                response.Data.RoleName,
                shortner
                ),
            // Body = $"El usuario {request.Username} para {request.Nombre} {request.Apellido} ha sido creado con éxito"
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
    //Crear url de confirmación de email
    private async Task<string> CreateEmailConfirmationUrl(string userId, string token)
    {
        var tokenEncoded = WebUtility.UrlEncode(token);
        var result = await _shortnerService.CreateUrlAsync("UrlEmail", new[] { $"usuarioId={userId}", $"token={tokenEncoded}" });
        return result.Data!;
    }


    private async Task<string> CreatePhoneConfirmationUrl(string userId, string phoneNumber, string token)
    {
        var tokenEncoded = WebUtility.UrlEncode(token);
        var phoneEncoded = WebUtility.UrlEncode(phoneNumber);
        var result = await _shortnerService.CreateUrlAsync("UrlPhone", new[] { $"usuarioId={userId}", $"phone={phoneEncoded}", $"token={tokenEncoded}" } );
        return result.Data!;
    }
    private async Task SendSms(Usuario usuario, string token)
    {
        //var url = CreatePhoneConfirmationUrl(usuario.Id.ToString(), usuario.Celular, token);
        var shortner = await CreatePhoneConfirmationUrl(usuario.Id.ToString(), usuario.Celular, token);
        if (!string.IsNullOrEmpty(shortner))
        {
            var mensaje = SmsTemplates.GetTemplateConfirmacionTelefono(shortner);
            var sms = new Sms
            {
                Contact = usuario.Celular,
                Date = DateTime.Now,
                Message = mensaje
            };
            var result = await _smsService.SendSms(sms);
            if (!result.IsSuccess)
            {
                _appLogger.LogError(result.Message!);
            }
        }
        else
        {
            _appLogger.LogError($"Error al crear la URL de confirmación de teléfono para el usuario {usuario.Id}");
        }
    }
    public static bool EsSoloNumeros(string? valor)
    {
        return !string.IsNullOrEmpty(valor) && valor.All(char.IsDigit);
    }

}
