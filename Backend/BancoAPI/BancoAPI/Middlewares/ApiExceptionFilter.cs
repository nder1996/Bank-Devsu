using BancoAPI.Application.DTOs;
using BancoAPI.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BancoAPI.Middlewares
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;

        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError(context.Exception, context.Exception.Message);

            ApiResponse<object> response;
            int statusCode;

            switch (context.Exception)
            {
                case SaldoNoDisponibleException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.Fail("Saldo no disponible");
                    break;

                case CupoDiarioExcedidoException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.Fail("Cupo diario Excedido");
                    break;

                case KeyNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    response = ApiResponse<object>.Fail("Recurso no encontrado");
                    break;

                case ArgumentException:
                    statusCode = StatusCodes.Status400BadRequest;
                    response = ApiResponse<object>.Fail(context.Exception.Message);
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    // En desarrollo, mostrar más detalles del error
                    var isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
                    var errorMessage = isDevelopment 
                        ? $"Error: {context.Exception.Message}. Inner: {context.Exception.InnerException?.Message}"
                        : "Ha ocurrido un error interno. Por favor, inténtelo de nuevo más tarde.";
                    response = ApiResponse<object>.Fail(errorMessage);
                    break;
            }

            context.Result = new ObjectResult(response)
            {
                StatusCode = statusCode
            };

            context.ExceptionHandled = true;
        }
    }
}
