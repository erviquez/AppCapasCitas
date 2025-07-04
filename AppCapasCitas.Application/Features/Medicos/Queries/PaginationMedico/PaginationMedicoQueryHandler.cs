using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications.Medicos;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Queries.PaginationMedico;

public class PaginationMedicoQueryHandler: IRequestHandler<PaginationMedicoQuery, ResponsePagination<IReadOnlyList<MedicoResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;
   

    public PaginationMedicoQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
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

            //var data = _mapper.Map<IReadOnlyList<Usuario>, IReadOnlyList<MedicoVm>>(medicos);  
            var listMedico = new List<MedicoResponse>();
            foreach (var medico in medicos)
            {
                var medicoVm = new MedicoResponse
                {
                    //Id = medico.Id,
                    MedicoId = medico.UsuarioNavigation!.Id,
                    Nombre = medico.UsuarioNavigation!.Nombre,
                    Apellido = medico.UsuarioNavigation!.Apellido,
                    Telefono = medico.UsuarioNavigation!.Telefono,
                    Celular = medico.UsuarioNavigation!.Celular,
                    Direccion = medico.UsuarioNavigation!.Direccion,
                    Ciudad = medico.UsuarioNavigation!.Ciudad,
                    CodigoPais = medico.UsuarioNavigation!.CodigoPais,
                    Pais = medico.UsuarioNavigation!.Pais,
                    Estado = medico.UsuarioNavigation!.Estado,
                    Activo = medico.UsuarioNavigation!.Activo,
                    UltimoLogin = medico.UsuarioNavigation!.UltimoLogin,
                    FechaCreacion = medico.UsuarioNavigation!.FechaCreacion,
                    FechaActualizacion = medico.UsuarioNavigation!.FechaActualizacion,
                    CreadoPor = medico.UsuarioNavigation!.CreadoPor,
                    ModificadoPor = medico.UsuarioNavigation!.ModificadoPor,
                    Email = medico.UsuarioNavigation!.Email,
                    CedulaProfesional = medico.CedulaProfesional ?? string.Empty,
                    Biografia = medico.Biografia!,
                    //pendiente especialidades y hospitales

               
                };
                listMedico.Add(medicoVm);
            }
  
            responsePagination = new ResponsePagination<IReadOnlyList<MedicoResponse>>
            {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalMedicos,
                    IsSuccess = true,
                    Message = "Lista de medicos obtenida correctamente",
                    Data = listMedico
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