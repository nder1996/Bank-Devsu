using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Enums;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BancoAPI.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {

        private readonly BancoDbContext _context;

        public CuentaRepository(BancoDbContext context)
        {
            _context = context;
        }

        async Task<bool> ICuentaRepository.ActualizarEstadoAsync(long cuentaId, bool nuevoEstado)
        {
            var cuenta = await _context.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
                return false;

            cuenta.Estado = nuevoEstado;
            _context.Cuentas.Update(cuenta);
            return await _context.SaveChangesAsync() > 0;
        }

        async Task<bool> ICuentaRepository.ActualizarTipoCuentaAsync(long cuentaId, string nuevoTipo)
        {
            var cuenta = await _context.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
                return false;

            if (Enum.TryParse(typeof(TipoCuenta), nuevoTipo, true, out var tipoEnum))
            {
                cuenta.TipoCuenta = (TipoCuenta)tipoEnum;
                _context.Cuentas.Update(cuenta);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }

        async Task<Cuenta> ICuentaRepository.CrearAsync(Cuenta cuenta)
        {
            await _context.Cuentas.AddAsync(cuenta);
            await _context.SaveChangesAsync();
            return cuenta;
        }

        async Task<Cuenta> ICuentaRepository.ActualizarAsync(Cuenta cuenta)
        {
            var cuentaExistente = await _context.Cuentas.FindAsync(cuenta.Id);
            if (cuentaExistente == null)
                return null;

            cuentaExistente.NumeroCuenta = cuenta.NumeroCuenta;
            cuentaExistente.TipoCuenta = cuenta.TipoCuenta;
            cuentaExistente.SaldoInicial = cuenta.SaldoInicial;
            cuentaExistente.Estado = cuenta.Estado;
            cuentaExistente.ClienteId = cuenta.ClienteId;

            _context.Cuentas.Update(cuentaExistente);
            await _context.SaveChangesAsync();
            
            return await _context.Cuentas
                .Include(c => c.ClienteNavigation)
                .FirstOrDefaultAsync(c => c.Id == cuentaExistente.Id);
        }

        async Task<bool> ICuentaRepository.EliminarAsync(long cuentaId)
        {
            var cuenta = await _context.Cuentas.FindAsync(cuentaId);
            if (cuenta == null)
            {
                return false;
            }
            cuenta.Estado = false;
            _context.Cuentas.Update(cuenta);
            return await _context.SaveChangesAsync() > 0;
        }

        async Task<IEnumerable<Cuenta>> ICuentaRepository.ObtenerPorClienteIdAsync(long clienteId)
        {
            return await _context.Cuentas
                .Where(c => c.ClienteId == clienteId)
                .Include(c => c.ClienteNavigation)
                .ToListAsync();
        }

        async Task<Cuenta> ICuentaRepository.ObtenerPorIdAsync(long cuentaId)
        {
            return await _context.Cuentas
                .Include(c => c.ClienteNavigation)
                .FirstOrDefaultAsync(c => c.Id == cuentaId);
        }

        async Task<Cuenta> ICuentaRepository.ObtenerPorNumeroCuentaAsync(string numeroCuenta)
        {
            return await _context.Cuentas
               .Include(c => c.ClienteNavigation)
               .FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        async Task<IEnumerable<Cuenta>> ICuentaRepository.ObtenerTodasAsync()
        {
            return await _context.Cuentas
            .Where(c => c.Estado == true)
            .Include(c => c.ClienteNavigation)
            .ToListAsync();

        }
    }
    }
