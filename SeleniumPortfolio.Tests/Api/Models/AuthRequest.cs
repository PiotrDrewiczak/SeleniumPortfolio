using System.Text.Json.Serialization;

namespace SeleniumPortfolio.Tests.Api.Models;

/// <summary>Body of POST /auth. Restful-Booker publishes these demo credentials.</summary>
public sealed record AuthRequest(
    [property: JsonPropertyName("username")] string Username,
    [property: JsonPropertyName("password")] string Password)
{
    public static AuthRequest Admin { get; } = new("admin", "password123");
}
