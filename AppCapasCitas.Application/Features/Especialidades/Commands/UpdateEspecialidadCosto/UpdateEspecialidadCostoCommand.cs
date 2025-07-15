using System;
using AppCapasCitas.Transversal.Common;
using MediatR;

namespace AppCapasCitas.Application.Features.Especialidades.Commands.UpdateEspecialidadCosto;

public class UpdateEspecialidadCostoCommand : IRequest<Response<bool>>
{
    public Guid EspecialidadId { get; set; }
    public decimal CostoConsultaBase { get; set; }
    public Guid? UsuarioModificacionId { get; set; }

}
