

namespace AppCapasCitas.Transversal.Common.Identity;


public class Role
{
    public Guid Id { get; set; }
    public string? Name { get; set; } 
    public int Prioridad { get; set; }
    public int Orden { get; set; }
    public bool Activo { get; set; }

}
