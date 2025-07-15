using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications.Medicos;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;

public class PaginationMedicoQueryHandler: IRequestHandler<PaginationMedicoQuery, ResponsePagination<IReadOnlyList<MedicoResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaginationMedicoQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponsePagination<IReadOnlyList<MedicoResponse>>> Handle(PaginationMedicoQuery request, CancellationToken cancellationToken)
    {
        var responsePagination = new ResponsePagination<IReadOnlyList<MedicoResponse>>();
        try
        {
              var medicoSpecificationParams = new MedicoSpecificationParams
              {
                  PageIndex = request.PageIndex,
                  PageSize = request.PageSize,
                  Search = request.Search,
                  Sort = request.Sort,
                  IsActive = request.IsActive // <-- Se agrega el filtro de activos/inactivos
                
                };
            var spec = new MedicoSpecification(medicoSpecificationParams);
            var medicos = await _unitOfWork.GetRepository<Medico>().GetAllWithSpec(spec);

            var specCount = new MedicoFourCountingSpecification(medicoSpecificationParams);
            var totalMedicos = await _unitOfWork.GetRepository<Medico>().CountAsyncWithSpec(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalMedicos) / Convert.ToDecimal(medicoSpecificationParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);           
            var medicoResponse = _mapper.Map<List<MedicoResponse>>(medicos);  
            responsePagination = new ResponsePagination<IReadOnlyList<MedicoResponse>>
            {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalMedicos,
                    IsSuccess = true,
                    Message = "Lista de medicos obtenida correctamente",
                    Data = medicoResponse
            };
        }
        catch (Exception ex)
        {
            responsePagination.IsSuccess = false;
            responsePagination.Message = ex.Message + " - Error al obtener la paginación de medicos" + ex.InnerException?.Message;
        }
        return responsePagination;
    }
    
}