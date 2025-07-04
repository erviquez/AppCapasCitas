using System;
using AppCapasCitas.Application.Contracts.Identity;
using AppCapasCitas.Transversal.Common;
using AppCapasCitas.Transversal.Common.Identity;
using MediatR;

namespace AppCapasCitas.Application.Features.Identity.Queries.GetRoles;

public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, Response<List<Role>>>
{
    private readonly IAuthService _authService;
    private readonly IAppLogger<GetRolesQueryHandler> _appLogger;

    public GetRolesQueryHandler(IAuthService authService, IAppLogger<GetRolesQueryHandler> appLogger)
    {
        _authService = authService;
        _appLogger = appLogger;
    }

    public async Task<Response<List<Role>>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
    {
        var response = new Response<List<Role>>();
        var result = await _authService.GetRoles();
        if (!result.IsSuccess)
        {
            _appLogger.LogError($"Error al obtener los roles: {result.Message}");
            response.IsSuccess = false;
            response.Message = "Error al obtener los roles";
            response.Errors = result.Errors;
            return response;
        }
        response.Data = result.Data;
        response.IsSuccess = true;
        response.Message = "Roles obtenidos correctamente";
        _appLogger.LogInformation("Roles obtenidos correctamente");
        return response;
        

    }
}
