using Allure.NUnit.Attributes;
using RestSharp;
using SeleniumPortfolio.Tests.Api.Models;

namespace SeleniumPortfolio.Tests.Api.Clients;

/// <summary>
/// Thin wrapper over the Restful-Booker endpoints: the API equivalent of a Page Object.
/// Every method returns the full RestResponse so tests decide what to assert (status code, body, headers).
/// </summary>
public sealed class BookingApiClient(RestClient client)
{
    [AllureStep("POST /auth")]
    public RestResponse<AuthResponse> Authenticate(AuthRequest credentials)
    {
        var request = new RestRequest("auth", Method.Post).AddJsonBody(credentials);
        return client.Execute<AuthResponse>(request);
    }

    [AllureStep("POST /booking")]
    public RestResponse<CreateBookingResponse> CreateBooking(Booking booking)
    {
        var request = new RestRequest("booking", Method.Post).AddJsonBody(booking);
        return client.Execute<CreateBookingResponse>(request);
    }

    [AllureStep("GET /booking/{bookingId}")]
    public RestResponse<Booking> GetBooking(int bookingId)
    {
        var request = new RestRequest("booking/{id}").AddUrlSegment("id", bookingId);
        return client.Execute<Booking>(request);
    }

    [AllureStep("PUT /booking/{bookingId}")]
    public RestResponse<Booking> UpdateBooking(int bookingId, Booking booking, string token)
    {
        var request = new RestRequest("booking/{id}", Method.Put)
            .AddUrlSegment("id", bookingId)
            .AddHeader("Cookie", $"token={token}")
            .AddJsonBody(booking);
        return client.Execute<Booking>(request);
    }

    [AllureStep("DELETE /booking/{bookingId}")]
    public RestResponse DeleteBooking(int bookingId, string token)
    {
        var request = new RestRequest("booking/{id}", Method.Delete)
            .AddUrlSegment("id", bookingId)
            .AddHeader("Cookie", $"token={token}");
        return client.Execute(request);
    }
}
