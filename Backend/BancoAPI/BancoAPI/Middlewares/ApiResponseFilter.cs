using BancoAPI.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BancoAPI.Middlewares
{
    public class ApiResponseFilter : IActionFilter
    {
        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
            // No procesar si la respuesta ya es un ApiResponse
            if (context.Result is ObjectResult objectResult)
            {
                var type = objectResult.Value?.GetType();
                if (type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(ApiResponse<>))
                {
                    return;
                }

                // Envolver el resultado en ApiResponse
                objectResult.Value = ApiResponse<object>.CreateSuccess(objectResult.Value);
            }
            // Para respuestas no encontradas
            else if (context.Result is NotFoundResult)
            {
                context.Result = new NotFoundObjectResult(
                    ApiResponse<object>.Fail("Recurso no encontrado"));
            }
            // Para respuestas de error de solicitud
            else if (context.Result is BadRequestResult)
            {
                context.Result = new BadRequestObjectResult(
                    ApiResponse<object>.Fail("Solicitud inválida"));
            }
        }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
          
        }
    }
}
