using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Queries.GetEspecialidadList;

public class GetEspecialidadListQueryHandler : IRequestHandler<GetEspecialidadListQuery, Response<List<EspecialidadResponse>>>
{
    private readonly IAsyncRepository<Especialidad> _especialidadRepository;
    private readonly IMapper _mapper;

    public GetEspecialidadListQueryHandler(IAsyncRepository<Especialidad> especialidadRepository, IMapper mapper)
    {
        _especialidadRepository = especialidadRepository;
        _mapper = mapper;
    }

    public async Task<Response<List<EspecialidadResponse>>> Handle(GetEspecialidadListQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<List<EspecialidadResponse>>();
        try
        {
            var especialidades =await _especialidadRepository.GetAllAsync(cancellationToken);
            if (especialidades == null || !especialidades.Any())
            {
                response.IsSuccess = false;
                response.Message = "No se encontraron especialidades.";
                return response;
            }
            
            var especialidadesResponse = _mapper.Map<List<EspecialidadResponse>>(especialidades);
            response.Data = especialidadesResponse;
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = $"Error al obtener la lista de especialidades: {ex.Message}";
        }
        return response;
    }
}
