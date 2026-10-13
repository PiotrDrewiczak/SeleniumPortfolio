using NUnit.Framework;
using RestSharp;
using SeleniumPortfolio.Tests.Api.Clients;
using SeleniumPortfolio.Tests.Api.Models;
using SeleniumPortfolio.Tests.Configuration;

namespace SeleniumPortfolio.Tests.Api;

/// <summary>
/// Base class for API tests: the counterpart of TestBase, but with an HTTP client instead of a browser.
/// Bookings created through <see cref="CreateBooking"/> are deleted in TearDown, so tests leave no data behind
/// on the shared public server.
/// </summary>
public abstract class ApiTestBase
{
    private RestClient _restClient = null!;
    private readonly List<int> _createdBookingIds = [];

    protected TestSettings Settings { get; private set; } = null!;
    protected BookingApiClient Api { get; private set; } = null!;

    [SetUp]
    public void SetUpApi()
    {
        Settings = TestSettings.Load();
        _restClient = new RestClient(Settings.ApiBaseUrl);
        // Restful-Booker answers "418 I'm a Teapot" to RestSharp's default (very long) Accept header.
        _restClient.AddDefaultHeader("Accept", "application/json");
        Api = new BookingApiClient(_restClient);
        _createdBookingIds.Clear();
    }

    [TearDown]
    public void TearDownApi()
    {
        try
        {
            if (_createdBookingIds.Count > 0)
            {
                var token = GetToken();
                foreach (var id in _createdBookingIds)
                    Api.DeleteBooking(id, token);
            }
        }
        finally
        {
            _restClient.Dispose();
        }
    }

    /// <summary>Logs in with the demo admin account and returns the token; fails the test if none is issued.</summary>
    protected string GetToken()
    {
        var response = Api.Authenticate(AuthRequest.Admin);
        Assert.That(response.Data?.Token, Is.Not.Null.And.Not.Empty, "POST /auth should return a token.");
        return response.Data!.Token!;
    }

    /// <summary>Creates a booking, remembers its id for cleanup and returns the parsed response.</summary>
    protected CreateBookingResponse CreateBooking(Booking booking)
    {
        var response = Api.CreateBooking(booking);
        Assert.That(response.IsSuccessful, Is.True, $"POST /booking failed: {(int)response.StatusCode} {response.Content}");
        _createdBookingIds.Add(response.Data!.BookingId);
        return response.Data;
    }
}
