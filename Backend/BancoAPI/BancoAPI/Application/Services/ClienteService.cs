using AutoMapper;
using BancoAPI.Application.DTOs;
using BancoAPI.Domain.Entities;
using BancoAPI.Domain.Interfaces.Repositories;
using BancoAPI.Domain.Interfaces.Services;

namespace BancoAPI.Application.Services
{
    public class ClienteService : IClienteService
    {


        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;


        public ClienteService(IClienteRepository clienteRepository, IMapper mapper)
        {
            _clienteRepository = clienteRepository;
            _mapper = mapper;
        }


        Task<Cliente> IClienteService.ActualizarAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        Task<bool> IClienteService.ActualizarEstadoAsync(int clienteId, bool nuevoEstado)
        {
            throw new NotImplementedException();
        }

        Task<Cliente> IClienteService.CrearAsync(Cliente cliente)
        {
            throw new NotImplementedException();
        }

        Task<bool> IClienteService.EliminarAsync(int clienteId)
        {
            throw new NotImplementedException();
        }

        Task<Cliente> IClienteService.ObtenerPorIdAsync(int clienteId)
        {
            throw new NotImplementedException();
        }

        Task<Cliente> IClienteService.ObtenerPorIdentificacionAsync(string identificacion)
        {
            throw new NotImplementedException();
        }

        async Task<IEnumerable<ClienteDto>> IClienteService.ObtenerTodosAsync()
        {
            var clientes = await _clienteRepository.ObtenerTodosAsync();
            return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
        }

    }
}
