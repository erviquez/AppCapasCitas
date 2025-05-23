using System.Net;
using System.Net.Mail;
using AppCapasCitas.Application.Contracts.Persistence.Infrastructure;
using AppCapasCitas.Application.Models.Identity;
using AppCapasCitas.Transversal.Common;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace AppCapasCitas.Infrastructure.Repositories.Email;

public class EmailService : IEmailService
{
    public EmailSettings? _emailSettings { get; }
    private readonly IAppLogger<EmailService>? _appLogger;

    public EmailService() { }


    public EmailService(IOptions<EmailSettings> emailSettings, IAppLogger<EmailService> appLogger)
    {
        _emailSettings = emailSettings.Value;
        _appLogger = appLogger;
    }

    public async Task<Response<bool>> SendEmail(Application.Models.Email email)
    {
        var response = new Response<bool>();
        try
        {
            using (SmtpClient client = new SmtpClient(_emailSettings!.Server, int.Parse(_emailSettings.Port!)))
            {
                client.Credentials = new NetworkCredential(_emailSettings.Email, _emailSettings.Secret);
                client.EnableSsl = true;
                var correo = new MailMessage(_emailSettings.Email!, email.To!)
                {
                    Subject = email.Subject,
                    Body = email.Body,
                    IsBodyHtml = true
                };
                await client.SendMailAsync(correo);
            }
            response.IsSuccess = true;
        }
        catch (Exception ex)
        {
            var message = "Ocurrio un error al enviar el email;" + ex.InnerException?.Message; ;
            _appLogger!.LogError(message);
            response.IsSuccess = true;
            response.Message = message;
            response.Errors = new List<ValidationFailure>
            {
                new ValidationFailure("Error", ex.InnerException?.Message ?? "Error desconocido")
            };
        }
        return response;    

    }
}
