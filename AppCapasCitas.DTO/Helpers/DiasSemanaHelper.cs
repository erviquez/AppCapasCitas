using System;

namespace AppCapasCitas.DTO.Helpers;

public class DiasSemanaHelper
{
    public static string GetNombreDiaSemana(DayOfWeek diaSemana)
    {
        return diaSemana switch
        {
            DayOfWeek.Monday => "Lunes",
            DayOfWeek.Tuesday => "Martes",
            DayOfWeek.Wednesday => "Miércoles",
            DayOfWeek.Thursday => "Jueves",
            DayOfWeek.Friday => "Viernes",
            DayOfWeek.Saturday => "Sábado",
            DayOfWeek.Sunday => "Domingo",
            _ => throw new ArgumentOutOfRangeException(nameof(diaSemana), diaSemana, null)
        };
    }

}
