using BancoAPI.Application.DTOs;

namespace BancoAPI.Application.Exceptions
{
    public class SaldoNoDisponibleException : Exception
    {
        public SaldoNoDisponibleException()
           : base("Saldo no disponible.")
        {
        }

        public SaldoNoDisponibleException(string message)
            : base(message)
        {
        }

        public SaldoNoDisponibleException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public ApiResponse<object> ToApiResponse()
        {
            return ApiResponse<object>.Fail(Message);
        }

    }

}
