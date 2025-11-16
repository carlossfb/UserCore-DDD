using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using UsersFunctionApp.src.application.dto;
using UsersFunctionApp.src.application.service;
using UsersFunctionApp.src.domain.exception;
using UsersFunctionApp.src.domain.service;
using Xunit;

namespace UsersFunctionApp.Tests.Integration;

public class ServiceIntegrationTests
{
    private readonly IHost _host;
    private readonly IUserService _userService;

    public ServiceIntegrationTests()
    {
        _host = new HostBuilder()
            .ConfigureServices(services =>
            {
                services.AddScoped<IUserService, UserServiceImpl>();
                services.AddLogging();
            })
            .Build();

        _userService = _host.Services.GetRequiredService<IUserService>();
    }

    [Fact]
    public void Should_Create_User_With_Full_Pipeline_Integration()
    {
        // Arrange - Testa toda a pipeline: DTO → Service → Domain → DTO
        var request = new UserRequestDTO("João Silva", 30, "12345678909");

        // Act
        var response = _userService.Create(request);

        // Assert
        response.Should().NotBeNull();
        response.Name.Should().Be("João Silva");
        response.Age.Should().Be(30);
        response.Cpf.Should().Be("123.456.789-09");
        response.Id.Should().NotBeEmpty();
    }

    [Fact]
    public void Should_Validate_Business_Rules_Integration()
    {
        // Arrange - Testa validação através de toda a pipeline
        var request = new UserRequestDTO("", -1, "11111111111");

        // Act & Assert
        var act = () => _userService.Create(request);
        act.Should().Throw<DomainException>()
           .WithMessage("Name is required");
    }

    [Fact]
    public void Should_Handle_CPF_Validation_Integration()
    {
        // Arrange - Testa validação de CPF através da pipeline
        var request = new UserRequestDTO("João Silva", 30, "123456789");

        // Act & Assert
        var act = () => _userService.Create(request);
        act.Should().Throw<DomainException>()
           .WithMessage("CPF must contain 11 digits");
    }

    [Fact]
    public void Should_Format_CPF_Correctly_Integration()
    {
        // Arrange - Testa formatação através da pipeline
        var request = new UserRequestDTO("Maria Silva", 25, "98765432100");

        // Act
        var response = _userService.Create(request);

        // Assert
        response.Cpf.Should().Be("987.654.321-00");
    }
}