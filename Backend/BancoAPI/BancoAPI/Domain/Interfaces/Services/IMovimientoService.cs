using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IMovimientoService
    {
        Task<Movimiento> CreateAsync(Movimiento movimiento);
        Task<Movimiento?> GetByIdAsync(int id);
        Task<IEnumerable<MovimientoDto>> GetAllAsync();
        Task<Movimiento> UpdateAsync(Movimiento movimiento);
        Task<bool> DeleteAsync(int id);
    }
}
