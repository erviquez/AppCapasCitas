using System;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.EditMedico;

public class EditMedicoCommandHandler : IRequestHandler<EditMedicoCommand, Response<bool>>
{

    private readonly IUnitOfWork _unitOfWork;
    //private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IAsyncRepository<Medico> _medicoRepository;
    private readonly IAppLogger<EditMedicoCommandHandler> _appLogger;
    private readonly IValidator<EditMedicoCommand> _validator;

    public EditMedicoCommandHandler(IUnitOfWork unitOfWork, IAsyncRepository<Medico> medicoRepository, IAppLogger<EditMedicoCommandHandler> appLogger, IValidator<EditMedicoCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _medicoRepository = medicoRepository;
        _appLogger = appLogger;
        _validator = validator;
    }

    public async Task<Response<bool>> Handle(EditMedicoCommand request, CancellationToken cancellationToken)
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
            // 2. Verificar medico existente
            var medicoBd = await _medicoRepository.GetEntityAsync(
                 x => x.Id == request.MedicoId, null, false);
            if (medicoBd == null)
            {
                response.IsSuccess = false;
                response.Message = "El médico no existe";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Medico", "El médico no existe")
                };
                return response;
            }
            // 3. Actualizar médico (Base de Datos Local)
            medicoBd.CedulaProfesional = request.CedulaProfesional;
            medicoBd.Biografia = request.Biografia;
            var result = await _unitOfWork.SaveChangesAsync();
            if (result <= 0)
            {
                response.IsSuccess = false;
                response.Message = "Error al actualizar médico";
                response.Errors = new List<ValidationFailure>
                {
                    new ValidationFailure("Medico", "Error al actualizar médico")
                };
                return response;
            }
            response.IsSuccess = true;
            response.Message = "Médico actualizado con éxito";
            response.Data = true; // Indica que la actualización fue exitosa
            _appLogger.LogInformation("Médico actualizado con éxito");
           
        }
        catch (Exception ex)
        {
            _appLogger.LogError($"Error al actualizar médico - {ex.Message}");
            response.IsSuccess = false;
            response.Message = "Error al actualizar médico";
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure("Error", $"Error al crear médico: {ex.Message}")
            };
        }

        return response;
    }


}
