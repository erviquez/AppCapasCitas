using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.ActionLogUsuario;

public class ActionLogUsuarioCommandHandler : IRequestHandler<ActionLogUsuarioCommand, Response<bool>>
{
    private readonly IAsyncRepository<UsuarioLogAccion> _usuarioLogAccionRepository;
    private readonly IAsyncRepository<Usuario> _usuarioRepository;
    private readonly IAsyncRepository<TipoAccion> _tipoAccionRepository;
    private readonly IAppLogger<ActionLogUsuarioCommandHandler> _appLogger;

    public ActionLogUsuarioCommandHandler(IAsyncRepository<UsuarioLogAccion> usuarioLogAccionRepository,
                                           IAsyncRepository<TipoAccion> tipoAccionRepository,
                                           IAppLogger<ActionLogUsuarioCommandHandler> appLogger,
                                           IAsyncRepository<Usuario> usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
        _usuarioLogAccionRepository = usuarioLogAccionRepository;
        _tipoAccionRepository = tipoAccionRepository;
        _appLogger = appLogger;
    }

    public async Task<Response<bool>> Handle(ActionLogUsuarioCommand request, CancellationToken cancellationToken)
    {
        Response<bool> response = new();
        TipoAccion usuarioAccion = new ();
        try
        {
            // Verificar si el tipo de acción existe, si no, crearla
            var tipoAccionResponse = await VerificarTipoAccion(request.TipoAccion!);
            if (!tipoAccionResponse.IsSuccess)
            {
                response.IsSuccess = false;
                response.Message = tipoAccionResponse.Message;
                _appLogger.LogError($"Error al verificar el tipo de acción: {tipoAccionResponse.Message}");
                return response;
            }
            usuarioAccion = tipoAccionResponse.Data!;
            // usuarioAccion = await _tipoAccionRepository.GetEntityAsync(x => x.Nombre == request.TipoAccion);
            if (usuarioAccion == null)
            {
                response.IsSuccess = false;
                response.Message = "Tipo de acción no encontrado";
                _appLogger.LogError($"Tipo de acción no encontrado: {request.TipoAccion}");
            }
            else
            {
                var fechaCreacion = DateTime.Now;
                var usuarioLogAccion = new UsuarioLogAccion
                {
                    UsuarioId = request.UsuarioId,
                    TipoAccionId = usuarioAccion.Id,
                    FechaCreacion = fechaCreacion,
                    CreadoPor = request.UsuarioCreacion ?? "system",
                };
                await _usuarioLogAccionRepository.AddAsync(usuarioLogAccion);
                if (usuarioAccion.Nombre == "Login")
                    await UltimoLoginUsuario(request.UsuarioId, fechaCreacion);
                response.IsSuccess = true;
                response.Message = "Acción registrada correctamente";
                _appLogger.LogInformation($"Acción registrada correctamente" +
                                          $" UsuarioId: {request.UsuarioId}, TipoAccionId: {usuarioAccion.Id}");
            }
        }
        catch (Exception ex)
        {
            _appLogger.LogError($"Error al registrar la acción del usuario" +
                                 $" UsuarioId: {request.UsuarioId}, TipoAccionId: {request.TipoAccion}, Error: {ex.Message}");
            response.IsSuccess = false;
            response.Message = "Error al registrar la acción del usuario";
        }
        return response;
    }

    private async Task UltimoLoginUsuario(Guid usuarioId, DateTime fechaCreacion)
    {
        var usuario = await _usuarioRepository.GetEntityAsync(x => x.Id == usuarioId);
        if (usuario == null)
        {
            _appLogger.LogError($"Usuario no encontrado: {usuarioId}");
            return; // Retorna si el usuario no existe      
        }
        usuario.UltimoLogin = fechaCreacion; // Actualiza el último login
        var result = await _usuarioRepository.UpdateAsync(usuario);
        if (result == null)
        {
            _appLogger.LogError($"Error al actualizar el último login para el usuario: {usuarioId}");
        }
        _appLogger.LogInformation($"Último login actualizado para el usuario: {usuarioId}");
    }

    //Verificar que exista el tipo de acción en la base de datos
    private async Task<Response<TipoAccion>> VerificarTipoAccion(string tipoAccion)
    {
        var response = new Response<TipoAccion>();
        try
        {
            // Convertir a CamelCase
            if (string.IsNullOrEmpty(tipoAccion))
            {
                _appLogger.LogError("Tipo de acción no puede ser nulo o vacío");
                response.IsSuccess = false;
                response.Message = "Tipo de acción no puede ser nulo o vacío";
                return response;
            }
            tipoAccion = ToPascalCase(tipoAccion);
            var accion = await _tipoAccionRepository.GetEntityAsync(x => x.Nombre == tipoAccion);
            //usuarioAccion = await _tipoAccionRepository.GetEntityAsync(x => x.Nombre == request.TipoAccion);

            //Crear si no existe
            if (accion == null)
            {
                accion = new TipoAccion
                {
                    Id = Guid.NewGuid(),
                    Nombre = tipoAccion,
                    Descripcion = "Acción de usuario: " + tipoAccion
                };
                await _tipoAccionRepository.AddAsync(accion);
                response.IsSuccess = true;
                response.Data = accion;
                response.Message = "Tipo de acción creado correctamente";
                return response;
            }
            response.IsSuccess = true;
            response.Data = accion;
            response.Message = "Tipo de acción verificado correctamente";
            return response;
        }
        catch (Exception ex)
        {
            _appLogger.LogError($"Error al verificar el tipo de acción: {ex.Message}");
            response.IsSuccess = false;
            response.Message = "Error al verificar el tipo de acción";
            response.Errors = new List<ValidationFailure> { new ValidationFailure("Error", ex.Message) };
            return response;
        }

    }
    public static string ToCamelCaseFull(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        var words = input.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 1; i < words.Length; i++)
            words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1).ToLowerInvariant();

        var result = string.Concat(words);
        return char.ToLowerInvariant(result[0]) + result.Substring(1);
    }
    public static string ToPascalCase(string input)
{
    if (string.IsNullOrEmpty(input))
        return input;

    var words = input.Split(new[] { ' ', '-', '_' }, StringSplitOptions.RemoveEmptyEntries);
    for (int i = 0; i < words.Length; i++)
        words[i] = char.ToUpperInvariant(words[i][0]) + words[i].Substring(1).ToLowerInvariant();

    return string.Concat(words);
}

}
