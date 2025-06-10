
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Helpers;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;

public class GetUsuarioListQueryHandler : IRequestHandler<GetUsuarioListQuery, Response<IReadOnlyList<UsuarioResponse>>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _usuarioReposistory;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetUsuarioListQueryHandler> _appLogger;

    public GetUsuarioListQueryHandler(IAuthService authService, IAsyncRepository<Usuario> usuarioReposistory, IMapper mapper, IAppLogger<GetUsuarioListQueryHandler> appLogger)
    {
        _authService = authService;
        _usuarioReposistory = usuarioReposistory;
        _mapper = mapper;
        _appLogger = appLogger;
    }
    public async Task<Response<IReadOnlyList<UsuarioResponse>>> Handle(GetUsuarioListQuery request, CancellationToken cancellationToken)
    {
        Response<IReadOnlyList<UsuarioResponse>> response = new();
        var usuarioIdentityList = await _authService.GetAllApplicationUser();
        if (!usuarioIdentityList.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = "No se encontraron registros identity";
            return response;
        }

        var usuariosAppList = await _usuarioReposistory.GetAllAsync();
        var usuariosResponseList = new List<UsuarioResponse>();
        if (usuariosAppList.Count > 0)
        {
            foreach (var usuarioIdentity in usuarioIdentityList.Data!)
            {
                var usuarioResponse = new UsuarioResponse
                {
                    IdentityId = usuarioIdentity.Id != null ? Guid.Parse(usuarioIdentity.Id) : (Guid?)null,
                    Email = usuarioIdentity.Email,
                    Activo = usuarioIdentity.Active
                };
                foreach (var usuarioApp in usuariosAppList)
                {
                    if (usuarioApp.IdentityId == usuarioResponse.IdentityId)
                    {
                        usuarioResponse.Nombre = usuarioApp.Nombre;
                        usuarioResponse.Apellido = usuarioApp.Apellido;
                        usuarioResponse.Telefono = usuarioApp.Telefono;
                        usuarioResponse.Celular = usuarioApp.Celular;
                        usuarioResponse.Direccion = usuarioApp.Direccion;
                        usuarioResponse.Ciudad = usuarioApp.Ciudad;
                        usuarioResponse.Estado = usuarioApp.Estado;
                        usuarioResponse.CodigoPais = usuarioApp.CodigoPais;
                        usuarioResponse.Pais = PaisesHelper.GetNombrePais(usuarioApp.CodigoPais);
                        usuarioResponse.Activo = usuarioApp.Activo;
                        if (usuarioApp.MedicoId.HasValue)
                        {
                            usuarioResponse.MedicoId = usuarioApp.MedicoId.Value;
                        }
                        if (usuarioApp.PacienteId.HasValue)
                        {
                            usuarioResponse.PacienteId = usuarioApp.PacienteId.Value;
                        }
                    }
                }
                usuariosResponseList.Add(usuarioResponse);
            }
            response.Data = usuariosResponseList;
            response.IsSuccess = true;
            response.Message = "Registros obtenidos correctamente";
        }
        else
        {
            _appLogger.LogWarning("No se encontraron registros de usuarios en la base de datos");
            response.IsSuccess = false;
            response.Message = "No se encontraron registros de usuarios";
        }
        return response;
    }
}
        
