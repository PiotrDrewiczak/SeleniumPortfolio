using System.Text.Json.Serialization;

namespace SeleniumPortfolio.Tests.Api.Models;

/// <summary>Response of POST /booking: the new id plus the booking as stored by the server.</summary>
public sealed record CreateBookingResponse(
    [property: JsonPropertyName("bookingid")] int BookingId,
    [property: JsonPropertyName("booking")] Booking Booking);
