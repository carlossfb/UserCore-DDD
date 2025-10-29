using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UsersFunctionApp.src.domain.service;
using System.Text.Json;
using UsersFunctionApp.src.application.dto;

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
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post",Route ="create-user")] HttpRequest req)
    {
        _logger.LogInformation("C# HTTP trigger function processed a request.");
        
        var request = await JsonSerializer.DeserializeAsync<UserRequestDTO>(req.Body);

        if (request == null)
            return new BadRequestObjectResult("Invalid request body");

        var user = _userService.Create(request.Name, request.Age,request.Cpf);

        return new OkObjectResult(user);
    }
}
