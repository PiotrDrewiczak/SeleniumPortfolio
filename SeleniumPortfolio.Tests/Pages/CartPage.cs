using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class CartPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By CartItems = By.CssSelector(".cart_item");
    private static readonly By BackpackName = By.XPath("//div[@class='inventory_item_name' and text()='Sauce Labs Backpack']");

    public int ItemCount => Driver.FindElements(CartItems).Count;

    public bool WaitUntilEmpty()
    {
        WaitForCount(CartItems, 0);
        return true;
    }
    public bool ContainsBackpack => IsVisible(BackpackName);

    public void RemoveBackpack() => Click(By.Id("remove-sauce-labs-backpack"));
    public void Checkout() => Click(By.Id("checkout"));
}
