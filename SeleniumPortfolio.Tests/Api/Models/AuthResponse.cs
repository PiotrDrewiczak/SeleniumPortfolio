using System.Text.Json.Serialization;

namespace SeleniumPortfolio.Tests.Api.Models;

/// <summary>Response of POST /auth. The token is needed for PUT, PATCH and DELETE.</summary>
public sealed record AuthResponse([property: JsonPropertyName("token")] string? Token);
