using FluentAssertions;
using UsersFunctionApp.src.domain;
using UsersFunctionApp.src.domain.exception;
using Xunit;

namespace UsersFunctionApp.Tests;

public class UserTests
{
    [Fact]
    public void Should_Create_Valid_User()
    {
        var name = "João Silva";
        var age = 30;
        var cpf = "12345678909";

        var user = User.Create(name, age, cpf);

        user.Id.Should().NotBeEmpty();
        user.Name.Should().Be(name);
        user.Age.Should().Be(age);
        user.Cpf.Document.Should().Be(cpf);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Should_Throw_Exception_For_Empty_Name(string name)
    {
        var act = () => User.Create(name, 30, "12345678909");
        act.Should().Throw<DomainException>()
           .WithMessage("Name is required");
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-10)]
    public void Should_Throw_Exception_For_Negative_Age(int age)
    {
        var act = () => User.Create("João Silva", age, "12345678909");
        act.Should().Throw<DomainException>()
           .WithMessage("Age cannot be negative");
    }

    [Fact]
    public void Should_Accept_Zero_Age()
    {
        var user = User.Create("João Silva", 0, "12345678909");

        user.Age.Should().Be(0);
    }

    [Fact]
    public void Should_Throw_Exception_For_Invalid_Cpf()
    {
        var act = () => User.Create("João Silva", 30, "11111111111");
        act.Should().Throw<DomainException>()
           .WithMessage("Invalid CPF");
    }
}