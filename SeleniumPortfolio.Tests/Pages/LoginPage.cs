using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SeleniumPortfolio.Tests.Pages;

public sealed class LoginPage(IWebDriver driver, WebDriverWait wait) : BasePage(driver, wait)
{
    private static readonly By UsernameInput = By.Id("user-name");
    private static readonly By PasswordInput = By.Id("password");
    private static readonly By LoginButton = By.Id("login-button");
    private static readonly By ErrorMessage = By.CssSelector("[data-test='error']");

    public void Open(string baseUrl) => Driver.Navigate().GoToUrl(baseUrl);

    public void LoginAs(string username, string password)
    {
        Type(UsernameInput, username);
        Type(PasswordInput, password);
        Click(LoginButton);
    }
    public bool IsOnLoginPage => IsVisible(LoginButton, TimeSpan.FromSeconds(1));
    public string GetErrorMessage() => TextOf(ErrorMessage);
    public void OpenDirect(string baseUrl, string path) => Driver.Navigate().GoToUrl($"{baseUrl.TrimEnd('/')}/{path}");
}
