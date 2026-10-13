using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class ProductDetailsPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By Name = By.CssSelector("[data-test='inventory-item-name']");
    private static readonly By Price = By.CssSelector("[data-test='inventory-item-price']");

    public string GetName() => TextOf(Name);

    public decimal GetPrice() =>
        decimal.Parse(TextOf(Price).Replace("$", "").Trim(), System.Globalization.CultureInfo.InvariantCulture);

    [AllureStep("Go back to the product list")]
    public void BackToProducts() => Click(By.CssSelector("[data-test='back-to-products']"));
}
