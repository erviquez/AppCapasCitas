
using AppCapasCitas.Domain.Models.Common;

namespace AppCapasCitas.Domain.Models;

public partial class Usuario : EntidadBaseAuditoria
    {
        public Guid RoleId { get; set; }  
        public string RolName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string Apellido{ get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;    
        public string Ciudad { get; set; } = string.Empty;
        public Int32 CodigoPais { get; set; }
        public string Pais { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;  
        public string? Genero { get; set; }
        ///
    public DateTime? UltimoLogin { get; set; }

        // Relaciones
        public virtual Paciente? PacienteNavigation { get; set; }

        public virtual Medico? MedicoNavigation { get; set; }
    }