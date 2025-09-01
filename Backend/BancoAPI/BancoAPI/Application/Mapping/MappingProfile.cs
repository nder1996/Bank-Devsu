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
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.numeroCuenta, opt => opt.MapFrom(src => src.NumeroCuenta))
                .ForMember(dest => dest.tipoCuenta, opt => opt.MapFrom(src => src.TipoCuenta))
                .ForMember(dest => dest.saldoInicial, opt => opt.MapFrom(src => src.SaldoInicial))
                .ForMember(dest => dest.estado, opt => opt.MapFrom(src => src.Estado))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => new CuentaDto.ClienteResumido
                {
                    id = src.ClienteNavigation != null ? src.ClienteNavigation.Id : 0,
                    nombre = src.ClienteNavigation != null ? src.ClienteNavigation.Nombre : "Sin cliente"
                }));


            CreateMap<Movimiento, MovimientoDto>()
                .ForMember(dest => dest.cuenta, opt => opt.MapFrom(src => new MovimientoDto.CuentaResumida
                {
                    id = src.CuentaId,
                    numeroCuenta = src.Cuenta != null ? src.Cuenta.NumeroCuenta : null
                }));

            CreateMap<MovimientoDto, Movimiento>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.fecha))
                .ForMember(dest => dest.TipoMovimiento, opt => opt.MapFrom(src => src.tipoMovimiento))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.valor))
                .ForMember(dest => dest.Saldo, opt => opt.MapFrom(src => src.saldo))
                .ForMember(dest => dest.CuentaId, opt => opt.MapFrom(src => src.cuenta.id))
                .ForMember(dest => dest.Cuenta, opt => opt.Ignore()); // Se maneja por separado

            // Mapeo Cliente -> ClienteDto (hereda de Persona)
            CreateMap<Cliente, ClienteDto>()
               .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
               .ForMember(dest => dest.nombre, opt => opt.MapFrom(src => src.Nombre))
               .ForMember(dest => dest.identificacion, opt => opt.MapFrom(src => src.Identificacion))
               .ForMember(dest => dest.direccion, opt => opt.MapFrom(src => src.Direccion))
               .ForMember(dest => dest.telefono, opt => opt.MapFrom(src => src.Telefono))
               .ForMember(dest => dest.edad, opt => opt.MapFrom(src => src.Edad))
               .ForMember(dest => dest.genero, opt => opt.MapFrom(src => src.Genero))
               .ForMember(dest => dest.estado, opt => opt.MapFrom(src => src.Estado))
               .ForMember(dest => dest.cuentas, opt => opt.MapFrom(src => src.Cuentas));

            // Mapeo ClienteDto -> Cliente (hereda de Persona)
            CreateMap<ClienteDto, Cliente>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
               .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.nombre))
               .ForMember(dest => dest.Identificacion, opt => opt.MapFrom(src => src.identificacion))
               .ForMember(dest => dest.Direccion, opt => opt.MapFrom(src => src.direccion))
               .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.telefono))
               .ForMember(dest => dest.Edad, opt => opt.MapFrom(src => src.edad))
               .ForMember(dest => dest.Genero, opt => opt.MapFrom(src => src.genero))
               .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado))
               .ForMember(dest => dest.Contrasena, opt => opt.Ignore()) // Se maneja por separado
               .ForMember(dest => dest.Cuentas, opt => opt.Ignore()); // Se maneja por separado

            CreateMap<CuentaDto, Cuenta>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.numeroCuenta))
                .ForMember(dest => dest.TipoCuenta, opt => opt.MapFrom(src => src.tipoCuenta))
                .ForMember(dest => dest.SaldoInicial, opt => opt.MapFrom(src => src.saldoInicial))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado))
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.cliente.id))
                .ForMember(dest => dest.ClienteNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Movimientos, opt => opt.Ignore());


        }
    }
}
