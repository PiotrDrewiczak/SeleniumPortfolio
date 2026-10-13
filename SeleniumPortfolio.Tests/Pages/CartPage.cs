using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class CartPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By CartItems = By.CssSelector(".cart_item");
    private static readonly By ItemNames = By.CssSelector(".cart_item [data-test='inventory-item-name']");
    private static readonly By BackpackName = By.XPath("//div[@class='inventory_item_name' and text()='Sauce Labs Backpack']");

    public int ItemCount => Driver.FindElements(CartItems).Count;

    [AllureStep("Wait until the cart has {expected} item(s)")]
    public void WaitForItemCount(int expected) => WaitForCount(CartItems, expected);

    [AllureStep("Wait until the cart is empty")]
    public bool WaitUntilEmpty()
    {
        WaitForCount(CartItems, 0);
        return true;
    }
    public bool ContainsBackpack => IsVisible(BackpackName);

    [AllureStep("Remove the backpack from the cart")]
    public void RemoveBackpack() => Click(By.Id("remove-sauce-labs-backpack"));

    public List<string> GetItemNames() =>
        Driver.FindElements(ItemNames).Select(e => e.Text).ToList();

    [AllureStep("Continue shopping")]
    public void ContinueShopping() => Click(By.Id("continue-shopping"));

    [AllureStep("Proceed to checkout")]
    public void Checkout() => Click(By.Id("checkout"));
}
