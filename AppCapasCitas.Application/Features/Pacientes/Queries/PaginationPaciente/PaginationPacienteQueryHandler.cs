using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications.Pacientes;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Pacientes.Queries.PaginationPaciente;

public class PaginationPacienteQueryHandler : IRequestHandler<PaginationPacienteQuery, ResponsePagination<IReadOnlyList<PacienteResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PaginationPacienteQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

            //var data = _mapper.Map<IReadOnlyList<Usuario>, IReadOnlyList<PacienteVm>>(pacientes);  
            var listPaciente = new List<PacienteResponse>();
            foreach (var paciente in pacientes)
            {
                var pacienteVm = new PacienteResponse
                {
                    //Id = paciente.Id,
                    PacienteId = paciente.UsuarioNavigation!.Id,
                    Nombre = paciente.UsuarioNavigation!.Nombre,
                    Apellido = paciente.UsuarioNavigation!.Apellido,
                    Telefono = paciente.UsuarioNavigation!.Telefono,
                    Celular = paciente.UsuarioNavigation!.Celular,
                    Direccion = paciente.UsuarioNavigation!.Direccion,
                    Ciudad = paciente.UsuarioNavigation!.Ciudad,
                    CodigoPais = paciente.UsuarioNavigation!.CodigoPais,
                    Pais = paciente.UsuarioNavigation!.Pais,
                    Estado = paciente.UsuarioNavigation!.Estado,
                    Activo = paciente.UsuarioNavigation!.Activo,
                    UltimoLogin = paciente.UsuarioNavigation!.UltimoLogin,
                    FechaCreacion = paciente.UsuarioNavigation!.FechaCreacion,
                    FechaActualizacion = paciente.UsuarioNavigation!.FechaActualizacion,
                    CreadoPor = paciente.UsuarioNavigation!.CreadoPor,
                    ModificadoPor = paciente.UsuarioNavigation!.ModificadoPor,
                    Email = paciente.UsuarioNavigation!.Email,
                    FechaNacimiento = paciente.FechaNacimiento,
                    Genero = paciente.Genero,
                    Alergias = paciente.Alergias,
                    EnfermedadesCronicas = paciente.EnfermedadesCronicas,
                    MedicamentosActuales = paciente.MedicamentosActuales

                    //pendiente especialidades y hospitales


                };
                listPaciente.Add(pacienteVm);
            }
  
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
