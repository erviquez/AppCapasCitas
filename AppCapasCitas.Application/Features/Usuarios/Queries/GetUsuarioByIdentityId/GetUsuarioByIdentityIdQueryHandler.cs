using System;
using System.Linq.Expressions;
using AppCapasCitas.Application.Contracts.Persistence.Identity;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Usuario;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Queries.GetUsuarioByIdentityId;

public class GetUsuarioByIdentityIdQueryHandler : IRequestHandler<GetUsuarioByIdentityIdQuery, Response<UsuarioResponse>>
{
    private readonly IAsyncRepository<Usuario> _usuarioRepository;
    private readonly IAppLogger<GetUsuarioByIdentityIdQueryHandler> _appLogger;
    private readonly IAuthService _authService;

    public GetUsuarioByIdentityIdQueryHandler(IAsyncRepository<Usuario> usuarioRepository, IAppLogger<GetUsuarioByIdentityIdQueryHandler> appLogger, IAuthService authService)
    {
        _usuarioRepository = usuarioRepository;
        _appLogger = appLogger;
        _authService = authService;
    }

    public async Task<Response<UsuarioResponse>> Handle(GetUsuarioByIdentityIdQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<UsuarioResponse>();

        // Consultar usuario en Identity
        var usuarioIdentity = await _authService.GetApplicationUser(request.UsuarioId.ToString());
        if (!usuarioIdentity.IsSuccess)
        {
            response.IsSuccess = false;
            response.Message = "No se encontraron registros identity";
            return response;
        }

        // Consultar usuario en la base de datos de la aplicación
        //Expression function
        var includes = new List<Expression<Func<Usuario, object>>>
        {
            x => x.MedicoNavigation!,
            x => x.PacienteNavigation!
        };
        var usuario = await _usuarioRepository.GetEntityAsync(x => x.Id == request.UsuarioId,
                                                              includes,
                                                              true,
                                                              cancellationToken: cancellationToken);

        if (usuario is not null)
        {
            response.Data = MapearUsuarioResponse(usuario);
            response.Message = "Usuario encontrado exitosamente";
            response.IsSuccess = true;
        }
        else
        {
            _appLogger.LogWarning($"No se encontró el usuario con ID {request.UsuarioId} en la base de datos de la aplicación");
            response.IsSuccess = false;
            response.Message = "No se encontró el usuario en la base de datos";
        }

        return response;
    }

    // Método auxiliar para mapear la entidad Usuario a UsuarioResponse
    private UsuarioResponse MapearUsuarioResponse(Usuario usuario)
    {
        return new UsuarioResponse
        {
            UsuarioId = usuario.Id,
            RoleId = usuario.RoleId,
            RoleName = usuario.RolName!,
            Nombre = usuario.Nombre!,
            Apellido = usuario.Apellido!,
            Email = usuario.Email!,
            Telefono = usuario.Telefono!,
            Celular = usuario.Celular!,
            Direccion = usuario.Direccion!,
            Estado = usuario.Estado!,
            Ciudad = usuario.Ciudad!,
            CodigoPais = usuario.CodigoPais!,
            Pais = usuario.Pais!,
            Activo = usuario.Activo,
            FechaCreacion = usuario.FechaCreacion,
            FechaActualizacion = usuario.FechaActualizacion,
            UltimoLogin = usuario.UltimoLogin,
            CreadoPor = usuario.CreadoPor,
            ModificadoPor = usuario.ModificadoPor,
  
        };
    }
}
