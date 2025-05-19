using System;
using AppCapasCitas.Application.Contracts.Persistence;
using AppCapasCitas.Domain.Models;
using AppCapasCitas.Transversal.Common;
using FluentValidation;
using MediatR;

namespace AppCapasCitas.Application.Features.Medicos.Commands.CreateMedico;

public class CreateMedicoCommandHandler:IRequestHandler<CreateMedicoCommand, int>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IAsyncRepository<Usuario> _userRepository;
    private readonly IAsyncRepository<Medico> _medicoRepository;
    //pendiente
    //private readonly IAuthService _authService;
    private readonly IAppLogger<CreateMedicoCommandHandler> _appLogger;
    private readonly IValidator<CreateMedicoCommand> _validator;

    public CreateMedicoCommandHandler(IUnitOfWork unitOfWork, IAsyncRepository<Usuario> userRepository, IAsyncRepository<Medico> medicoRepository, IAppLogger<CreateMedicoCommandHandler> appLogger, IValidator<CreateMedicoCommand> validator)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _medicoRepository = medicoRepository;
        _appLogger = appLogger;
        _validator = validator;
    }

    public Task<int> Handle(CreateMedicoCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
