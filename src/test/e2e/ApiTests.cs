using FluentAssertions;
using System.Net;
using System.Text;
using System.Text.Json;
using UsersFunctionApp.src.application.dto;
using Xunit;

namespace UsersFunctionApp.Tests.E2E;

public class ApiTests : IDisposable
{
    private readonly HttpClient _httpClient;
    private const string BaseUrl = "http://localhost:7071"; // Azure Functions local URL

    public ApiTests()
    {
        _httpClient = new HttpClient { BaseAddress = new Uri(BaseUrl) };
    }

    [Fact]
    [Trait("Category", "E2E")]
    public async Task Should_Create_User_E2E()
    {
        // Arrange
        var request = new UserRequestDTO("João Silva", 30, "12345678909");
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/create-user", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var userResponse = JsonSerializer.Deserialize<UserResponseDTO>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        userResponse.Should().NotBeNull();
        userResponse!.Name.Should().Be("João Silva");
        userResponse.Age.Should().Be(30);
        userResponse.Cpf.Should().Be("123.456.789-09");
        userResponse.Id.Should().NotBeEmpty();
    }

    [Fact]
    [Trait("Category", "E2E")]
    public async Task Should_Return_BadRequest_For_Invalid_Data_E2E()
    {
        // Arrange
        var request = new UserRequestDTO("", -1, "invalid-cpf");
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/create-user", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Category", "E2E")]
    public async Task Should_Return_BadRequest_For_Invalid_CPF_E2E()
    {
        // Arrange
        var request = new UserRequestDTO("João Silva", 30, "11111111111");
        var json = JsonSerializer.Serialize(request);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/create-user", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    [Trait("Category", "E2E")]
    public async Task Should_Handle_Empty_Request_Body_E2E()
    {
        // Arrange
        var content = new StringContent("", Encoding.UTF8, "application/json");

        // Act
        var response = await _httpClient.PostAsync("/api/create-user", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}