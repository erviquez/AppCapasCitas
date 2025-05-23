using System;
using AppCapasCitas.Application.Helpers;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Usuarios.Commands.CreateUsuario;

 public class CreateUsuarioCommand : IRequest<Response<RegistrationResponse>>
    {
        public string Username { get; init; } = string.Empty;
        public string Password { get; init; } = string.Empty;
        public string Email { get; init; } = string.Empty;
        public string Nombre { get; init; } = string.Empty;
        public string Apellido { get; init; } = string.Empty;
        public string Telefono { get; init; } = string.Empty;
        public string Celular { get; init; } = string.Empty;
        public string Direccion { get; init; } = string.Empty;
        public string Ciudad { get; init; } = string.Empty;
        public string Estado { get; init; } = string.Empty;
        public int CodigoPais { get; init; }
        public string Pais => PaisesHelper.GetNombrePais(CodigoPais);
        public string RoleId { get; init; } = string.Empty;
    }