using BancoAPI.Application.DTOs;
using BancoAPI.Controllers;
using BancoAPI.Domain.Enums;
using BancoAPI.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BancoAPI.Tests
{
    public class CuentasControllerTests
    {
        private readonly Mock<ICuentaService> _cuentaServiceMock;
        private readonly CuentasController _controller;

        public CuentasControllerTests()
        {
            _cuentaServiceMock = new Mock<ICuentaService>();
            _controller = new CuentasController(_cuentaServiceMock.Object);
        }

        [Fact]
        public async Task Get_DeberiaRetornarOkConListaDeCuentas()
        {
            // Arrange
            var cuentasEsperadas = new List<CuentaDto>
            {
                new CuentaDto
                {
                    id = 1,
                    numeroCuenta = "001234567",
                    tipoCuenta = TipoCuenta.Ahorro,
                    saldoInicial = 1000m,
                    estado = true,
                    cliente = new CuentaDto.ClienteResumido
                    {
                        id = 1,
                        nombre = "Juan Pérez"
                    }
                },
                new CuentaDto
                {
                    id = 2,
                    numeroCuenta = "001234568",
                    tipoCuenta = TipoCuenta.Corriente,
                    saldoInicial = 2500m,
                    estado = true,
                    cliente = new CuentaDto.ClienteResumido
                    {
                        id = 2,
                        nombre = "María García"
                    }
                }
            };

            _cuentaServiceMock.Setup(service => service.ObtenerTodasAsync())
                             .ReturnsAsync(cuentasEsperadas);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var cuentas = Assert.IsAssignableFrom<IEnumerable<CuentaDto>>(okResult.Value);
            Assert.Equal(2, cuentas.Count());
            _cuentaServiceMock.Verify(service => service.ObtenerTodasAsync(), Times.Once);
        }

        [Fact]
        public async Task Get_DeberiaRetornarOkConListaVacia_CuandoNoHayCuentas()
        {
            // Arrange
            var cuentasVacias = new List<CuentaDto>();
            _cuentaServiceMock.Setup(service => service.ObtenerTodasAsync())
                             .ReturnsAsync(cuentasVacias);

            // Act
            var resultado = await _controller.Get();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(resultado.Result);
            var cuentas = Assert.IsAssignableFrom<IEnumerable<CuentaDto>>(okResult.Value);
            Assert.Empty(cuentas);
            _cuentaServiceMock.Verify(service => service.ObtenerTodasAsync(), Times.Once);
        }
    }
}