using System;

namespace AppCapasCitas.DTO.Helpers;

public class EstadosCitaHelper
{
    public static string ObtenerEstadoCita(int estado)
    {
        return estado switch
        {
            1 => "Pendiente",
            2 => "Confirmada",
            3 => "Cancelada",
            4 => "Atendida",
            _ => "Desconocido"
        };
    }
}
