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


        public async Task<bool> ActualizarEstadoAsync(long cuentaId, bool nuevoEstado)
        {
            try
            {
                if (cuentaId <= 0)
                    throw new ArgumentException("El id de la cuenta debe ser mayor a cero.", nameof(cuentaId));

                return await _cuentaRepository.ActualizarEstadoAsync(cuentaId, nuevoEstado);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el estado de la cuenta.", ex);
            }
        }

        public async Task<bool> ActualizarTipoCuentaAsync(long cuentaId, string nuevoTipo)
        {
            try
            {
                if (cuentaId <= 0)
                    throw new ArgumentException("El id de la cuenta debe ser mayor a cero.", nameof(cuentaId));
                if (string.IsNullOrWhiteSpace(nuevoTipo))
                    throw new ArgumentException("El nuevo tipo de cuenta es requerido.", nameof(nuevoTipo));

                return await _cuentaRepository.ActualizarTipoCuentaAsync(cuentaId, nuevoTipo);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el tipo de la cuenta.", ex);
            }
        }

        public async Task<CuentaDto> CrearAsync(CuentaDto cuentaDto)
        {
            try
            {
                if (cuentaDto is null)
                    throw new ArgumentNullException(nameof(cuentaDto), "La cuenta no puede ser nula.");
                if (string.IsNullOrWhiteSpace(cuentaDto.numeroCuenta))
                    throw new ArgumentException("El número de cuenta es requerido.", nameof(cuentaDto.numeroCuenta));
                if (cuentaDto.saldoInicial < 0)
                    throw new ArgumentException("El saldo inicial debe ser mayor o igual a cero.", nameof(cuentaDto.saldoInicial));

                // Mapeo de DTO a entidad
                var cuentaEntity = _mapper.Map<Cuenta>(cuentaDto);
                var cuentaCreada = await _cuentaRepository.CrearAsync(cuentaEntity);

                // Mapeo de entidad a DTO
                return _mapper.Map<CuentaDto>(cuentaCreada);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al crear la cuenta.", ex);
            }
        }

        public async Task<CuentaDto> ActualizarAsync(long cuentaId, CuentaDto cuentaDto)
        {
            try
            {
                if (cuentaId <= 0)
                    throw new ArgumentException("El id de la cuenta debe ser mayor a cero.", nameof(cuentaId));
                if (cuentaDto is null)
                    throw new ArgumentNullException(nameof(cuentaDto), "La cuenta no puede ser nula.");
                if (string.IsNullOrWhiteSpace(cuentaDto.numeroCuenta))
                    throw new ArgumentException("El número de cuenta es requerido.", nameof(cuentaDto.numeroCuenta));
                if (cuentaDto.saldoInicial < 0)
                    throw new ArgumentException("El saldo inicial debe ser mayor o igual a cero.", nameof(cuentaDto.saldoInicial));

                // Mapeo de DTO a entidad
                var cuentaEntity = _mapper.Map<Cuenta>(cuentaDto);
                cuentaEntity.Id = cuentaId;
                
                var cuentaActualizada = await _cuentaRepository.ActualizarAsync(cuentaEntity);
                if (cuentaActualizada == null)
                    throw new Exception("No se pudo actualizar la cuenta.");

                // Mapeo de entidad a DTO
                return _mapper.Map<CuentaDto>(cuentaActualizada);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar la cuenta.", ex);
            }
        }

        public async Task<bool> EliminarAsync(long cuentaId)
        {
            try
            {
                if (cuentaId <= 0)
                    throw new ArgumentException("El id de la cuenta debe ser mayor a cero.", nameof(cuentaId));

                return await _cuentaRepository.EliminarAsync(cuentaId);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar la cuenta.", ex);
            }
        }


        public async Task<CuentaDto> ObtenerPorIdAsync(long cuentaId)
        {
            try
            {
                if (cuentaId <= 0)
                    throw new ArgumentException("El id de la cuenta debe ser mayor a cero.", nameof(cuentaId));

                var cuenta = await _cuentaRepository.ObtenerPorIdAsync(cuentaId);
                return _mapper.Map<CuentaDto>(cuenta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cuenta por id.", ex);
            }
        }

        public async Task<CuentaDto> ObtenerPorNumeroCuentaAsync(string numeroCuenta)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(numeroCuenta))
                    throw new ArgumentException("El número de cuenta es requerido.", nameof(numeroCuenta));

                var cuenta = await _cuentaRepository.ObtenerPorNumeroCuentaAsync(numeroCuenta);
                return _mapper.Map<CuentaDto>(cuenta);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la cuenta por número de cuenta.", ex);
            }
        }

        public async Task<IEnumerable<CuentaDto>> ObtenerTodasAsync()
        {
            try
            {
                var cuentas = await _cuentaRepository.ObtenerTodasAsync();
                return _mapper.Map<IEnumerable<CuentaDto>>(cuentas);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener todas las cuentas.", ex);
            }
        }
    }
}
