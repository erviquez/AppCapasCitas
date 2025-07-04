using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Request.Paciente;
using AppCapasCitas.DTO.Response.Medico;
using AutoMapper;

namespace AppCapasCitas.Application.Mappings;

public class MappingProfile : Profile
{
    //Importante
    public MappingProfile()
    {
        CreateMap<Medico, MedicoResponse>()
            // .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.UsuarioNavigation!.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UsuarioNavigation!.Email))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.UsuarioNavigation!.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.UsuarioNavigation!.Apellido))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.UsuarioNavigation!.Telefono))
            .ForMember(dest => dest.Celular, opt => opt.MapFrom(src => src.UsuarioNavigation!.Celular))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.UsuarioNavigation!.Direccion))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.UsuarioNavigation!.Ciudad))
            .ForMember(dest => dest.CodigoPais, opt => opt.MapFrom(src => src.UsuarioNavigation!.CodigoPais))
            .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.UsuarioNavigation!.Pais))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.UsuarioNavigation!.Estado))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.UsuarioNavigation!.Activo))
            .ForMember(dest => dest.UltimoLogin, opt => opt.MapFrom(src => src.UsuarioNavigation!.UltimoLogin))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.UsuarioNavigation!.FechaCreacion))
            .ForMember(dest => dest.CreadoPor, opt => opt.MapFrom(src => src.UsuarioNavigation!.CreadoPor))


            ;
        // Esto permite el mapeo explícito de listas
        // CreateMap<IEnumerable<Medico>, IReadOnlyList<MedicoResponse>>()
        // .ConvertUsing(src => src.Select(m => MapMedicoToResponse(m)).ToList().AsReadOnly());


        CreateMap<Paciente, PacienteResponse>()
            .ForMember(dest => dest.PacienteId, opt => opt.MapFrom(src => src.UsuarioNavigation!.Id))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.UsuarioNavigation!.Email))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.UsuarioNavigation!.Nombre))
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.UsuarioNavigation!.Apellido))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.UsuarioNavigation!.Telefono))
            .ForMember(dest => dest.Celular, opt => opt.MapFrom(src => src.UsuarioNavigation!.Celular))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.UsuarioNavigation!.Direccion))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.UsuarioNavigation!.Ciudad))
            .ForMember(dest => dest.CodigoPais, opt => opt.MapFrom(src => src.UsuarioNavigation!.CodigoPais))
            .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.UsuarioNavigation!.Pais))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.UsuarioNavigation!.Estado))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.UsuarioNavigation!.Activo))
            .ForMember(dest => dest.UltimoLogin, opt => opt.MapFrom(src => src.UsuarioNavigation!.UltimoLogin))
            .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.UsuarioNavigation!.FechaCreacion))
            .ForMember(dest => dest.CreadoPor, opt => opt.MapFrom(src => src.UsuarioNavigation!.CreadoPor))
            .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
            .ForMember(dest => dest.Sexo, opt => opt.MapFrom(src => src.Sexo))
            .ForMember(dest => dest.TipoSangre, opt => opt.MapFrom(src => src.TipoSangre))
            .ForMember(dest => dest.NumeroIdentificacion, opt => opt.MapFrom(src => src.NumeroIdentificacion))
            .ForMember(dest => dest.EstadoCivil, opt => opt.MapFrom(src => src.EstadoCivil))
            .ForMember(dest => dest.Ocupacion, opt => opt.MapFrom(src => src.Ocupacion))
            .ForMember(dest => dest.Nacionalidad, opt => opt.MapFrom(src => src.Nacionalidad))
            .ForMember(dest => dest.Idiomas, opt => opt.MapFrom(src => src.Idiomas))
            .ForMember(dest => dest.Alergias, opt => opt.MapFrom(src => src.Alergias))
            .ForMember(dest => dest.EnfermedadesCronicas, opt => opt.MapFrom(src => src.EnfermedadesCronicas))
            .ForMember(dest => dest.MedicamentosActuales, opt => opt.MapFrom(src => src.MedicamentosActuales))
            .ForMember(dest => dest.AntecedentesFamiliares, opt => opt.MapFrom(src => src.AntecedentesFamiliares))
            .ForMember(dest => dest.AntecedentesPersonales, opt => opt.MapFrom(src => src.AntecedentesPersonales))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));


            
            
      
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
            // otras propiedades..
        };
    }
}
