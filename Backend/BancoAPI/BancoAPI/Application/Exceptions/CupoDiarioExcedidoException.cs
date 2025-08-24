using BancoAPI.Application.DTOs;

namespace BancoAPI.Application.Exceptions
{
    public class CupoDiarioExcedidoException : Exception
    {
        public CupoDiarioExcedidoException()
            : base("Cupo diario excedido.")
        {
        }

        public CupoDiarioExcedidoException(string message)
            : base(message)
        {
        }

        public CupoDiarioExcedidoException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ApiResponse<object> ToApiResponse()
        {
            return ApiResponse<object>.Fail(Message);
        }
    }
}
