using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class InventoryPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By PageTitle = By.CssSelector("[data-test='title']");
    private static readonly By ProductItems = By.CssSelector(".inventory_item");
    private static readonly By SortDropdown = By.CssSelector("[data-test='product-sort-container']");
    private static readonly By FirstProductPrice = By.CssSelector(".inventory_item_price");

    public bool IsLoaded => IsVisible(PageTitle) && TextOf(PageTitle) == "Products";
    public void WaitUntilLoaded()
    {
        WaitUntilVisible(PageTitle);
        var title = TextOf(PageTitle);
        if (title != "Products")
            throw new InvalidOperationException($"Expected page title 'Products' but was '{title}'.");
    }
    public int ProductCount => Driver.FindElements(ProductItems).Count;

    public void SortByPriceLowToHigh()
    {
        var dropdown = Find(SortDropdown);
        new SelectElement(dropdown).SelectByValue("lohi");
    }

    public decimal GetFirstProductPrice()
    {
        var text = TextOf(FirstProductPrice).Replace("$", "").Trim();
        return decimal.Parse(text, System.Globalization.CultureInfo.InvariantCulture);
    }

    public void AddBackpackToCart() => Click(By.Id("add-to-cart-sauce-labs-backpack"));
    public void OpenCart() => Click(By.CssSelector(".shopping_cart_link"));
}
