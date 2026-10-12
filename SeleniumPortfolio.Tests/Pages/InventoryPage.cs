using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class InventoryPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By PageTitle = By.CssSelector("[data-test='title']");
    private static readonly By ProductItems = By.CssSelector(".inventory_item");
    private static readonly By SortDropdown = By.CssSelector("[data-test='product-sort-container']");
    private static readonly By FirstProductPrice = By.CssSelector(".inventory_item_price");
    private static readonly By ProductNames = By.CssSelector(".inventory_item_name");
    private static readonly By ProductPrices = By.CssSelector(".inventory_item_price");
    private static readonly By CartBadge = By.CssSelector(".shopping_cart_badge");

    public bool IsLoaded => IsVisible(PageTitle) && TextOf(PageTitle) == "Products";
    [AllureStep("Wait for the Products page to load")]
    public void WaitUntilLoaded()
    {
        WaitUntilVisible(PageTitle);
        var title = TextOf(PageTitle);
        if (title != "Products")
            throw new InvalidOperationException($"Expected page title 'Products' but was '{title}'.");
    }
    public int ProductCount => Driver.FindElements(ProductItems).Count;

    [AllureStep("Sort products by price, low to high")]
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

    [AllureStep("Sort products using option {0}")]
    public void SortBy(string optionValue) =>
        new SelectElement(Find(SortDropdown)).SelectByValue(optionValue);

    public List<string> GetProductNames() =>
        Driver.FindElements(ProductNames).Select(e => e.Text).ToList();

    public List<decimal> GetProductPrices() =>
        Driver.FindElements(ProductPrices)
            .Select(e => decimal.Parse(e.Text.Replace("$", "").Trim(), System.Globalization.CultureInfo.InvariantCulture))
            .ToList();

    /// <summary>Number shown on the cart badge; 0 when the badge is not displayed.</summary>
    public int CartBadgeCount
    {
        get
        {
            var badges = Driver.FindElements(CartBadge);
            return badges.Count == 0 ? 0 : int.Parse(badges[0].Text);
        }
    }

    [AllureStep("Wait until the cart badge shows {0}")]
    public void WaitForCartBadge(int expected)
    {
        if (expected == 0)
            WaitForCount(CartBadge, 0);
        else
            Wait.Until(_ => CartBadgeCount == expected);
    }

    /// <summary>Adds a product by its id slug, e.g. "sauce-labs-backpack".</summary>
    [AllureStep("Add {0} to the cart")]
    public void AddToCart(string productSlug) => Click(By.Id($"add-to-cart-{productSlug}"));
    [AllureStep("Remove {0} from the cart")]
    public void RemoveFromCart(string productSlug) => Click(By.Id($"remove-{productSlug}"));

    public void AddBackpackToCart() => AddToCart("sauce-labs-backpack");
    [AllureStep("Open the cart")]
    public void OpenCart() => Click(By.CssSelector(".shopping_cart_link"));

    [AllureStep("Log out from the burger menu")]
    public void Logout()
    {
        Click(By.Id("react-burger-menu-btn"));
        Click(By.Id("logout_sidebar_link"));
    }
}
