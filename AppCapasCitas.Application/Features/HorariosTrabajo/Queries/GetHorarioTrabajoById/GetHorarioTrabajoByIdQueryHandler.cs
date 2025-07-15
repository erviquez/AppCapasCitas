using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Helpers;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.HorariosTrabajo.Queries.GetHorarioTrabajoById;

public class GetHorarioTrabajoByIdQueryHandler : IRequestHandler<GetHorarioTrabajoByIdQuery, Response<HorarioTrabajoResponse>>
{
    private readonly IAsyncRepository<HorarioTrabajo> _horarioTrabajoRepository;
    private readonly IAppLogger<GetHorarioTrabajoByIdQueryHandler> _appLogger;

    public GetHorarioTrabajoByIdQueryHandler(IAsyncRepository<HorarioTrabajo> horarioTrabajoRepository, IAppLogger<GetHorarioTrabajoByIdQueryHandler> appLogger)
    {
        _horarioTrabajoRepository = horarioTrabajoRepository;
        _appLogger = appLogger;
    }

    public async Task<Response<HorarioTrabajoResponse>> Handle(GetHorarioTrabajoByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<HorarioTrabajoResponse>();
        try
        {
            var horarioTrabajo = await _horarioTrabajoRepository.GetByIdAsync(request.HorarioTrabajoId, cancellationToken);
            if (horarioTrabajo == null)
            {
                _appLogger.LogInformation($"No se encontró el horario de trabajo con el Id {request.HorarioTrabajoId}.");
                response.Message = $"No se encontró el horario de trabajo con el Id {request.HorarioTrabajoId}.";
                response.IsSuccess = false;
                return response; 
            }


            response.Data = new HorarioTrabajoResponse
            {
                HorarioTrabajoId = horarioTrabajo.Id,
                DiaSemana = horarioTrabajo.DiaSemana,
                HoraInicio = horarioTrabajo.HoraInicio,
                HoraFin = horarioTrabajo.HoraFin,
                MedicoId = horarioTrabajo.MedicoId
               

            };
            
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            var className = frame!.GetMethod()!.DeclaringType!.FullName;
            var lineNumber = frame.GetFileLineNumber();
            string errorMessage = $"Error en la Linea: {lineNumber} -> {ex.Message} ";

            _appLogger.LogError(ex.Message, ex);
            response.Message = "Ocurrió un error, revisar detalle.";
            
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure(className, errorMessage)
            };
        }

        return response;
    }


}
