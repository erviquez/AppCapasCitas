using System;
using AppCapasCitas.Application.Contracts.Persistence.Identity;

using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;

public class CreateMedicoCommandHandler:IRequestHandler<CreateMedicoCommand,Response<Guid>>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IAsyncRepository<Medico> _medicoRepository;
    private readonly IAuthService _authService;
    private readonly IAppLogger<CreateMedicoCommandHandler> _appLogger;
    private readonly IValidator<CreateMedicoCommand> _validator;

    public CreateMedicoCommandHandler(IUnitOfWork unitOfWork, IAsyncRepository<Usuario> userRepository, IAsyncRepository<Medico> medicoRepository, IAuthService authService, IAppLogger<CreateMedicoCommandHandler> appLogger, IValidator<CreateMedicoCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _medicoRepository = medicoRepository;
        _authService = authService;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<Guid>> Handle(CreateMedicoCommand request, CancellationToken cancellationToken)
    {
        var response = new Response<Guid>();
        // Iniciamos transacción local

        await using var transaction = await _unitOfWork.BeginTransactionAsync();

        var medicoId = Guid.Empty;
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
            // 2. Verificar usuario existente
            var usuarioExistente = await _userRepository.GetEntityAsync(
                 x => x.Id == request.UsuarioIdentityId, null, false);

            if (usuarioExistente != null)
            {
                response.IsSuccess = false;
                response.Message = "El usuario ya es médico";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Usuario", "El usuario ya es médico")
                };
                return response;
            }
            // 3. Crear médico (Base de Datos Local)
            var nuevoMedico = new Medico
            {
                CedulaProfesional = request.CedulaProfesional,
                Biografia = request.Biografia,
            };
            var medico = await _medicoRepository.AddAsync(nuevoMedico);
            await _unitOfWork.SaveChangesAsync();
            medicoId = medico.Id;

            // Acción de compensación si falla después
            compensations.Add(async () =>
            {
                _medicoRepository.DeleteEntity(nuevoMedico);
                await _unitOfWork.SaveChangesAsync();
            });

            // 4. Asignar rol (Base de Datos Identity - Externa)
            var rolId = await GetRoleIdByName("MEDICO");
            if (rolId == Guid.Empty)
            {
                await ExecuteCompensations(compensations);
                response.IsSuccess = false;
                response.Message = "Rol Médico no encontrado";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Rol", "Rol Médico no encontrado")
                };
                return response;
            }
            var rolAsignado = await _authService.AssignRoleToUser(
                request.UsuarioIdentityId.ToString(),
                rolId.ToString("D"));

            if (!rolAsignado.IsSuccess)
            {
                await ExecuteCompensations(compensations);
                response.IsSuccess = false;
                response.Message = "No se pudo asignar el rol";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Rol", "No se pudo asignar el rol")
                };
                return response;
            }

            // 5. Actualizar usuario (Base de Datos Local)
            var usuario = await _userRepository.GetEntityAsync(
                x => x.Id == request.UsuarioIdentityId, null, false);

            if (usuario == null)
            {
                await ExecuteCompensations(compensations);
                await _authService.RemoveRoleFromUser(request.UsuarioIdentityId.ToString(), rolId.ToString("D"));
                response.IsSuccess = false;
                response.Message = "Usuario no encontrado";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Usuario", "Usuario no encontrado")
                };
                return response;
            }

            usuario.Id = medico.Id;
            usuario.RoleId = rolId;
            usuario.RolName = "MEDICO";

            _userRepository.UpdateEntity(usuario);
            await _unitOfWork.CommitTransactionAsync();
            //eliminar rol de registrado al usuario
            var rolIdRegistrado = await GetRoleIdByName("REGISTRADO");
            if (rolIdRegistrado != Guid.Empty)
            {
                await _authService.RemoveRoleFromUser(request.UsuarioIdentityId.ToString(), rolIdRegistrado.ToString("D"));//remover el rol de paciente
            }
            response.IsSuccess = true;
            response.Message = "Médico creado exitosamente";
            response.Data = medico.Id;
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            // Si ocurre un error, ejecutamos las compensaciones
            await ExecuteCompensations(compensations);
            _appLogger.LogError($"Error al crear médico - {ex.Message}");
            response.IsSuccess = false;
            response.Message = "Error al crear médico";
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure("Error", $"Error al crear médico: {ex.Message}")
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

    private async Task<Guid> GetRoleIdByName(string roleName)
    {
       return await _authService.GetRoleIdByName(roleName);
    }
}


