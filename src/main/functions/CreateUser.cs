using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersFunctionApp.src.domain.service;
using System.Text.Json;
using UsersFunctionApp.src.application.dto;
using UsersFunctionApp.src.domain.exception;

namespace UsersFunctionApp;

public class CreateUser
{
    private readonly ILogger<CreateUser> _logger;
    private readonly IUserService _userService;

    public CreateUser(ILogger<CreateUser> logger, IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    [Function("CreateUser")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = "create-user")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        try
        {
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            if (string.IsNullOrEmpty(requestBody))
            {
                return new BadRequestObjectResult(new { code = 400, message = "Request body cannot be empty", timestamp = DateTime.UtcNow });
            }

            var request = JsonSerializer.Deserialize<UserRequestDTO>(requestBody);
            
            if (request == null)
            {
                return new BadRequestObjectResult(new { code = 400, message = "Invalid request body", timestamp = DateTime.UtcNow });
            }

            var user = _userService.Create(request);
            return new OkObjectResult(user);
        }
        catch (DomainException ex)
        {
            return new BadRequestObjectResult(new { code = 400, message = ex.Message, timestamp = DateTime.UtcNow });
        }
    }
}
