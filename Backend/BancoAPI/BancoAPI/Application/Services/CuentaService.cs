using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Application.Services
{
    public class CuentaService : ICuentaService
    {

        private readonly ICuentaRepository _cuentaRepository;
        private readonly IMapper _mapper;


        public CuentaService(ICuentaRepository cuentaRepository, IMapper mapper)
        {
            _cuentaRepository = cuentaRepository;
            _mapper = mapper;
        }


        Task<bool> ICuentaService.ActualizarEstadoAsync(long cuentaId, bool nuevoEstado)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICuentaService.ActualizarTipoCuentaAsync(long cuentaId, string nuevoTipo)
        {
            throw new NotImplementedException();
        }

        Task<Cuenta> ICuentaService.CrearAsync(Cuenta cuenta)
        {
            throw new NotImplementedException();
        }

        Task<bool> ICuentaService.EliminarAsync(long cuentaId)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<Cuenta>> ICuentaService.ObtenerPorClienteIdAsync(long clienteId)
        {
            throw new NotImplementedException();
        }

        Task<Cuenta> ICuentaService.ObtenerPorIdAsync(long cuentaId)
        {
            throw new NotImplementedException();
        }

        Task<Cuenta> ICuentaService.ObtenerPorNumeroCuentaAsync(string numeroCuenta)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<CuentaDto>> ICuentaService.ObtenerTodasAsync()
        {
            var cuentas = await _cuentaRepository.ObtenerTodasAsync();
            return _mapper.Map<IEnumerable<CuentaDto>>(cuentas);
        }
    }
}
