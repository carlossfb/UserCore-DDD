using FluentAssertions;
using UsersFunctionApp.src.application.dto;
using UsersFunctionApp.src.domain;
using Xunit;

namespace UsersFunctionApp.Tests;

public class UserMapperTests
{
    [Fact]
    public void Should_Map_User_To_UserResponseDTO()
    {
        var user = User.Create("João Silva", 30, "12345678909");

        var response = UserMapper.ToUserResponseDTO(user);

        response.Id.Should().Be(user.Id);
        response.Name.Should().Be("João Silva");
        response.Age.Should().Be(30);
        response.Cpf.Should().Be("123.456.789-09");
    }
}