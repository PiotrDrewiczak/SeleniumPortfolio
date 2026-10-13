using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class CheckoutPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void EnterCustomerInformation(CustomerInfo customer) =>
        EnterCustomerInformation(customer.FirstName, customer.LastName, customer.PostalCode);

    [AllureStep("Enter customer information: {firstName} {lastName}, postal code {postalCode}")]
    public void EnterCustomerInformation(string firstName, string lastName, string postalCode)
    {
        Type(By.Id("first-name"), firstName);
        Type(By.Id("last-name"), lastName);
        Type(By.Id("postal-code"), postalCode);
    }

    [AllureStep("Continue to the order overview")]
    public void Continue() => Click(By.Id("continue"));

    [AllureStep("Cancel checkout")]
    public void Cancel() => Click(By.Id("cancel"));

    public List<string> GetOverviewItemNames() =>
        Driver.FindElements(By.CssSelector(".cart_item [data-test='inventory-item-name']")).Select(e => e.Text).ToList();

    [AllureStep("Finish the order")]
    public void Finish() => Click(By.Id("finish"));

    public string GetErrorMessage() => TextOf(By.CssSelector("[data-test='error']"));
    public decimal GetItemTotal() => AmountOf("subtotal-label");
    public decimal GetTax() => AmountOf("tax-label");
    public decimal GetTotal() => AmountOf("total-label");

    private decimal AmountOf(string dataTest)
    {
        var text = TextOf(By.CssSelector($"[data-test='{dataTest}']"));
        var amount = System.Text.RegularExpressions.Regex.Match(text, @"\d+(\.\d+)?").Value;
        return decimal.Parse(amount, System.Globalization.CultureInfo.InvariantCulture);
    }

    public string GetCompletionMessage() => TextOf(By.CssSelector("[data-test='complete-header']"));
}
