using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Api.Models;

namespace SeleniumPortfolio.Tests.Api.Tests;

[AllureNUnit]
[AllureSuite("Api")]
[AllureEpic("Restful-Booker API")]
[AllureFeature("Bookings")]
[Category("Api")]
public sealed class BookingTests : ApiTestBase
{
    [Test]
    [AllureStory("Create booking")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Creating a booking returns a new id and echoes back the data that was sent.")]
    public void CreateBooking_ShouldReturnIdAndStoredBooking()
    {
        var created = CreateBooking(Booking.Sample);

        Assert.Multiple(() =>
        {
            Assert.That(created.BookingId, Is.GreaterThan(0), "The server should assign an id.");
            Assert.That(created.Booking, Is.EqualTo(Booking.Sample), "The stored booking should match what was sent.");
        });
    }

    [Test]
    [AllureStory("Get booking")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("A created booking can be fetched by its id and contains the data that was sent.")]
    public void GetBooking_ShouldReturnCreatedBooking()
    {
        var created = CreateBooking(Booking.Sample);

        var response = Api.GetBooking(created.BookingId);

        Assert.Multiple(() =>
        {
            Assert.That((int)response.StatusCode, Is.EqualTo(200), response.Content);
            Assert.That(response.Data, Is.EqualTo(Booking.Sample));
        });
    }
}
