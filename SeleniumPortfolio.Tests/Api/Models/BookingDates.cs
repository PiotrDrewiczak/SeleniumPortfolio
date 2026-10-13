using System.Text.Json.Serialization;

namespace SeleniumPortfolio.Tests.Api.Models;

/// <summary>Check-in and check-out dates; the API expects the format yyyy-MM-dd.</summary>
public sealed record BookingDates(
    [property: JsonPropertyName("checkin")] string CheckIn,
    [property: JsonPropertyName("checkout")] string CheckOut);
