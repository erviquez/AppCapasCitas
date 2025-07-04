

using Microsoft.AspNetCore.Identity;

namespace AppCapasCitas.Identity.Models
{
    public class ApplicationRole:IdentityRole
    {
        public int Prioridad { get; set; }
        public int Orden { get; set; }
        public bool Activo { get; set; }
    }
}