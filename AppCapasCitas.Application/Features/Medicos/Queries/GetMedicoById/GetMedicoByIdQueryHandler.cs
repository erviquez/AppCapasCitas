using System.Linq.Expressions;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.GetMedicoById;

public class GetMedicoByIdQueryHandler : IRequestHandler<GetMedicoByIdQuery, Response<MedicoResponse>>
{
    private readonly IAsyncRepository<Medico> _medicoRepository;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetMedicoByIdQueryHandler> _appLogger;

    public GetMedicoByIdQueryHandler(IAsyncRepository<Medico> medicoRepository, IMapper mapper, IAppLogger<GetMedicoByIdQueryHandler> appLogger)
    {
        _medicoRepository = medicoRepository;
        _mapper = mapper;
        _appLogger = appLogger;
    }

    public async Task<Response<MedicoResponse>> Handle(GetMedicoByIdQuery request, CancellationToken cancellationToken)
    {

        var response = new Response<MedicoResponse>();
        try
        {
            var medicoIdentityId = request.IdentityId;
            var includes = new List<Expression<Func<Medico, object>>>
            {
                x => x.UsuarioNavigation!,
                x => x.HorariosTrabajo!,
                
                


                x => x.MedicoEspecialidadHospitales!
            };

            var medico = await _medicoRepository.GetEntityAsync(
                                            x => x.Activo == true && x.UsuarioNavigation!.Id == medicoIdentityId,
                                            includes,
                                            true,
                                            cancellationToken: cancellationToken);
            var message = string.Empty;
            if (medico == null)
            {

                message = $"No se encontró el médico con el Id {request.IdentityId}.";
                _appLogger.LogInformation(message);
                response.Message = message;
                
                return response;
            }

            var medicoResponse = _mapper.Map<MedicoResponse>(medico);
            response.Data = medicoResponse;
            response.Message = "Médico encontrado.";
            response.IsSuccess = true;
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
