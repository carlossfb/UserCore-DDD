using System.Net;
using System.Text.Json;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Azure.Functions.Worker.Http;
using UsersFunctionApp.src.domain.exception;
using Microsoft.Extensions.Logging;

namespace UsersFunctionApp.src.infraestructure
{
    public class GlobalExceptionHandler : IFunctionsWorkerMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (DomainException ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, HttpStatusCode.InternalServerError, "Internal Server Error");
                _logger.LogError(ex.ToString());
            }
        }

        private static async Task HandleExceptionAsync(FunctionContext context, HttpStatusCode statusCode, string message)
        {
            var logger = context.GetLogger<GlobalExceptionHandler>();
            logger.LogError("Error captured by middleware: {Message}", message);

            // Obtém o HttpRequestData (se existir)
            var request = await context.GetHttpRequestDataAsync();
            if (request == null)
                return;

            var response = request.CreateResponse(statusCode);
            response.Headers.Add("Content-Type", "application/json");

            var error = new
            {
                code = (int)statusCode,
                message,
                timestamp = DateTime.UtcNow
            };

            await response.WriteStringAsync(JsonSerializer.Serialize(error));
            context.GetInvocationResult().Value = response;
        }
    }
}
