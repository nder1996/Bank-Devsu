using BancoAPI.Domain.Entities;

namespace BancoAPI.Domain.Interfaces.Repositories
{
    public interface IMovimientoRepository
    {
        Task<Movimiento> CreateAsync(Movimiento movimiento);
        Task<Movimiento?> GetByIdAsync(int id);
        Task<IEnumerable<Movimiento>> GetAllAsync();
        Task<Movimiento> UpdateAsync(Movimiento movimiento);
        Task<bool> DeleteAsync(int id);
    }
}
