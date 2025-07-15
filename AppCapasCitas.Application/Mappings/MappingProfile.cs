using AppCapasCitas.Domain.Models;
using AppCapasCitas.DTO.Response.Cita;
using AppCapasCitas.DTO.Response.Especialidad;
using AppCapasCitas.DTO.Response.Medico;
using AppCapasCitas.DTO.Response.Paciente;
using AppCapasCitas.DTO.Response.Usuario;
using AutoMapper;

namespace AppCapasCitas.Application.Mappings;

public class MappingProfile : Profile
{
    //Importante
    public MappingProfile()
    {
        CreateMap<HorarioTrabajo, HorarioTrabajoResponse>()
           .ForMember(dest => dest.HorarioTrabajoId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.DiaSemana, opt => opt.MapFrom(src => src.DiaSemana))
           .ForMember(dest => dest.HoraInicio, opt => opt.MapFrom(src => src.HoraInicio))
           .ForMember(dest => dest.HoraFin, opt => opt.MapFrom(src => src.HoraFin))
           .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.MedicoId));

        CreateMap<Especialidad, EspecialidadResponse>()
            .ForMember(dest => dest.EspecialidadId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.CostoConsultaBase, opt => opt.MapFrom(src => src.CostoConsultaBase))
            .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
            .ForMember(dest => dest.HospitalResponse, opt => opt.MapFrom(src => src.MedicoEspecialidadHospitales!
                .Select(x => x.HospitalNavigation)
                .FirstOrDefault()));

        CreateMap<Medico, MedicoResponse>()
           .ForPath(dest => dest.UsuarioResponse!.UsuarioId, opt => opt.MapFrom(src => src.UsuarioNavigation!.Id))
           .ForPath(dest => dest.UsuarioResponse!.Email, opt => opt.MapFrom(src => src.UsuarioNavigation!.Email))
           .ForPath(dest => dest.UsuarioResponse!.Nombre, opt => opt.MapFrom(src => src.UsuarioNavigation!.Nombre))
           .ForPath(dest => dest.UsuarioResponse!.Apellido, opt => opt.MapFrom(src => src.UsuarioNavigation!.Apellido))
           .ForPath(dest => dest.UsuarioResponse!.Telefono, opt => opt.MapFrom(src => src.UsuarioNavigation!.Telefono))
           .ForPath(dest => dest.UsuarioResponse!.Celular, opt => opt.MapFrom(src => src.UsuarioNavigation!.Celular))
           .ForPath(dest => dest.UsuarioResponse!.Direccion, opt => opt.MapFrom(src => src.UsuarioNavigation!.Direccion))
           .ForPath(dest => dest.UsuarioResponse!.Ciudad, opt => opt.MapFrom(src => src.UsuarioNavigation!.Ciudad))
           .ForPath(dest => dest.UsuarioResponse!.CodigoPais, opt => opt.MapFrom(src => src.UsuarioNavigation!.CodigoPais))
           .ForPath(dest => dest.UsuarioResponse!.Pais, opt => opt.MapFrom(src => src.UsuarioNavigation!.Pais))
           .ForPath(dest => dest.UsuarioResponse!.Estado, opt => opt.MapFrom(src => src.UsuarioNavigation!.Estado))
           .ForPath(dest => dest.UsuarioResponse!.Activo, opt => opt.MapFrom(src => src.UsuarioNavigation!.Activo))
           .ForPath(dest => dest.UsuarioResponse!.UltimoLogin, opt => opt.MapFrom(src => src.UsuarioNavigation!.UltimoLogin))
           .ForPath(dest => dest.UsuarioResponse!.FechaCreacion, opt => opt.MapFrom(src => src.UsuarioNavigation!.FechaCreacion))
           .ForPath(dest => dest.UsuarioResponse!.CreadoPor, opt => opt.MapFrom(src => src.UsuarioNavigation!.CreadoPor))
           .ForMember(dest => dest.CedulaProfesional, opt => opt.MapFrom(src => src.CedulaProfesional))
           .ForMember(dest => dest.MedicoId, opt => opt.MapFrom(src => src.UsuarioNavigation!.Id))
           .ForMember(dest => dest.Biografia, opt => opt.MapFrom(src => src.Biografia))
           .ForMember(dest => dest.Universidad, opt => opt.MapFrom(src => src.Universidad))
           .ForMember(dest => dest.ListHorarioTrabajoResponse, opt => opt.MapFrom(src => src.HorariosTrabajo))
           .ForMember(dest => dest.ListEspecialidadResponse, opt => opt.MapFrom(src => src.MedicoEspecialidadHospitales));

        CreateMap<Paciente, PacienteResponse>()
            .ForPath(dest => dest.UsuarioResponse!.UsuarioId, opt => opt.MapFrom(src => src.UsuarioNavigation!.Id))
            .ForPath(dest => dest.UsuarioResponse!.RoleId, opt => opt.MapFrom(src => src.UsuarioNavigation!.RoleId))
            .ForPath(dest => dest.UsuarioResponse!.RoleName, opt => opt.MapFrom(src => src.UsuarioNavigation!.RolName))
            .ForPath(dest => dest.UsuarioResponse!.Email, opt => opt.MapFrom(src => src.UsuarioNavigation!.Email))
            .ForPath(dest => dest.UsuarioResponse!.Nombre, opt => opt.MapFrom(src => src.UsuarioNavigation!.Nombre))
            .ForPath(dest => dest.UsuarioResponse!.Apellido, opt => opt.MapFrom(src => src.UsuarioNavigation!.Apellido))
            .ForPath(dest => dest.UsuarioResponse!.Telefono, opt => opt.MapFrom(src => src.UsuarioNavigation!.Telefono))
            .ForPath(dest => dest.UsuarioResponse!.Celular, opt => opt.MapFrom(src => src.UsuarioNavigation!.Celular))
            .ForPath(dest => dest.UsuarioResponse!.Direccion, opt => opt.MapFrom(src => src.UsuarioNavigation!.Direccion))
            .ForPath(dest => dest.UsuarioResponse!.Ciudad, opt => opt.MapFrom(src => src.UsuarioNavigation!.Ciudad))
            .ForPath(dest => dest.UsuarioResponse!.CodigoPais, opt => opt.MapFrom(src => src.UsuarioNavigation!.CodigoPais))
            .ForPath(dest => dest.UsuarioResponse!.Pais, opt => opt.MapFrom(src => src.UsuarioNavigation!.Pais))
            .ForPath(dest => dest.UsuarioResponse!.Estado, opt => opt.MapFrom(src => src.UsuarioNavigation!.Estado))
            .ForPath(dest => dest.UsuarioResponse!.Activo, opt => opt.MapFrom(src => src.UsuarioNavigation!.Activo))
            .ForPath(dest => dest.UsuarioResponse!.UltimoLogin, opt => opt.MapFrom(src => src.UsuarioNavigation!.UltimoLogin))
            .ForPath(dest => dest.UsuarioResponse!.FechaCreacion, opt => opt.MapFrom(src => src.UsuarioNavigation!.FechaCreacion))
            .ForPath(dest => dest.UsuarioResponse!.CreadoPor, opt => opt.MapFrom(src => src.UsuarioNavigation!.CreadoPor))
            .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FechaNacimiento))
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
            .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Genero))
            .ForMember(dest => dest.EstadoCivil, opt => opt.MapFrom(src => src.EstadoCivil))
            .ForMember(dest => dest.Ocupacion, opt => opt.MapFrom(src => src.Ocupacion))
            .ForMember(dest => dest.Alergias, opt => opt.MapFrom(src => src.Alergias))
            .ForMember(dest => dest.EnfermedadesCronicas, opt => opt.MapFrom(src => src.EnfermedadesCronicas))
            .ForMember(dest => dest.AntecedentesFamiliares, opt => opt.MapFrom(src => src.AntecedentesFamiliares))
            .ForMember(dest => dest.AntecedentesPersonales, opt => opt.MapFrom(src => src.AntecedentesPersonales))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones))
            .ForMember(dest => dest.TipoSangreResponse, opt => opt.MapFrom(src => src.TipoSangreNavigation))
            .ForMember(dest => dest.ListContactoResponse, opt => opt.MapFrom(src => src.Contactos))
            .ForMember(dest => dest.ListAseguradoraResponse, opt => opt.MapFrom(src => src.Aseguradoras));

        CreateMap<TipoSangre, TipoSangreResponse>()
            .ForMember(dest => dest.TipoSangreId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Descripcion))
            .ForMember(dest => dest.Antigenos, opt => opt.MapFrom(src => src.Antigenos))
            .ForMember(dest => dest.Anticuerpos, opt => opt.MapFrom(src => src.Anticuerpos))
            .ForMember(dest => dest.RecibirDe, opt => opt.MapFrom(src => src.RecibirDe))
            .ForMember(dest => dest.DonarA, opt => opt.MapFrom(src => src.DonarA));
        CreateMap<Aseguradora, AseguradoraResponse>()
            .ForMember(dest => dest.AseguradoraId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono))
            .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Direccion))
            .ForMember(dest => dest.Ciudad, opt => opt.MapFrom(src => src.Ciudad))
            .ForMember(dest => dest.CodigoPostal, opt => opt.MapFrom(src => src.CodigoPostal))
            .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado))
            .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais))
            .ForMember(dest => dest.Observaciones, opt => opt.MapFrom(src => src.Observaciones));
        CreateMap<Contacto, ContactoResponse>()
            .ForMember(dest => dest.ContactoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));


        CreateMap<Pago, PagoResponse>()
            .ForMember(dest => dest.PagoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.PacienteNombre, opt => opt.Ignore())
            .ForMember(dest => dest.CitaFechaHora, opt => opt.Ignore())
            .ForMember(dest => dest.CitaMotivo, opt => opt.Ignore());
        CreateMap<Contacto, ContactoResponse>()
            .ForMember(dest => dest.ContactoId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre))
            .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Telefono));
        }


}
