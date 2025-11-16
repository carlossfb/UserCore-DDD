using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using UsersFunctionApp.src.application.service;
using UsersFunctionApp.src.domain.service;
using UsersFunctionApp.src.infraestructure;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        builder.UseMiddleware<GlobalExceptionHandler>();
    })
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();
        services.AddSingleton<IUserService, UserServiceImpl>();
    })
    .Build();

host.Run();
