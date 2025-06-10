using System;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Application.Specifications.Usuarios;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.PaginationUsuario;

public class PaginationUsuarioQueryHandler : IRequestHandler<PaginationUsuarioQuery, ResponsePagination<IReadOnlyList<UsuarioResponse>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public PaginationUsuarioQueryHandler(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponsePagination<IReadOnlyList<UsuarioResponse>>> Handle(PaginationUsuarioQuery request, CancellationToken cancellationToken)
    {
        var responsePagination = new ResponsePagination<IReadOnlyList<UsuarioResponse>>();
        try
        {
            var usuarioSpecificationParams = new UsuarioSpecificationParams
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Search = request.Search,
                Sort = request.Sort,
                IsActive = request.IsActive // <-- Se agrega el filtro de activos/inactivos
            };
            var spec = new UsuarioSpecification(usuarioSpecificationParams);
            var usuarios = await _unitOfWork.GetRepository<Usuario>().GetAllWithSpec(spec);
            var specCount = new UsuarioForCountingSpecification(usuarioSpecificationParams);
            var totalUsuarios = await _unitOfWork.GetRepository<Usuario>().CountAsyncWithSpec(specCount);
            var rounded = Math.Ceiling(Convert.ToDecimal(totalUsuarios) / Convert.ToDecimal(usuarioSpecificationParams.PageSize));
            var totalPages = Convert.ToInt32(rounded);

            var listUsuario = new List<UsuarioResponse>();
            if (usuarios != null && usuarios.Any())
            {
                foreach (var usuario in usuarios)
                {
                    var usuarioVm = new UsuarioResponse
                    {
                        IdentityId = usuario!.IdentityId,
                        Nombre = usuario!.Nombre,
                        Apellido = usuario!.Apellido,
                        Email = usuario!.Email,
                        Telefono = usuario!.Telefono,
                        Celular = usuario!.Celular,
                        Direccion = usuario!.Direccion,
                        Ciudad = usuario!.Ciudad,
                        CodigoPais = usuario!.CodigoPais,
                        Pais = usuario!.Pais,
                        Activo = usuario!.Activo,
                        UltimoLogin = usuario.UltimoLogin
                    };
                    listUsuario.Add(usuarioVm);
                }
                responsePagination = new ResponsePagination<IReadOnlyList<UsuarioResponse>>
                {
                    PageNumber = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalUsuarios,
                    IsSuccess = true,
                    Message = "Lista de usuarios obtenida correctamente",
                    Data = listUsuario
                };                
            }else
            {
                responsePagination = new ResponsePagination<IReadOnlyList<UsuarioResponse>>
                {
                    IsSuccess = false,
                    Message = "No se encontraron usuarios con los parámetros de búsqueda proporcionados"
                };
            }
        }
        catch (Exception ex)
        {
            responsePagination.IsSuccess = false;
            responsePagination.Message = ex.InnerException!.Message;
        }
        return responsePagination;
    }
}
