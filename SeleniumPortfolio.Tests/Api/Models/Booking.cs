using System.Text.Json.Serialization;

namespace SeleniumPortfolio.Tests.Api.Models;

/// <summary>
/// A booking as sent to and returned by Restful-Booker.
/// [JsonPropertyName] maps our C# names (PascalCase) to the API's JSON names (all lower case).
/// </summary>
public sealed record Booking(
    [property: JsonPropertyName("firstname")] string FirstName,
    [property: JsonPropertyName("lastname")] string LastName,
    [property: JsonPropertyName("totalprice")] int TotalPrice,
    [property: JsonPropertyName("depositpaid")] bool DepositPaid,
    [property: JsonPropertyName("bookingdates")] BookingDates BookingDates,
    [property: JsonPropertyName("additionalneeds")] string? AdditionalNeeds = null)
{
    public static Booking Sample { get; } = new(
        "Piotr", "Tester", 150, true, new BookingDates("2026-11-01", "2026-11-05"), "Breakfast");
}
