using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Services
{
    public interface IMovimientoService
    {
        Task<MovimientoDto> CreateAsync(MovimientoDto movimiento);
        Task<MovimientoDto?> GetByIdAsync(int id);
        Task<IEnumerable<MovimientoDto>> GetAllAsync();
        Task<MovimientoDto> UpdateAsync(MovimientoDto movimiento);
        Task<bool> DeleteAsync(int id);
    }
}
