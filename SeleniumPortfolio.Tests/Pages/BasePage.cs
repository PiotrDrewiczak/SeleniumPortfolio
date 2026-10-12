using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public abstract class BasePage(IWebDriver driver, WebDriverWait wait)
{
    protected IWebDriver Driver { get; } = driver;
    protected WebDriverWait Wait { get; } = wait;

    protected IWebElement Find(By locator) =>
        Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            return element.Displayed && element.Enabled ? element : null;
        })!;

    // Re-locates the element on every attempt so a re-rendered DOM cannot cause a stale reference.
    protected void Click(By locator) =>
        Wait.Until(d =>
        {
            var element = d.FindElement(locator);
            if (!element.Displayed || !element.Enabled)
                return false;
            element.Click();
            return true;
        });

    /// <summary>Waits until exactly <paramref name="expected"/> elements match the locator.</summary>
    protected void WaitForCount(By locator, int expected) =>
        Wait.Until(d => d.FindElements(locator).Count == expected);

    protected void Type(By locator, string value)
    {
        var element = Find(locator);
        element.Clear();
        element.SendKeys(value);
    }

    protected string TextOf(By locator) => Find(locator).Text;

    /// <summary>Waits until the element is visible; the failure message names the locator.</summary>
    protected void WaitUntilVisible(By locator)
    {
        try
        {
            Wait.Until(d => d.FindElements(locator).Any(e => e.Displayed));
        }
        catch (WebDriverTimeoutException ex)
        {
            throw new WebDriverTimeoutException(
                $"Element {locator} was not visible within {Wait.Timeout.TotalSeconds}s. Current URL: {Driver.Url}", ex);
        }
    }
    /// <summary>Non-throwing check. Pass a short timeout when asserting that something is NOT shown.</summary>
    protected bool IsVisible(By locator, TimeSpan? timeout = null)
    {
        var shortWait = new WebDriverWait(Driver, timeout ?? Wait.Timeout);
        try
        {
            return shortWait.Until(d => d.FindElements(locator).Any(e => e.Displayed));
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}
