using System;
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public class TipoAccion : EntidadBase
{   
    //public Guid TipoAccionId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
