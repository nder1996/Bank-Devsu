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


        public async Task<ClienteDto> ActualizarAsync(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");

            try
            {
                var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(cliente.Id);
                if (clienteExistente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                var clienteActualizado = await _clienteRepository.ActualizarAsync(cliente);
                return _mapper.Map<ClienteDto>(clienteActualizado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> ActualizarAsync(ClienteDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto), "El cliente no puede ser nulo.");

            try
            {
                var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(clienteDto.id);
                if (clienteExistente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                // Actualizar datos de Persona
                clienteExistente.Persona.Nombre = clienteDto.nombre;
                clienteExistente.Persona.Identificacion = clienteDto.identificacion;
                clienteExistente.Persona.Direccion = clienteDto.direccion;
                clienteExistente.Persona.Telefono = clienteDto.telefono;
                clienteExistente.Persona.Edad = clienteDto.edad;
                clienteExistente.Persona.Genero = clienteDto.genero;

                // Actualizar datos de Cliente
                clienteExistente.Estado = clienteDto.estado;
                
                // Solo actualizar contraseña si se proporciona
                if (!string.IsNullOrEmpty(clienteDto.contrasena))
                {
                    clienteExistente.Contrasena = clienteDto.contrasena;
                }

                var clienteActualizado = await _clienteRepository.ActualizarAsync(clienteExistente);
                return _mapper.Map<ClienteDto>(clienteActualizado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> ActualizarEstadoAsync(int clienteId, bool nuevoEstado)
        {
            if (clienteId <= 0)
                throw new ArgumentException("El identificador del cliente es inválido.", nameof(clienteId));

            try
            {
                var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
                if (clienteExistente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                return await _clienteRepository.ActualizarEstadoAsync(clienteId, nuevoEstado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> CrearAsync(Cliente cliente)
        {
            if (cliente == null)
                throw new ArgumentNullException(nameof(cliente), "El cliente no puede ser nulo.");

            try
            {
                var clienteCreado = await _clienteRepository.CrearAsync(cliente);
                return _mapper.Map<ClienteDto>(clienteCreado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> CrearAsync(ClienteDto clienteDto)
        {
            if (clienteDto == null)
                throw new ArgumentNullException(nameof(clienteDto), "El cliente no puede ser nulo.");

            try
            {
                var cliente = _mapper.Map<Cliente>(clienteDto);
                
                // Establecer contraseña (requerida para creación)
                cliente.Contrasena = clienteDto.contrasena ?? throw new ArgumentException("La contraseña es requerida para crear un cliente.");

                var clienteCreado = await _clienteRepository.CrearAsync(cliente);
                return _mapper.Map<ClienteDto>(clienteCreado);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> EliminarAsync(long clienteId)
        {
            if (clienteId <= 0)
                throw new ArgumentException("El identificador del cliente es inválido.", nameof(clienteId));

            try
            {
                var clienteExistente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
                if (clienteExistente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                return await _clienteRepository.EliminarAsync(clienteId);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> ObtenerPorIdAsync(int clienteId)
        {
            if (clienteId <= 0)
                throw new ArgumentException("El identificador del cliente es inválido.", nameof(clienteId));

            try
            {
                var cliente = await _clienteRepository.ObtenerPorIdAsync(clienteId);
                if (cliente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                return _mapper.Map<ClienteDto>(cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ClienteDto> ObtenerPorIdentificacionAsync(string identificacion)
        {
            if (string.IsNullOrWhiteSpace(identificacion))
                throw new ArgumentException("La identificación es requerida.", nameof(identificacion));

            try
            {
                var cliente = await _clienteRepository.ObtenerPorIdentificacionAsync(identificacion);
                if (cliente == null)
                    throw new KeyNotFoundException("Cliente no encontrado.");

                return _mapper.Map<ClienteDto>(cliente);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ClienteDto>> ObtenerTodosAsync()
        {
            try
            {
                var clientes = await _clienteRepository.ObtenerTodosAsync();
                return _mapper.Map<IEnumerable<ClienteDto>>(clientes);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
