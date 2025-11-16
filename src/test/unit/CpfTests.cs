using FluentAssertions;
using UsersFunctionApp.src.domain.exception;
using UsersFunctionApp.src.domain.vo;
using Xunit;

namespace UsersFunctionApp.Tests;

public class CpfTests
{
    [Theory]
    [InlineData("12345678909")]
    [InlineData("123.456.789-09")]
    [InlineData("123 456 789 09")]
    public void Should_Create_Valid_Cpf(string document)
    {
        var cpf = new Cpf(document);

        cpf.Document.Should().Be("12345678909");
        cpf.ToString().Should().Be("123.456.789-09");
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Should_Throw_Exception_For_Empty_Cpf(string document)
    {
        var act = () => new Cpf(document);
        act.Should().Throw<DomainException>()
           .WithMessage("CPF cannot be empty");
    }

    [Theory]
    [InlineData("123456789")]
    [InlineData("123456789012")]
    public void Should_Throw_Exception_For_Invalid_Length(string document)
    {
        var act = () => new Cpf(document);
        act.Should().Throw<DomainException>()
           .WithMessage("CPF must contain 11 digits");
    }

    [Theory]
    [InlineData("11111111111")]
    [InlineData("22222222222")]
    [InlineData("00000000000")]
    public void Should_Throw_Exception_For_Repeated_Digits(string document)
    {
        var act = () => new Cpf(document);
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid CPF");
    }

    [Theory]
    [InlineData("12345678901")]
    [InlineData("12345678900")]
    public void Should_Throw_Exception_For_Invalid_Check_Digits(string document)
    {
        var act = () => new Cpf(document);
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid CPF");
    }
}