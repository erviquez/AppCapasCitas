using System;

namespace AppCapasCitas.DTO.Helpers;

public class TipoConfirmacion
{
        public static readonly Dictionary<int, string> TiposConfirmacion = new()
    {
        { 1, "Email" },
        { 2, "Celular" },
        { 3, "WhatsApp" },
        { 4, "Llamada telefónica" },
        { 5, "Mensaje de texto (SMS)" },
        { 6, "Notificación push" },
        { 7, "Correo postal" },
        { 8, "Aplicación móvil" },
        { 9, "Redes sociales" },
        { 10, "Otro" }
    };

    public static string GetNombreTipoConfirmacion(int codigo)
    {
        return TiposConfirmacion.TryGetValue(codigo, out var nombre) ? nombre : "Desconocido";
    }

}


        // // América Latina
        // { 52, "México" },
        // { 54, "Argentina" },
        // { 591, "Bolivia" },
        // { 55, "Brasil" },
        // { 56, "Chile" },
        // { 57, "Colombia" },
        // { 506, "Costa Rica" },
        // { 53, "Cuba" },
        // { 593, "Ecuador" },
        // { 503, "El Salvador" },
        // { 502, "Guatemala" },
        // { 504, "Honduras" },
        // { 505, "Nicaragua" },
        // { 507, "Panamá" },
        // { 595, "Paraguay" },
        // { 51, "Perú" },
        // { 1, "República Dominicana" },
        // { 598, "Uruguay" },
        // { 58, "Venezuela" },
        // { 1, "Puerto Rico" },
        // { 1, "Estados Unidos" }, // Por si se requiere en la lista
        // // Principales países de Europa
        // { 34, "España" },
        // { 33, "Francia" },
        // { 49, "Alemania" },
        // { 39, "Italia" },
        // { 44, "Reino Unido" },
        // { 351, "Portugal" },
        // { 41, "Suiza" },
        // { 31, "Países Bajos" },
        // { 32, "Bélgica" },
        // { 43, "Austria" },
        // { 46, "Suecia" },
        // { 47, "Noruega" },
        // { 45, "Dinamarca" },
        // { 420, "República Checa" },
        // { 48, "Polonia" },
        // { 36, "Hungría" },
        // { 40, "Rumanía" },
        // { 30, "Grecia" },
        // { 353, "Irlanda" },
        // { 358, "Finlandia" }