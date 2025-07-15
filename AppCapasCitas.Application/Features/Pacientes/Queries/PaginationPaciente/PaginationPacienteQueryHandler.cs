using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications.Pacientes;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Paciente;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.PaginationPaciente;

public class PaginationPacienteQueryHandler : IRequestHandler<PaginationPacienteQuery, ResponsePagination<IReadOnlyList<PacienteResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public PaginationPacienteQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponsePagination<IReadOnlyList<PacienteResponse>>> Handle(PaginationPacienteQuery request, CancellationToken cancellationToken)
    {
           var responsePagination = new ResponsePagination<IReadOnlyList<PacienteResponse>>();
        try
        {
              var pacienteSpecificationParams = new PacienteSpecificationParams
              {
                  PageIndex = request.PageIndex,
                  PageSize = request.PageSize,
                  Search = request.Search,
                  Sort = request.Sort,
                  IsActive = request.IsActive // <-- Se agrega el filtro de activos/inactivos
                
                };
            var spec = new PacienteSpecification(pacienteSpecificationParams);
            var pacientes = await _unitOfWork.GetRepository<Paciente>().GetAllWithSpec(spec);

            var specCount = new PacienteFourCountingSpecification(pacienteSpecificationParams);
            var totalPacientes = await _unitOfWork.GetRepository<Paciente>().CountAsyncWithSpec(specCount);

            var rounded = Math.Ceiling(Convert.ToDecimal(totalPacientes) / Convert.ToDecimal(pacienteSpecificationParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);
            var listPaciente = _mapper.Map<List<PacienteResponse>>(pacientes);           
            responsePagination = new ResponsePagination<IReadOnlyList<PacienteResponse>>
            {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalPacientes,
                    IsSuccess = true,
                    Message = "Lista de pacientes obtenida correctamente",
                    Data = listPaciente
            };
        }
        catch (Exception ex)
        {
            responsePagination.IsSuccess = false;
            responsePagination.Message = ex.Message + " - Error al obtener la paginación de pacientes" + ex.InnerException?.Message;
        }
        return responsePagination;
    }
 
    
}
