using AppCapasCitas.Application.Models;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.Application.Contracts.Persistence.Infrastructure;

public interface IEmailService
{
     Task<Response<bool>> SendEmail(Email email);
}
