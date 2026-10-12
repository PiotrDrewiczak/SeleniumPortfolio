using Allure.NUnit.Attributes;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class LoginPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By UsernameInput = By.Id("user-name");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By LoginButton = By.Id("login-button");
    private static readonly By ErrorMessage = By.CssSelector("[data-test='error']");

    [AllureStep("Open the login page")]
    public void Open(string baseUrl) => Driver.Navigate().GoToUrl(baseUrl);

    [AllureStep("Log in as user {0}")]
    public void LoginAs(string username, string password)
    {
        Type(UsernameInput, username);
        Type(PasswordInput, password);
        Click(LoginButton);
    }
    public bool IsOnLoginPage => IsVisible(LoginButton, TimeSpan.FromSeconds(1));
    /// <summary>Waits up to the full timeout for the login form, e.g. after a logout redirect.</summary>
    [AllureStep("Wait for the login form")]
    public bool WaitUntilLoginFormShown() => IsVisible(LoginButton);

    [AllureStep("Read the login error message")]
    public string GetErrorMessage() => TextOf(ErrorMessage);

    [AllureStep("Open {1} directly without logging in")]
    public void OpenDirect(string baseUrl, string path) => Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/{path}");
}
