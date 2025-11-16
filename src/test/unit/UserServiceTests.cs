using FluentAssertions;
using UsersFunctionApp.src.application.dto;
using UsersFunctionApp.src.application.service;
using UsersFunctionApp.src.domain.exception;
using Xunit;

namespace UsersFunctionApp.Tests;

public class UserServiceTests
{
    private readonly UserServiceImpl _userService = new();

    [Fact]
    public void Should_Create_User_Successfully()
    {
        var request = new UserRequestDTO("João Silva", 30, "12345678909");

        var response = _userService.Create(request);

        response.Id.Should().NotBeEmpty();
        response.Name.Should().Be("João Silva");
        response.Age.Should().Be(30);
        response.Cpf.Should().Be("123.456.789-09");
    }

    [Fact]
    public void Should_Throw_Exception_For_Invalid_Name()
    {
        var request = new UserRequestDTO("", 30, "12345678909");

        var act = () => _userService.Create(request);
        act.Should().Throw<DomainException>()
           .WithMessage("Name is required");
    }

    [Fact]
    public void Should_Throw_Exception_For_Negative_Age()
    {
        var request = new UserRequestDTO("João Silva", -1, "12345678909");

        var act = () => _userService.Create(request);
        act.Should().Throw<DomainException>()
           .WithMessage("Age cannot be negative");
    }

    [Fact]
    public void Should_Throw_Exception_For_Invalid_Cpf()
    {
        var request = new UserRequestDTO("João Silva", 30, "11111111111");

        var act = () => _userService.Create(request);
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid CPF");
    }
}