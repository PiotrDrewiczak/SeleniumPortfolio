using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumPortfolio.Tests.Configuration;
using SeleniumPortfolio.Tests.Data;
using SeleniumPortfolio.Tests.Pages;

namespace SeleniumPortfolio.Tests.Core;

public abstract class TestBase
{
    protected IWebDriver Driver { get; private set; } = null!;
    protected WebDriverWait Wait { get; private set; } = null!;
    protected TestSettings Settings { get; private set; } = null!;

    protected LoginPage LoginPage => new(Driver, Wait);
    protected InventoryPage InventoryPage => new(Driver, Wait);
    protected CartPage CartPage => new(Driver, Wait);
    protected CheckoutPage CheckoutPage => new(Driver, Wait);

    [SetUp]
    public void SetUp()
    {
        Settings = TestSettings.Load();
        Driver = DriverFactory.Create(Settings);
        try
        {
            Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(Settings.WaitTimeoutSeconds));
            Wait.IgnoreExceptionTypes(typeof(StaleElementReferenceException), typeof(ElementClickInterceptedException));
        }
        catch
        {
            Driver.Quit();
            throw;
        }
        TestContext.Out.WriteLine($"Environment={Settings.Environment} Browser={Settings.Browser} Headless={Settings.Headless} BaseUrl={Settings.BaseUrl}");
    }

    [TearDown]
    public void TearDown()
    {
        try
        {
            if (Driver is not null && TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
                SaveFailureScreenshot();
        }
        finally
        {
            Driver?.Quit();
            Driver?.Dispose();
        }
    }

    /// <summary>Opens the login page and signs in; returns the inventory page.</summary>
    protected InventoryPage LoginAs(TestUser user)
    {
        LoginPage.Open(Settings.BaseUrl);
        LoginPage.LoginAs(user.Username, user.Password);
        return InventoryPage;
    }

    /// <summary>Signs in as the standard user, adds the backpack and proceeds to checkout step one.</summary>
    protected CheckoutPage StartCheckoutWithBackpack()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddBackpackToCart();
        inventory.OpenCart();
        CartPage.Checkout();
        return CheckoutPage;
    }

    private void SaveFailureScreenshot()
    {
        if (Driver is not ITakesScreenshot screenshotDriver)
            return;

        var directory = Path.Combine(TestContext.CurrentContext.WorkDirectory, "Artifacts", "Screenshots");
        Directory.CreateDirectory(directory);

        var safeName = string.Concat(TestContext.CurrentContext.Test.Name.Select(c =>
            Path.GetInvalidFileNameChars().Contains(c) ? '_' : c));

        var path = Path.Combine(directory, $"{safeName}_{DateTime.UtcNow:yyyyMMdd_HHmmss}.png");
        screenshotDriver.GetScreenshot().SaveAsFile(path);
        TestContext.AddTestAttachment(path, "Failure screenshot");
        TestContext.Out.WriteLine($"Screenshot saved: {path}");
    }
}
