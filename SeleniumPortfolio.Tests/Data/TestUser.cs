namespace SeleniumPortfolio.Tests.Data;

public sealed record TestUser(string Username, string Password);

/// <summary>Public demo credentials published by SauceDemo.</summary>
public static class Users
{
    public const string SharedPassword = "secret_sauce";

    public static TestUser Standard { get; } = new("standard_user", SharedPassword);
    public static TestUser LockedOut { get; } = new("locked_out_user", SharedPassword);
    public static TestUser WrongPassword { get; } = new("standard_user", "wrong-password");
}
