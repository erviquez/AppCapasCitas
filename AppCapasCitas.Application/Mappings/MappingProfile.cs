using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Medico;
using AutoMapper;

namespace AppCapasCitas.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Medico,MedicoResponse>()
            .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.IdentityId, opt => opt.MapFrom(src => src.Usuario!.IdentityId))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Usuario!.Email))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Usuario!.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Usuario!.Apellido))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Usuario!.Telefono))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))

            
            ;
        // Esto permite el mapeo explícito de listas
        // CreateMap<IEnumerable<Medico>, IReadOnlyList<MedicoResponse>>()
        // .ConvertUsing(src => src.Select(m => MapMedicoToResponse(m)).ToList().AsReadOnly());


        // CreateMap<Paciente,PacienteResponse>();
        // CreateMap<Especialidad,EspecialidadVm>();
        // CreateMap<HorarioTrabajo,HorarioTrabajoVm>()
        //     .ForMember(dest => dest.HorarioId, opt => opt.MapFrom(src => src.Id!))
        //     .ForMember(dest => dest.DiaSemana, opt => opt.MapFrom(src => src.DiaSemana));
        // CreateMap<HorarioTrabajo, HorarioTrabajoVm>()
        //     .ForMember(dest => dest.HorarioId, opt => opt.MapFrom(src => src.Id!));
    }

    private MedicoResponse MapMedicoToResponse(Medico medico)
    {
        // Aquí puedes poner lógica personalizada si necesitas
        return new MedicoResponse
        {
            MedicoId = medico.Id,
            // otras propiedades...
        };
    }
}
