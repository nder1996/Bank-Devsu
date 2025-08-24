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
                .ForMember(dest => dest.Cliente, opt => opt.MapFrom(src => new CuentaDto.ClienteResumido
                {
                Id = src.ClienteNavigation.Id,
                Nombre = src.ClienteNavigation.Persona != null ? src.ClienteNavigation.Persona.Nombre : null
                }));


            CreateMap<Movimiento, MovimientoDto>()
                .ForMember(dest => dest.Cuenta, opt => opt.MapFrom(src => new MovimientoDto.CuentaResumida
                {
                    Id = src.CuentaId,
                    NumeroCuenta = src.Cuenta != null ? src.Cuenta.NumeroCuenta : null
                }));

            // Mapeo Cliente -> ClienteDto (incluye datos de Persona)
            CreateMap<Cliente, ClienteDto>()
               .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Nombre : null))
               .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Identificacion : null))
               .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Direccion : null))
               .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Telefono : null))
               .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Edad : 0))
               .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.Persona != null ? src.Persona.Genero : null))
               .ForMember(dest => dest.Cuentas, opt => opt.MapFrom(src => src.Cuentas));

        }
    }
}
