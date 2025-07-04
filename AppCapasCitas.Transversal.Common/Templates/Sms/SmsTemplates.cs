namespace AppCapasCitas.Transversal.Common.Templates.Sms;

public class SmsTemplates
{
    public static string GetTemplateCambioRol(string nombreCompleto, string rolAnterior, string rolNuevo)
    {
        return $"Hola {nombreCompleto}, tu rol en Sistema de citas ha cambiado de '{rolAnterior}' a '{rolNuevo}'. Si no reconoces este cambio, contacta al administrador.";
    }
    public static string GetTemplateConfirmacionTelefono( string confirmUrl)
    {
        return $@"Citas-Clic en enlace para confirmar: {confirmUrl}

    ";
    }
}