using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using SeleniumPortfolio.Tests.Configuration;

namespace SeleniumPortfolio.Tests.Core;

public static class DriverFactory
{
    public static IWebDriver Create(TestSettings settings)
    {
        IWebDriver driver = settings.Browser.Trim().ToLowerInvariant() switch
        {
            "chrome" => CreateChrome(settings.Headless),
            "firefox" => CreateFirefox(settings.Headless),
            var browser => throw new ArgumentException(
                $"Unsupported browser '{browser}'. Supported values: chrome, firefox.")
        };

        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        driver.Manage().Window.Size = new System.Drawing.Size(1440, 1000);
        return driver;
    }

    private static IWebDriver CreateChrome(bool headless)
    {
        var options = new ChromeOptions();
        if (headless)
        {
            options.AddArgument("--headless=new");
            options.AddArgument("--window-size=1440,1000");
        }

        options.AddArgument("--disable-notifications");
        options.AddArgument("--disable-search-engine-choice-screen");
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_leak_detection", false);
        if (Environment.GetEnvironmentVariable("CI") == "true")
        {
            options.AddArgument("--no-sandbox");
            options.AddArgument("--disable-dev-shm-usage");
        }
        return new ChromeDriver(options);
    }

    private static IWebDriver CreateFirefox(bool headless)
    {
        var options = new FirefoxOptions();
        if (headless)
            options.AddArgument("-headless");

        return new FirefoxDriver(options);
    }
}
