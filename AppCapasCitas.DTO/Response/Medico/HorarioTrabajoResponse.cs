using AppCapasCitas.DTO.Helpers;
namespace AppCapasCitas.DTO.Response.Medico;
public class HorarioTrabajoResponse
{
    public Guid HorarioTrabajoId { get; set; }
    public DayOfWeek DiaSemana { get; set; }
    public string DiaSemanaNombre => DiasSemanaHelper.GetNombreDiaSemana(DiaSemana);
    public TimeSpan HoraInicio { get; set; }
    public TimeSpan HoraFin { get; set; }
    public Guid? MedicoId { get; set; }
}
