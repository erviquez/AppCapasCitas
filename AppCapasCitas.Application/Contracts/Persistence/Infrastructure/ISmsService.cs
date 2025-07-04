using System;
using AppCapasCitas.Application.Models;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.Application.Contracts.Persistence.Infrastructure;

public interface ISmsService
{
     Task<Response<bool>> SendSms(Sms sms);
}
