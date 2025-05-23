using System;

namespace AppCapasCitas.Application.Contracts.Persistence;

// En Core/Interfaces
public interface ICurrentUserService
{
    string? UserId { get; }
    string? Username { get; }
}