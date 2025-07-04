using System;
using System.Linq.Expressions;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.GetPacienteById;

public class GetPacienteByIdQueryHandler : IRequestHandler<GetPacienteByIdQuery, Response<PacienteResponse>>
{
    private readonly IAsyncRepository<Paciente> _pacienteRepository;
    private readonly IAppLogger<GetPacienteByIdQueryHandler> _appLogger;
    private readonly IAuthService _authService;
     private readonly IMapper _mapper;

    public GetPacienteByIdQueryHandler(IAsyncRepository<Paciente> pacienteRepository, IAppLogger<GetPacienteByIdQueryHandler> appLogger, IAuthService authService, IMapper mapper)
    {
        _pacienteRepository = pacienteRepository;
        _appLogger = appLogger;
        _authService = authService;
        _mapper = mapper;
    }

    public async Task<Response<PacienteResponse>> Handle(GetPacienteByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<PacienteResponse>();

        try
        {          
             var includes = new List<Expression<Func<Paciente, object>>>
            {
                x => x.UsuarioNavigation!, 
            };
            var paciente = await _pacienteRepository.GetEntityAsync(
                                            x => x.Activo == true && x.Id == request.PacienteId,
                                            includes,
                                            true,
                                            cancellationToken: cancellationToken);
            var message = string.Empty;
            if (paciente == null)
            {
                message = $"No se encontró el paciente con el Id {request.PacienteId}.";
                _appLogger.LogInformation(message);
                response.Message = message;                
                return response;
            }


            var pacienteResponse = _mapper.Map<PacienteResponse>(paciente);
            response.Data = pacienteResponse;
            response.Message = "Paciente encontrado.";
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            var st = new System.Diagnostics.StackTrace(true);
            var frame = st.GetFrame(0); // Frame actual
            //var methodName = frame!.GetMethod()!.Name;
            //var fileName = frame!.GetFileName();
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
