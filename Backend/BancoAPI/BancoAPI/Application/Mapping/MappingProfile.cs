using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cuenta, CuentaDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => new CuentaDto.ClienteResumido
                {
                id = src.ClienteNavigation.Id,
                nombre = src.ClienteNavigation.Persona != null ? src.ClienteNavigation.Persona.Nombre : null
                }));


            CreateMap<Movimiento, MovimientoDto>()
                .ForMember(dest => dest.cuenta, opt => opt.MapFrom(src => new MovimientoDto.CuentaResumida
                {
                    id = src.CuentaId,
                    numeroCuenta = src.Cuenta != null ? src.Cuenta.NumeroCuenta : null
                }));

            // Mapeo Cliente -> ClienteDto (incluye datos de Persona)
            CreateMap<Cliente, ClienteDto>()
               .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Nombre : null))
               .ForMember(dest => dest.identificacion, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Identificacion : null))
               .ForMember(dest => dest.direccion, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Direccion : null))
               .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Telefono : null))
               .ForMember(dest => dest.edad, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Edad : 0))
               .ForMember(dest => dest.genero, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Genero : null))
               .ForMember(dest => dest.cuentas, opt => opt.MapFrom(src => src.Cuentas));

            // Mapeo ClienteDto -> Cliente (crea/actualiza Persona)
            CreateMap<ClienteDto, Cliente>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
               .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado))
               .ForMember(dest => dest.Contrasena, opt => opt.Ignore()) // Se maneja por separado
               .ForMember(dest => dest.PersonaId, opt => opt.Ignore()) // Se asigna después de crear Persona
               .ForMember(dest => dest.Persona, opt => opt.MapFrom(src => new Persona
               {
                   Nombre = src.nombre,
                   Identificacion = src.identificacion,
                   Direccion = src.direccion,
                   Telefono = src.telefono,
                   Edad = src.edad,
                   Genero = src.genero
               }))
               .ForMember(dest => dest.Cuentas, opt => opt.Ignore()); // Se maneja por separado

            CreateMap<CuentaDto, Cuenta>().ReverseMap();

        }
    }
}
