using System.Net;
using System.Text.Json;

namespace API_Waylan_Origin.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Permite que la petición siga su curso normal (hacia el Controlador y el Servicio)
                await _next(context);
            }
            catch (Exception ex)
            {
                // ¡La red de seguridad! Si algo explota en cualquier capa, cae aquí
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Mapeamos el tipo de excepción de C# al código de estado HTTP correcto
            context.Response.StatusCode = exception switch
            {
                KeyNotFoundException => (int)HttpStatusCode.NotFound, // 404 - No encontrado
                ArgumentException => (int)HttpStatusCode.BadRequest,   // 400 - Petición incorrecta
                _ => (int)HttpStatusCode.InternalServerError          // 500 - Errores imprevistos del servidor
            };

            // Creamos un objeto JSON estandarizado para el frontend
            var respuesta = new
            {
                statusCode = context.Response.StatusCode,
                message = exception.Message,
                error = exception.GetType().Name
            };

            // Lo convertimos a formato JSON con camelCase (estándar de JavaScript)
            var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            return context.Response.WriteAsync(JsonSerializer.Serialize(respuesta, jsonOptions));
        }
    }
}
