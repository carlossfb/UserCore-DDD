using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.DependencyInjection;
using UsersFunctionApp.src.application.service;
using UsersFunctionApp.src.domain.service;
using UsersFunctionApp.src.infraestructure;

var host = new HostBuilder()
    .ConfigureFunctionsWebApplication(builder =>
    {
        // Aqui registramos o middleware global (ex: para capturar DomainException)
        builder.UseMiddleware<GlobalExceptionHandler>();
    })
    .ConfigureServices(services =>
    {
        // Aqui registramos os serviços de domínio e aplicação
        services.AddSingleton<IUserService, UserServiceImpl>();
    })
    .Build();

host.Run();
