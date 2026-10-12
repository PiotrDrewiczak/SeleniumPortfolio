using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[Category("Checkout")]
public sealed class CheckoutTests : TestBase
{
    [Test, Category("Smoke")]
    public void ValidInformation_ShouldCompleteCheckout()
    {
        var checkout = StartCheckoutWithBackpack();

        checkout.EnterCustomerInformation(CustomerInfo.Valid);
        checkout.Continue();
        checkout.Finish();

        Assert.That(checkout.GetCompletionMessage(), Is.EqualTo("Thank you for your order!"));
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
    public void InvalidInformation_ShouldShowValidationError(CustomerInfo customer, string expectedError)
    {
        var checkout = StartCheckoutWithBackpack();

        checkout.EnterCustomerInformation(customer);
        checkout.Continue();

        Assert.That(checkout.GetErrorMessage(), Does.Contain(expectedError));
    }
}
