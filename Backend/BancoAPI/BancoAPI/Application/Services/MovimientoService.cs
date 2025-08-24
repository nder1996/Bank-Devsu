using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;
using BancoAPI.Infrastructure.Repositories;

namespace BancoAPI.Application.Services
{
    public class MovimientoService : IMovimientoService
    {

        private readonly IMovimientoRepository _movimientoRepository;
        private readonly IMapper _mapper;


        public MovimientoService(IMovimientoRepository movimientoRepository, IMapper mapper)
        {
            _movimientoRepository = movimientoRepository;
            _mapper = mapper;
        }

        Task<Movimiento> IMovimientoService.CreateAsync(Movimiento movimiento)
        {
            throw new NotImplementedException();
        }

        Task<bool> IMovimientoService.DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<MovimientoDto>> IMovimientoService.GetAllAsync()
        {
            var movimiento = await _movimientoRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<MovimientoDto>>(movimiento);
        }

        Task<Movimiento?> IMovimientoService.GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        Task<Movimiento> IMovimientoService.UpdateAsync(Movimiento movimiento)
        {
            throw new NotImplementedException();
        }
    }
}
