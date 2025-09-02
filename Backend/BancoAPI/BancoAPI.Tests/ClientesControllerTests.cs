using BancoAPI.Application.DTOs;
using BancoAPI.Controllers;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BancoAPI.Tests
{
    public class ClientesControllerTests
    {
        private readonly Mock<IClienteService> _clienteServiceMock;
        private readonly ClientesController _controller;

        public ClientesControllerTests()
        {
            _clienteServiceMock = new Mock<IClienteService>();
            _controller = new ClientesController(_clienteServiceMock.Object);
        }

        [Fact]
        public async Task Get_DeberiaRetornarOkConListaDeClientes()
        {
            // Arrange
            var clientesEsperados = new List<ClienteDto>
            {
                new ClienteDto
                {
                    id = 1,
                    estado = true,
                    nombre = "Juan Pérez",
                    identificacion = "12345678",
                    direccion = "Calle 123",
                    telefono = "555123456",
                    edad = 30,
                    genero = "M",
                    cuentas = new List<CuentaDto>()
                },
                new ClienteDto
                {
                    id = 2,
                    estado = true,
                    nombre = "María García",
                    identificacion = "87654321",
                    direccion = "Carrera 456",
                    telefono = "555789123",
                    edad = 25,
                    genero = "F",
                    cuentas = new List<CuentaDto>()
                }
            };

            _clienteServiceMock.Setup(service => service.ObtenerTodosAsync())
                              .ReturnsAsync(clientesEsperados);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var clientes = Assert.IsAssignableFrom<IEnumerable<ClienteDto>>(okResult.Value);
            Assert.Equal(2, clientes.Count());
            _clienteServiceMock.Verify(service => service.ObtenerTodosAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_DeberiaRetornarOkConListaVacia_CuandoNoHayClientes()
        {
            // Arrange
            var clientesVacios = new List<ClienteDto>();
            _clienteServiceMock.Setup(service => service.ObtenerTodosAsync())
                              .ReturnsAsync(clientesVacios);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado);
            var clientes = Assert.IsAssignableFrom<IEnumerable<ClienteDto>>(okResult.Value);
            Assert.Empty(clientes);
            _clienteServiceMock.Verify(service => service.ObtenerTodosAsync(), Times.Once);
        }
    }
}