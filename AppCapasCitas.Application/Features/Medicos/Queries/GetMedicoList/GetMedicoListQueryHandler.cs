using System;
using System.Linq.Expressions;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoList;

public class GetMedicoListQueryHandler : IRequestHandler<GetMedicoListQuery, Response<IReadOnlyList<MedicoResponse>>>
{

    private readonly IAsyncRepository<Medico> _medicoRepository;    
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetMedicoListQueryHandler> _appLogger;

    public GetMedicoListQueryHandler(IAsyncRepository<Medico> medicoRepository, IMapper mapper, IAppLogger<GetMedicoListQueryHandler> appLogger)
    {
        _medicoRepository = medicoRepository;
        _mapper = mapper;
        _appLogger = appLogger;
    }

    public async Task<Response<IReadOnlyList<MedicoResponse>>> Handle(GetMedicoListQuery request, CancellationToken cancellationToken)
    {
        string message = string.Empty;
        var response = new Response<IReadOnlyList<MedicoResponse>>();
        try
        {
            var includes = new List<Expression<Func<Medico, object>>>
            {
                x => x.Usuario!, // Include the related Usuario
                x => x.MedicoEspecialidadHospitales!
            };

            var medicos = await _medicoRepository.GetAsync(
                                                x => x.Activo == true,
                                                x => x.OrderBy(x => x.Id),
                                                includes,
                                                cancellationToken: cancellationToken);
            var totalCount = medicos.Count;
            if (totalCount == 0)
            {
                message = "No se encontraron medicos";
                _appLogger.LogWarning(message);
                
                response.Message = message;
                return response;
            }

            var medicoResponse = _mapper.Map<IReadOnlyList<MedicoResponse>>(medicos);
            message = "Lista de medicos obtenida correctamente. Total de medicos";
            _appLogger.LogInformation($"{message}: {0} - {1}", totalCount, DateTime.Now);
            response.IsSuccess = true;
            response.Message = message;
            response.Data = medicoResponse;
            
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
