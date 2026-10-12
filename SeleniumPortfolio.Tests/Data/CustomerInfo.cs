namespace SeleniumPortfolio.Tests.Data;

public sealed record CustomerInfo(string FirstName, string LastName, string PostalCode)
{
    public static CustomerInfo Valid { get; } = new("Piotr", "Tester", "00-001");

    public CustomerInfo WithFirstName(string value) => this with { FirstName = value };
    public CustomerInfo WithLastName(string value) => this with { LastName = value };
    public CustomerInfo WithPostalCode(string value) => this with { PostalCode = value };
}
