using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Helpers;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using AutoMapper;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioList;

/// <summary>
/// Handler para obtener la lista de usuarios combinando información de Identity y la base de datos de la aplicación.
/// </summary>
public class GetUsuarioListQueryHandler : IRequestHandler<GetUsuarioListQuery, Response<IReadOnlyList<UsuarioResponse>>>
{
    private readonly IAuthService _authService;
    private readonly IAsyncRepository<Usuario> _usuarioReposistory;
    private readonly IMapper _mapper;
    private readonly IAppLogger<GetUsuarioListQueryHandler> _appLogger;

    /// <summary>
    /// Constructor con inyección de dependencias.
    /// </summary>
    public GetUsuarioListQueryHandler(
        IAuthService authService,
        IAsyncRepository<Usuario> usuarioReposistory,
        IMapper mapper,
        IAppLogger<GetUsuarioListQueryHandler> appLogger)
    {
        _authService = authService;
        _usuarioReposistory = usuarioReposistory;
        _mapper = mapper;
        _appLogger = appLogger;
    }

    /// <summary>
    /// Maneja la consulta para obtener la lista de usuarios.
    /// </summary>
    public async Task<Response<IReadOnlyList<UsuarioResponse>>> Handle(GetUsuarioListQuery request, CancellationToken cancellationToken)
    {
        Response<IReadOnlyList<UsuarioResponse>> response = new();

        // Obtener la lista de usuarios desde Identity
        var usuarioIdentityList = await _authService.GetAllApplicationUser();
        if (!usuarioIdentityList.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = "No se encontraron registros identity";
            return response;
        }

        // Obtener la lista de usuarios desde la base de datos de la aplicación
        var usuariosAppList = await _usuarioReposistory.GetAllAsync();
        var usuariosResponseList = new List<UsuarioResponse>();

        // Si existen usuarios en la base de datos de la aplicación
        if (usuariosAppList.Count > 0)
        {
            // Crear un diccionario para acceso rápido por Id
            var usuariosAppDict = usuariosAppList.ToDictionary(u => u.Id, u => u);

            foreach (var usuarioIdentity in usuarioIdentityList.Data!)
            {
                var usuarioResponse = new UsuarioResponse
                {
                    UsuarioId = usuarioIdentity.Id,
                    Email = usuarioIdentity.Email,
                    Activo = usuarioIdentity.Active
                };
                // Si existe el usuario en la base de datos, combinar datos
                if (usuariosAppDict.TryGetValue(usuarioIdentity.Id, out var usuarioApp))
                {
                    MapearDatosUsuarioApp(usuarioResponse, usuarioApp);
                }
                usuariosResponseList.Add(usuarioResponse);
            }

            response.Data = usuariosResponseList;
            response.IsSuccess = true;
            response.Message = "Registros obtenidos correctamente";
        }
        else
        {
            // Si no hay usuarios en la base de datos, registrar advertencia y devolver mensaje
            _appLogger.LogWarning("No se encontraron registros de usuarios en la base de datos");
            response.IsSuccess = false;
            response.Message = "No se encontraron registros de usuarios";
        }
        return response;
    }
    // Método auxiliar para mapear datos adicionales
    private void MapearDatosUsuarioApp(UsuarioResponse usuarioResponse, Usuario usuarioApp)
    {
        usuarioResponse.UsuarioId = usuarioApp.Id;
        usuarioResponse.RoleId = usuarioApp.RoleId;
        usuarioResponse.RoleName = usuarioApp.RolName;
        usuarioResponse.Email = usuarioApp.Email;
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
        usuarioResponse.UltimoLogin = usuarioApp.UltimoLogin;
        usuarioResponse.FechaCreacion = usuarioApp.FechaCreacion;
        usuarioResponse.CreadoPor = usuarioApp.CreadoPor;
        usuarioResponse.FechaActualizacion = usuarioApp.FechaActualizacion;
        usuarioResponse.ModificadoPor = usuarioApp.ModificadoPor;
    }
}