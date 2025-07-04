namespace AppCapasCitas.DTO.Request.Paciente;

public class PacienteResponse
{
        
        public Guid PacienteId { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Celular { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string? CodigoPostal { get; set; }
        public string Email { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public int CodigoPais { get; set; }
        public string Pais { get; set; } = null!;
        public string Estado { get; set; } = null!;
        public bool Activo { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public string? CreadoPor { get; set; }
        public string? ModificadoPor { get; set; }
        //datos adicionales del paciente
        public DateTime? FechaNacimiento { get; set; }
        public string? Genero { get; set; }
        public string? Alergias { get; set; }
        public string? EnfermedadesCronicas { get; set; }
        public string? MedicamentosActuales { get; set; }
        public string? AntecedentesFamiliares { get; set; }
        public string? AntecedentesPersonales { get; set; }
        public string? Observaciones { get; set; }
        public string? Sexo { get; set; } // Nuevo campo para el sexo del paciente
        public string? TipoSangre { get; set; }
        public string? NumeroIdentificacion { get; set; }
        public string? EstadoCivil { get; set; }
        public string? Ocupacion { get; set; }
        public string? Nacionalidad { get; set; }
        public string? Idiomas { get; set; }
        public string? ContactoEmergenciaNombre { get; set; }
        public string? ContactoEmergenciaTelefono { get; set; }
        public string? ContactoEmergenciaParentesco { get; set; }
        public string? SeguroMedico { get; set; }
        public string? NumeroSeguroMedico { get; set; }
        public string? NombreAseguradora { get; set; }
        public string? TelefonoAseguradora { get; set; }
        public string? DireccionAseguradora { get; set; }
        public string? CiudadAseguradora { get; set; }
        public string? CodigoPostalAseguradora { get; set; }
        public string? EstadoAseguradora { get; set; }
        public string? PaisAseguradora { get; set; }
        public string? ObservacionesAdicionales { get; set; }
}
