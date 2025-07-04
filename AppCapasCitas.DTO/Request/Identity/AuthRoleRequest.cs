using System;

namespace AppCapasCitas.DTO.Request.Identity;

public class AuthRoleRequest
{
    public string? UserId { get; set; }
    public string? RoleId { get; set; }
    public string? RoleName { get; set; }
}
