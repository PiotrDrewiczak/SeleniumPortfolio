using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Checkout")]
[AllureEpic("SauceDemo web shop")]
[AllureFeature("Checkout")]
[Category("Checkout")]
public sealed class CheckoutTests : TestBase
{
    [Test, Category("Smoke")]
    [AllureStory("Place an order")]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("A user with a product in the cart enters valid customer details and completes the order.")]
    public void ValidInformation_ShouldCompleteCheckout()
    {
        var checkout = StartCheckoutWithBackpack();

        checkout.EnterCustomerInformation(CustomerInfo.Valid);
        checkout.Continue();
        checkout.Finish();

        Assert.That(checkout.GetCompletionMessage(), Is.EqualTo("Thank you for your order!"));
    }

    [Test]
    [AllureStory("Order overview")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("The order overview shows the item price and a total equal to item total plus tax.")]
    public void Overview_ShouldShowConsistentTotals()
    {
        var checkout = StartCheckoutWithBackpack();

        checkout.EnterCustomerInformation(CustomerInfo.Valid);
        checkout.Continue();

        Assert.Multiple(() =>
        {
            Assert.That(checkout.GetItemTotal(), Is.EqualTo(29.99m), "Backpack price.");
            Assert.That(checkout.GetTotal(), Is.EqualTo(checkout.GetItemTotal() + checkout.GetTax()),
                "Total must equal item total plus tax.");
        });
    }

    private static IEnumerable<TestCaseData> InvalidCustomers()
    {
        yield return new TestCaseData(CustomerInfo.Valid.WithFirstName(""), "First Name is required")
            .SetName("MissingFirstName_ShouldShowValidationError");
        yield return new TestCaseData(CustomerInfo.Valid.WithLastName(""), "Last Name is required")
            .SetName("MissingLastName_ShouldShowValidationError");
        yield return new TestCaseData(CustomerInfo.Valid.WithPostalCode(""), "Postal Code is required")
            .SetName("MissingPostalCode_ShouldShowValidationError");
    }

    [TestCaseSource(nameof(InvalidCustomers))]
    [AllureStory("Checkout form validation")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Leaving first name, last name or postal code empty blocks the order and shows a required-field error.")]
    public void InvalidInformation_ShouldShowValidationError(CustomerInfo customer, string expectedError)
    {
        var checkout = StartCheckoutWithBackpack();

        checkout.EnterCustomerInformation(customer);
        checkout.Continue();

        Assert.That(checkout.GetErrorMessage(), Does.Contain(expectedError));
    }
}
