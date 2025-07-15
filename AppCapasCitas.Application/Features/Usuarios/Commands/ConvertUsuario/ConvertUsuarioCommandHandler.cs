using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Templates.Html;
using AppCapasCitas.Transversal.Common.Templates.Sms;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ConvertUsuario;

public class ConvertUsuarioCommandHandler : IRequestHandler<ConvertUsuarioCommand, Response<bool>>
{

    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAsyncRepository<Medico> _medicoRepository;
    private readonly IAppLogger<ConvertUsuarioCommandHandler> _appLogger;
    private readonly IValidator<ConvertUsuarioCommand> _validator;
    private readonly IAuthService _authService;
    private readonly IEmailService _emailService;
    private readonly ISmsService _smsService;
    private readonly IUnitOfWork _unitOfWork;

    public ConvertUsuarioCommandHandler(IAsyncRepository<Usuario> userRepository, IAsyncRepository<Paciente> pacienteRepository, IAsyncRepository<Medico> medicoRepository, IAppLogger<ConvertUsuarioCommandHandler> appLogger, IValidator<ConvertUsuarioCommand> validator, IAuthService authService, IEmailService emailService, ISmsService smsService, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _pacienteRepository = pacienteRepository;
        _medicoRepository = medicoRepository;
        _appLogger = appLogger;
        _validator = validator;
        _authService = authService;
        _emailService = emailService;
        _smsService = smsService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Response<bool>> Handle(ConvertUsuarioCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<bool>();
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        var compensations = new List<Func<Task>>();
        Guid rolIdActual = Guid.Empty;
        string rolNameActual = string.Empty;
        Guid roleIdNuevo = request.RoleId;
        string message = string.Empty;
        try
        {
            // 1. Validación
            var validationResult = _validator.Validate(request);
            if (!validationResult.IsValid)
            {
                message = "Errores de validación";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message; ;
                response.Errors = validationResult.Errors;
                return response;
            }
            

            // 2. Obtener usuario por IdentityId
            var usuario = await _userRepository.GetEntityAsync(x => x.Id == request.UsuarioId);
            if(roleIdNuevo == usuario?.RoleId)
            {
                message = "No se aplicaron cambios, el usuario ya es de tipo " + usuario.RolName;
                _appLogger.LogInformation(message);
                response.IsSuccess = false;
                response.Message = message;
                return response;
            }
            if (usuario == null)
            {
                message = "Usuario no encontrado";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
                return response;
            }
            // Obtener nombre del rol nuevo
            var rolNombreNuevo = await _authService.GetRoleIdByRoleId(roleIdNuevo.ToString());
            if (rolNombreNuevo == null || string.IsNullOrEmpty(rolNombreNuevo.Data))
            {
                message = "No se pudo obtener el nombre del rol nuevo-Error en identity";
                _appLogger.LogError(message);
                response.IsSuccess = false;
                response.Message = message;
                return response;
            }
            //Obtener rol actual del usuario--por si hay rollback
            rolIdActual = usuario.RoleId;
            rolNameActual = usuario.RolName;
            // 3. Convertir usuario
            usuario.Id = request.UsuarioId;
            usuario.RoleId = roleIdNuevo;//Obtener nuevo rol
            usuario.RolName = rolNombreNuevo.Data!;
            usuario.FechaActualizacion = DateTime.Now;
            usuario.Activo = true;
            usuario.ModificadoPor = request.UsuarioAccion;
            //update usuario en la base de datos
            _userRepository.UpdateEntity(usuario, cancellationToken);
            if (rolNombreNuevo.Data == "Paciente" || rolNameActual == "Paciente")
            {
                var paciente = await _pacienteRepository.GetEntityAsync(x => x.Id == usuario.Id);
                if (paciente is not null)
                {
                    // actualizar paciente
                    paciente.Activo = paciente.Activo ? false : true;
                    paciente.FechaActualizacion = DateTime.Now;
                    paciente.ModificadoPor = request.UsuarioAccion;
                    _pacienteRepository.UpdateEntity(paciente, cancellationToken);
                }
                else
                {
                    // agregar nuevo paciente
                    _pacienteRepository.AddEntity(new Paciente
                    {
                        Id = usuario.Id,
                        FechaCreacion = DateTime.Now,
                        CreadoPor = request.UsuarioAccion,
                        Activo = true
                    }, cancellationToken);
                }
            }
            else if (rolNombreNuevo.Data == "Medico" || rolNameActual == "Paciente")
            {
                var medico = await _medicoRepository.GetEntityAsync(x => x.Id == usuario.Id);
                if (medico is not null)
                {
                    // actualizar médico
                    medico.Activo = medico.Activo ? false : true;
                    medico.FechaActualizacion = DateTime.Now;
                    medico.ModificadoPor = request.UsuarioAccion;
                    _medicoRepository.UpdateEntity(medico, cancellationToken);
                }
                else
                {
                    // agregar nuevo médico
                    _medicoRepository.AddEntity(new Medico
                    {
                        Id = usuario.Id,
                        FechaCreacion = DateTime.Now,
                        CreadoPor = request.UsuarioAccion,
                        Activo = true
                    }, cancellationToken);
                }
            }

            // 4. Asigna el rol al usuario
            // Si el usuario ya tiene un rol asignado, lo eliminamos antes de asignar el nuevo rol
            var deleteRoles = await _authService.RemoveAllRolesFromUser(usuario.Id.ToString());
            // Si la eliminación del rol falla, revertimos los cambios
            if (!deleteRoles)
            {
                await ExecuteCompensations(compensations);// Revertir cambios si falla la eliminación del rol actual
            }
            var roleResult = await AssignRoleToUser(usuario.Id.ToString(), usuario.RoleId.ToString());
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            if (!roleResult)
            {
                await ExecuteCompensations(compensations);
            }
            // 5. Guardar cambios en la base de datos
            await _unitOfWork.CommitTransactionAsync(cancellationToken);
            // Enviar correo de confirmación
            if (!string.IsNullOrEmpty(usuario.Email))
                await SendEmailChangeRole(usuario, rolNameActual);
            // Enviar SMS de confirmación
            //Validar formato correcto del número de celular
            if (usuario.Celular.Length >= 10 && usuario.Celular.Length <= 15 && EsSoloNumeros(usuario.Celular))    
                await SendSmsChangeRole(usuario, rolNameActual);            

            response.Data = true;
            response.IsSuccess = true;
            response.Message = $"Usuario cambío de {rolNameActual} y fue asignado como {rolNombreNuevo.Data} exitosamente";
        }
        catch (Exception ex)
        {
            await ExecuteCompensations(compensations);
            string messageError = ex.InnerException?.Message ?? ex.Message;
            // Si ocurre un error, revertir el registro en Identity
            var usuarioIdentity = await _authService.GetApplicationUser(request.UsuarioId.ToString());
            if (usuarioIdentity != null)
            {
                // Revertir el rol del usuario a su estado anterior
                // Reasigna el rol al usuario
                var roleResult = await AssignRoleToUser(rolIdActual.ToString(), rolNameActual!);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                if (!roleResult)
                {
                    messageError += $" Error al asignar el rol del usuario ";
                }
            }
            _appLogger.LogError(messageError);
            response.IsSuccess = false;
            response.Message = messageError;
        }
        finally
        {
            await transaction.DisposeAsync();
        }
        return response;
    }
    private async Task SendEmailChangeRole(Usuario usuario, string rolNameActual)
    {
        var nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}";
        var body = EmailTemplates.GetTemplateCambioRol(nombreCompleto, usuario.Email, rolNameActual, usuario.RolName);
        var email = new Email()
        {
            To = usuario.Email,
            Subject = $"Mensaje de la Aplicación- Aplicación de nuevo perfil {usuario.RolName}",
            Body = body

        };
        var result = await _emailService.SendEmail(email);
        if (!result.IsSuccess)
        {
            _appLogger.LogError(result.Message!);
        }
    }
    private async Task<bool> AssignRoleToUser(string userId, string roleId)
    {
        var result = await _authService.AssignRoleToUser(userId, roleId);
        if (!result.IsSuccess)
        {
            _appLogger.LogError($"Error al asignar rol al usuario: {result.Message}");
            return false;
        }
        return true;
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

    private async Task SendSmsChangeRole(Usuario usuario, string rolNameAnterior)
    {
        var nombreCompleto = $"{usuario.Nombre} {usuario.Apellido}";
        var mensaje = SmsTemplates.GetTemplateCambioRol(
            nombreCompleto, rolNameAnterior, usuario.RolName);
        var sms = new Sms
        {
            Contact = usuario.Celular, // O el campo correspondiente al número
            Date = DateTime.Now,
            Message = mensaje
        };

        var result = await _smsService.SendSms(sms);
        if (!result.IsSuccess)
        {
            _appLogger.LogError(result.Message!);
        }
    }

    public static bool EsSoloNumeros(string? valor)
    {
        return !string.IsNullOrEmpty(valor) && valor.All(char.IsDigit);
    }

}

