using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class CheckoutPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    public void EnterCustomerInformation(CustomerInfo customer) =>
        EnterCustomerInformation(customer.FirstName, customer.LastName, customer.PostalCode);

    public void EnterCustomerInformation(string firstName, string lastName, string postalCode)
    {
        Type(By.Id("first-name"), firstName);
        Type(By.Id("last-name"), lastName);
        Type(By.Id("postal-code"), postalCode);
    }

    public void Continue() => Click(By.Id("continue"));
    public void Finish() => Click(By.Id("finish"));

    public string GetErrorMessage() => TextOf(By.CssSelector("[data-test='error']"));
    public string GetCompletionMessage() => TextOf(By.CssSelector("[data-test='complete-header']"));
}
