using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadById;

public class GetEspecialidadByIdQueryHandler : IRequestHandler<GetEspecialidadByIdQuery, Response<EspecialidadResponse>>
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IMapper _mapper;

    public GetEspecialidadByIdQueryHandler(IAsyncRepository<Especialidad> especialidadRepository, IMapper mapper)
    {
        _especialidadRepository = especialidadRepository;
        _mapper = mapper;
    }

    public async Task<Response<EspecialidadResponse>> Handle(GetEspecialidadByIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<EspecialidadResponse>();
        try
        {
            var especialidad = await _especialidadRepository.GetByIdAsync(request.EspecialidadId, cancellationToken);
            if (especialidad == null)
            {
                response.IsSuccess = false;
                response.Message = $"No se encontró la especialidad con el Id {request.EspecialidadId}.";
                return response;
            }

            response.Data = _mapper.Map<EspecialidadResponse>(especialidad);
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al obtener la especialidad: {ex.Message}";
        }

        return response;

        
    }
}
