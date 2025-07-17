using System;
using AppCapasCitas.DTO.Request.Identity;
using AppCapasCitas.DTO.Response.Identity;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.FrontEnd.Proxy.Interfaces;

public interface ILoginProxy
{
     Task<Response<AuthResponse>> LoginAsync(AuthRequest request);
     Task<Response<bool>> ResetPasswordAsync(AuthResetPasswordRequest request);
}
