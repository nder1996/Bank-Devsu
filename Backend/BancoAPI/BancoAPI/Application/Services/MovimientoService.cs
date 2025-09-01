using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Application.Exceptions;
using BancoAPI.Domain.Constants;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Enums;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;
using BancoAPI.Infrastructure.Data;
using BancoAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Application.Services
{
    public class MovimientoService : IMovimientoService
    {

        private readonly IMovimientoRepository _movimientoRepository;
        private readonly ICuentaRepository _cuentaRepository;
        private readonly BancoDbContext _context;
        private readonly IMapper _mapper;

        public MovimientoService(
            IMovimientoRepository movimientoRepository,
            IMapper mapper,
            ICuentaRepository cuentaRepository,
            BancoDbContext context)
        {
            _movimientoRepository = movimientoRepository;
            _mapper = mapper;
            _cuentaRepository = cuentaRepository;
            _context = context;
        }

        public async Task<MovimientoDto> CreateAsync(MovimientoDto movimientoDto)
        {
            if (movimientoDto == null)
                throw new ArgumentNullException(nameof(movimientoDto), "El movimiento no puede ser nulo.");

            // Mapear DTO a entidad
            var movimiento = _mapper.Map<Movimiento>(movimientoDto);

            // Usar transacción para garantizar consistencia
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var cuenta = await _cuentaRepository.ObtenerPorIdAsync(movimiento.CuentaId);
                if (cuenta == null)
                    throw new KeyNotFoundException($"No se encontró la cuenta con ID {movimiento.CuentaId}.");

                // Obtener saldo actual real (saldo inicial + todos los movimientos)
                var movimientosCuenta = await _movimientoRepository.GetByCuentaIdAsync(cuenta.Id);
                decimal saldoActual = cuenta.SaldoInicial + movimientosCuenta.Sum(m => m.Valor);

                // Procesar según tipo de movimiento
                if (movimiento.TipoMovimiento == TipoMovimiento.Debito)
                {
                    movimiento.Valor = -Math.Abs(movimiento.Valor);
                    // Validar saldo suficiente (incluyendo saldo cero)
                    if (saldoActual + movimiento.Valor < 0)
                        throw new SaldoNoDisponibleException("Saldo no disponible");

                    // Validar si saldo actual es cero y se intenta hacer débito
                    if (saldoActual == 0)
                        throw new SaldoNoDisponibleException("Saldo no disponible");

                    // Validar límite diario
                    var hoy = DateTime.Today;
                    var debitos = await _movimientoRepository.GetDebitosDiariosByCuentaIdAsync(cuenta.Id, hoy, hoy.AddDays(1));
                    decimal totalRetiradoHoy = debitos.Sum(m => Math.Abs(m.Valor));

                    if (totalRetiradoHoy + Math.Abs(movimiento.Valor) > AppConstants.LIMITE_DIARIO_RETIRO)
                        throw new CupoDiarioExcedidoException("Cupo diario Excedido");
                }
                else if (movimiento.TipoMovimiento == TipoMovimiento.Credito)
                {
                    movimiento.Valor = Math.Abs(movimiento.Valor);
                }
                else
                {
                    throw new ArgumentException("Tipo de movimiento no válido.", nameof(movimiento.TipoMovimiento));
                }

                // Calcular y asignar el nuevo saldo
                movimiento.Saldo = saldoActual + movimiento.Valor;

                // Persistir el movimiento
                var result = await _movimientoRepository.CreateAsync(movimiento);
                await transaction.CommitAsync();

                // Mapear entidad a DTO para devolver
                return _mapper.Map<MovimientoDto>(result);
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("El id del movimiento debe ser mayor a cero.", nameof(id));

                return await _movimientoRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar el movimiento.", ex);
            }
        }

        public async Task<IEnumerable<MovimientoDto>> GetAllAsync()
        {
            try
            {
                var movimientos = await _movimientoRepository.GetAllAsync();
                return _mapper.Map<IEnumerable<MovimientoDto>>(movimientos);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los movimientos.", ex);
            }
        }

        public async Task<MovimientoDto> UpdateAsync(MovimientoDto movimientoDto)
        {
            if (movimientoDto == null)
                throw new ArgumentNullException(nameof(movimientoDto), "El movimiento no puede ser nulo.");

            if (movimientoDto.cuenta == null)
                throw new ArgumentNullException(nameof(movimientoDto.cuenta), "La cuenta del movimiento no puede ser nula.");

            try
            {
                // Validar que la cuenta exista
                var cuenta = await _cuentaRepository.ObtenerPorIdAsync(movimientoDto.cuenta.id);
                if (cuenta == null)
                    throw new KeyNotFoundException($"No se encontró la cuenta con ID {movimientoDto.cuenta.id}.");

                // Mapear DTO a entidad y actualizar
                var movimientoEntity = _mapper.Map<Movimiento>(movimientoDto);
                var result = await _movimientoRepository.UpdateAsync(movimientoEntity);
                return _mapper.Map<MovimientoDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al actualizar el movimiento.", ex);
            }
        }
    }
}
