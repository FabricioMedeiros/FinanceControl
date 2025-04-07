using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FinanceControl.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601) 
            {
                await HandleDuplicateKeyExceptionAsync(context, sqlEx);
            }
            catch (Exception ex)
            {
                await HandleGenericExceptionAsync(context, ex);
            }
        }

        private Task HandleDuplicateKeyExceptionAsync(HttpContext context, SqlException sqlEx)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            return context.Response.WriteAsJsonAsync(new
            {
                success = false,
                errors = new List<string>
                {
                    "Já existe um registro com a mesma chave única especificada. " + sqlEx.Message
                }
            });
        }

        private Task HandleGenericExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            return context.Response.WriteAsJsonAsync(new
            {
                success = false,
                errors = new List<string>
                {
                    "Ocorreu um erro inesperado. Erro: " + ex.GetBaseException().Message
                }
            });
        }
    }
}