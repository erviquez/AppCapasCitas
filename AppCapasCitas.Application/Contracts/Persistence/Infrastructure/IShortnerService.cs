using System;
using AppCapasCitas.Transversal.Common;

namespace AppCapasCitas.Application.Contracts.Persistence.Infrastructure;

public interface IShortnerService
{
    Task<Response<string>> ShortenUrlAsync(string url);
    Task<Response<string>> CreateUrlAsync(string typeUrl, string[] parameters);
}
