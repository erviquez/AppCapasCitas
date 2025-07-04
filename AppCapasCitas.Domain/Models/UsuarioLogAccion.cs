using System;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public class UsuarioLogAccion : EntidadBaseAuditoria
{
    public Guid UsuarioId { get; set; }
    public Guid TipoAccionId { get; set; } // Ejemplo: "Login", "Logout", "UpdateProfile", etc.
    // Relaciones
    public virtual Usuario UsuarioNavigation { get; set; } = null!;
    public virtual TipoAccion TipoAccionNavigation { get; set; } = null!;
}
