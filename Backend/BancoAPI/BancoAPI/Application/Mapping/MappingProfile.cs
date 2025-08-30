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
                    nombre = src.ClienteNavigation != null && src.ClienteNavigation.Persona != null ? src.ClienteNavigation.Persona.Nombre : "Sin cliente"
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

            CreateMap<CuentaDto, Cuenta>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.NumeroCuenta, opt => opt.MapFrom(src => src.numeroCuenta))
                .ForMember(dest => dest.TipoCuenta, opt => opt.MapFrom(src => src.tipoCuenta))
                .ForMember(dest => dest.SaldoInicial, opt => opt.MapFrom(src => src.saldoInicial))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.estado))
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.cliente.id))
                .ForMember(dest => dest.ClienteNavigation, opt => opt.Ignore())
                .ForMember(dest => dest.Movimientos, opt => opt.Ignore());

            // Mapeo Reporte -> ReporteDto
            CreateMap<Reporte, ReporteDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => new ReporteDto.ClienteResumido
                {
                    id = src.ClienteId,
                    nombre = src.Cliente != null && src.Cliente.Persona != null ? src.Cliente.Persona.Nombre : "Sin cliente"
                }));

            // Mapeo ReporteDto -> Reporte
            CreateMap<ReporteDto, Reporte>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.id))
                .ForMember(dest => dest.ClienteId, opt => opt.MapFrom(src => src.clienteId))
                .ForMember(dest => dest.FechaInicio, opt => opt.MapFrom(src => src.fechaInicio))
                .ForMember(dest => dest.FechaFin, opt => opt.MapFrom(src => src.fechaFin))
                .ForMember(dest => dest.Formato, opt => opt.MapFrom(src => src.formato))
                .ForMember(dest => dest.FechaGeneracion, opt => opt.MapFrom(src => src.fechaGeneracion))
                .ForMember(dest => dest.RutaArchivo, opt => opt.MapFrom(src => src.rutaArchivo))
                .ForMember(dest => dest.NombreArchivo, opt => opt.MapFrom(src => src.nombreArchivo))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.activo))
                .ForMember(dest => dest.Cliente, opt => opt.Ignore()); // Se maneja por separado

        }
    }
}
