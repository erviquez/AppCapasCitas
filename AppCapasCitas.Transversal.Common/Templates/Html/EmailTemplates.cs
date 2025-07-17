
namespace AppCapasCitas.Transversal.Common.Templates.Html;

public class EmailTemplates
{
    public static string GetTemplateCambioRol(string nombreCompleto, string email, string rolAnterior, string rolNuevo)
    {
        return $@"
        <!DOCTYPE html>
        <html lang=""es"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Cambio de Rol de Usuario - App Citas</title>
            <style>
                body {{
                    font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
                    line-height: 1.6;
                    color: #333;
                    background-color: #f5f7fa;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background: #ffffff;
                    border-radius: 12px;
                    overflow: hidden;
                    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                }}
                .email-header {{
                    background: linear-gradient(135deg, #4a6fa5, #3a5a8a);
                    color: white;
                    padding: 28px 24px;
                    text-align: center;
                }}
                .email-header h1 {{
                    margin: 0;
                    font-size: 24px;
                    font-weight: 600;
                }}
                .email-body {{
                    padding: 32px 24px;
                }}
                .greeting {{
                    font-size: 18px;
                    margin-bottom: 24px;
                    color: #2d3748;
                }}
                .message {{
                    margin-bottom: 24px;
                    color: #4a5568;
                }}
                .info-card {{
                    background: #f8fafc;
                    border-radius: 8px;
                    padding: 20px;
                    margin: 24px 0;
                    border-left: 4px solid #4a6fa5;
                }}
                .info-row {{
                    display: flex;
                    margin-bottom: 12px;
                }}
                .info-label {{
                    font-weight: 600;
                    color: #4a6fa5;
                    min-width: 120px;
                }}
                .info-value {{
                    flex: 1;
                }}
                .highlight {{
                    color: #2b6cb0;
                    font-weight: 600;
                }}
                .notice {{
                    background: #fffaf0;
                    padding: 16px;
                    border-radius: 6px;
                    border-left: 4px solid #dd6b20;
                    margin: 24px 0;
                    font-size: 15px;
                }}
                .email-footer {{
                    text-align: center;
                    padding: 20px;
                    color: #718096;
                    font-size: 14px;
                    background: #f5f7fa;
                    border-top: 1px solid #e2e8f0;
                }}
                @media only screen and (max-width: 600px) {{
                    .email-container {{
                        margin: 0;
                        border-radius: 0;
                    }}
                    .info-row {{
                        flex-direction: column;
                    }}
                    .info-label {{
                        margin-bottom: 4px;
                    }}
                }}
            </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""email-header"">
                    <h1>Actualización de Rol de Usuario</h1>
                </div>
                
                <div class=""email-body"">
                    <div class=""greeting"">
                        Hola <strong>{nombreCompleto}</strong>,
                    </div>
                    
                    <div class=""message"">
                        Tu rol de usuario en <strong>Sistema Médico</strong> ha sido actualizado. A continuación encontrarás los detalles del cambio:
                    </div>
                    
                    <div class=""info-card"">
                        <div class=""info-row"">
                            <div class=""info-label"">Correo electrónico:</div>
                            <div class=""info-value"">{email}</div>
                        </div>
                        <div class=""info-row"">
                            <div class=""info-label"">Rol anterior:</div>
                            <div class=""info-value"">{rolAnterior}</div>
                        </div>
                        <div class=""info-row"">
                            <div class=""info-label"">Nuevo rol:</div>
                            <div class=""info-value highlight"">{rolNuevo}</div>
                        </div>
                    </div>
                    
                    <div class=""notice"">
                        <strong>Importante:</strong> Si no reconoces este cambio o crees que se ha realizado por error, por favor contacta inmediatamente al administrador del sistema.
                    </div>
                    
                    <div class=""message"">
                        Gracias por utilizar nuestros servicios.<br>
                        El equipo de App Citas
                    </div>
                </div>
                
                <div class=""email-footer"">
                    &copy; {DateTime.Now.Year} App Citas. Todos los derechos reservados.<br>
                    <small>Este es un mensaje automático, por favor no respondas directamente.</small>
                </div>
            </div>
        </body>
        </html>
        ";
    }


    public static string GetTemplateAltaUsuario(string nombreCompleto, string email, string usuario, string rol)
    {
        return $@"
        <!DOCTYPE html>
        <html lang=""es"">
        <head>
            <meta charset=""UTF-8"">
            <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
            <title>Alta de Usuario - App Citas</title>
            <style>
                body {{
                    font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
                    line-height: 1.6;
                    color: #333;
                    background-color: #f5f7fa;
                    margin: 0;
                    padding: 0;
                }}
                .email-container {{
                    max-width: 600px;
                    margin: 20px auto;
                    background: #ffffff;
                    border-radius: 12px;
                    overflow: hidden;
                    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
                }}
                .email-header {{
                    background: linear-gradient(135deg, #4a6fa5, #3a5a8a);
                    color: white;
                    padding: 28px 24px;
                    text-align: center;
                }}
                .email-header h1 {{
                    margin: 0;
                    font-size: 24px;
                    font-weight: 600;
                }}
                .email-body {{
                    padding: 32px 24px;
                }}
                .greeting {{
                    font-size: 18px;
                    margin-bottom: 24px;
                    color: #2d3748;
                }}
                .message {{
                    margin-bottom: 24px;
                    color: #4a5568;
                }}
                .info-card {{
                    background: #f8fafc;
                    border-radius: 8px;
                    padding: 20px;
                    margin: 24px 0;
                    border-left: 4px solid #4a6fa5;
                }}
                .info-row {{
                    display: flex;
                    margin-bottom: 12px;
                }}
                .info-label {{
                    font-weight: 600;
                    color: #4a6fa5;
                    min-width: 120px;
                }}
                .info-value {{
                    flex: 1;
                }}
                .highlight {{
                    color: #2b6cb0;
                    font-weight: 600;
                }}
                .notice {{
                    background: #fffaf0;
                    padding: 16px;
                    border-radius: 6px;
                    border-left: 4px solid #38a169;
                    margin: 24px 0;
                    font-size: 15px;
                }}
                .email-footer {{
                    text-align: center;
                    padding: 20px;
                    color: #718096;
                    font-size: 14px;
                    background: #f5f7fa;
                    border-top: 1px solid #e2e8f0;
                }}
                @media only screen and (max-width: 600px) {{
                    .email-container {{
                        margin: 0;
                        border-radius: 0;
                    }}
                    .info-row {{
                        flex-direction: column;
                    }}
                    .info-label {{
                        margin-bottom: 4px;
                    }}
                }}
            </style>
        </head>
        <body>
            <div class=""email-container"">
                <div class=""email-header"">
                    <h1>Bienvenido a App Citas</h1>
                </div>
                
                <div class=""email-body"">
                    <div class=""greeting"">
                        Hola <strong>{nombreCompleto}</strong>,
                    </div>
                    
                    <div class=""message"">
                        Tu usuario ha sido creado exitosamente en <strong>App Citas</strong>. A continuación encontrarás tus datos de acceso:
                    </div>
                    
                    <div class=""info-card"">
                        <div class=""info-row"">
                            <div class=""info-label"">Correo electrónico:</div>
                            <div class=""info-value"">{email}</div>
                        </div>
                        <div class=""info-row"">
                            <div class=""info-label"">Usuario:</div>
                            <div class=""info-value"">{usuario}</div>
                        </div>
                        <div class=""info-row"">
                            <div class=""info-label"">Rol asignado:</div>
                            <div class=""info-value highlight"">{rol}</div>
                        </div>
                    </div>
                    
                    <div class=""notice"">
                        <strong>Importante:</strong> Por seguridad, te recomendamos cambiar tu contraseña en tu primer inicio de sesión.
                    </div>
                    
                    <div class=""message"">
                        Gracias por utilizar nuestros servicios.<br>
                        El equipo de App Citas
                    </div>
                </div>
                
                <div class=""email-footer"">
                    &copy; {DateTime.Now.Year} App Citas. Todos los derechos reservados.<br>
                    <small>Este es un mensaje automático, por favor no respondas directamente.</small>
                </div>
            </div>
        </body>
        </html>
        ";
    }

    public static string GetTemplateAltaUsuarioConfirmEmail(string nombreCompleto, string email, string usuario, string rol, string confirmUrl)
{
    return $@"
    <!DOCTYPE html>
    <html lang=""es"">
    <head>
        <meta charset=""UTF-8"">
        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
        <title>Alta de Usuario - App Citas</title>
        <style>
            body {{
                font-family: 'Segoe UI', 'Helvetica Neue', Arial, sans-serif;
                line-height: 1.6;
                color: #333;
                background-color: #f5f7fa;
                margin: 0;
                padding: 0;
            }}
            .email-container {{
                max-width: 600px;
                margin: 20px auto;
                background: #ffffff;
                border-radius: 12px;
                overflow: hidden;
                box-shadow: 0 4px 12px rgba(0, 0, 0, 0.1);
            }}
            .email-header {{
                background: linear-gradient(135deg, #4a6fa5, #3a5a8a);
                color: white;
                padding: 28px 24px;
                text-align: center;
            }}
            .email-header h1 {{
                margin: 0;
                font-size: 24px;
                font-weight: 600;
            }}
            .email-body {{
                padding: 32px 24px;
            }}
            .greeting {{
                font-size: 18px;
                margin-bottom: 24px;
                color: #2d3748;
            }}
            .message {{
                margin-bottom: 24px;
                color: #4a5568;
            }}
            .info-card {{
                background: #f8fafc;
                border-radius: 8px;
                padding: 20px;
                margin: 24px 0;
                border-left: 4px solid #4a6fa5;
            }}
            .info-row {{
                display: flex;
                margin-bottom: 12px;
            }}
            .info-label {{
                font-weight: 600;
                color: #4a6fa5;
                min-width: 120px;
            }}
            .info-value {{
                flex: 1;
            }}
            .highlight {{
                color: #2b6cb0;
                font-weight: 600;
            }}
            .notice {{
                background: #fffaf0;
                padding: 16px;
                border-radius: 6px;
                border-left: 4px solid #38a169;
                margin: 24px 0;
                font-size: 15px;
            }}
            .email-footer {{
                text-align: center;
                padding: 20px;
                color: #718096;
                font-size: 14px;
                background: #f5f7fa;
                border-top: 1px solid #e2e8f0;
            }}
            .confirm-btn {{
                display: inline-block;
                margin-top: 20px;
                padding: 12px 28px;
                background: #4a6fa5;
                color: #fff;
                text-decoration: none;
                border-radius: 6px;
                font-weight: 600;
                font-size: 16px;
            }}
            @media only screen and (max-width: 600px) {{
                .email-container {{
                    margin: 0;
                    border-radius: 0;
                }}
                .info-row {{
                    flex-direction: column;
                }}
                .info-label {{
                    margin-bottom: 4px;
                }}
            }}
        </style>
    </head>
    <body>
        <div class=""email-container"">
            <div class=""email-header"">
                <h1>Bienvenido a App Citas</h1>
            </div>
            
            <div class=""email-body"">
                <div class=""greeting"">
                    Hola <strong>{nombreCompleto}</strong>,
                </div>
                
                <div class=""message"">
                    Tu usuario ha sido creado exitosamente en <strong>App Citas</strong>. A continuación encontrarás tus datos de acceso:
                </div>
                
                <div class=""info-card"">
                    <div class=""info-row"">
                        <div class=""info-label"">Correo electrónico:</div>
                        <div class=""info-value"">{email}</div>
                    </div>
                    <div class=""info-row"">
                        <div class=""info-label"">Usuario:</div>
                        <div class=""info-value"">{usuario}</div>
                    </div>
                    <div class=""info-row"">
                        <div class=""info-label"">Rol asignado:</div>
                        <div class=""info-value highlight"">{rol}</div>
                    </div>
                </div>
                
                <div class=""notice"">
                    <strong>Importante:</strong> Por seguridad, te recomendamos cambiar tu contraseña en tu primer inicio de sesión.
                </div>

                <div style=""text-align:center;"">
                    <a href=""{confirmUrl}"" class=""confirm-btn"" style=""color: #FFFFFF;"">Confirmar mi correo electrónico</a>
                </div>
                
                <div class=""message"">
                    Gracias por utilizar nuestros servicios.<br>
                    El equipo de App Citas
                </div>
            </div>
            
            <div class=""email-footer"">
                &copy; {DateTime.Now.Year} App Citas. Todos los derechos reservados.<br>
                <small>Este es un mensaje automático, por favor no respondas directamente.</small>
            </div>
        </div>
    </body>
    </html>
    ";
}

}