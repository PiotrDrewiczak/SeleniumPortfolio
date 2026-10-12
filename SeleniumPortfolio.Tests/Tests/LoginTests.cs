using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Login")]
[AllureEpic("SauceDemo web shop")]
[AllureFeature("Authentication")]
[Category("Login")]
public sealed class LoginTests : TestBase
{
    [Test, Category("Smoke")]
    [AllureStory("Log in")]
    [AllureSeverity(SeverityLevel.blocker)]
    [AllureDescription("A user with valid credentials logs in and lands on the Products page with products listed.")]
    public void ValidCredentials_ShouldOpenInventory()
    {
        var inventory = LoginAs(Users.Standard);

        Assert.That(inventory.IsLoaded, Is.True, "The Products page should be displayed after a successful login.");
        Assert.That(inventory.ProductCount, Is.GreaterThan(0));
    }

    [Test]
    [AllureStory("Log out")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("A logged-in user logs out from the burger menu and sees the login form again.")]
    public void Logout_ShouldReturnToLoginPage()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.Logout();

        Assert.That(LoginPage.WaitUntilLoginFormShown(), Is.True, "Logging out should show the login form.");
    }

    [Test]
    [AllureStory("Rejected login")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Logging in with a correct username and a wrong password shows a clear error message.")]
    public void InvalidPassword_ShouldShowHelpfulError()
    {
        LoginAs(Users.WrongPassword);

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("Username and password do not match"));
    }

    [Test]
    [AllureStory("Rejected login")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("A locked-out user cannot log in and is told the account has been locked.")]
    public void LockedOutUser_ShouldBeRejected()
    {
        LoginAs(Users.LockedOut);

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("this user has been locked out"));
    }

    [TestCase("", "secret_sauce", "Username is required")]
    [TestCase("standard_user", "", "Password is required")]
    [AllureStory("Login form validation")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Submitting the login form with an empty username or password shows a required-field error.")]
    public void MissingCredential_ShouldShowRequiredFieldError(string username, string password, string expected)
    {
        LoginAs(new TestUser(username, password));

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain(expected));
    }

    [TestCase("no_such_user", "secret_sauce")]
    [TestCase("STANDARD_USER", "secret_sauce")]
    [TestCase("standard_user", "SECRET_SAUCE")]
    [AllureStory("Rejected login")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Unknown usernames and credentials with the wrong letter case are rejected (login is case-sensitive).")]
    public void UnknownOrWrongCaseCredentials_ShouldBeRejected(string username, string password)
    {
        LoginAs(new TestUser(username, password));

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("Username and password do not match"));
    }
    [Test]
    [AllureStory("Rejected login")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("After a failed login the user stays on the login page and is not taken to the shop.")]
    public void FailedLogin_ShouldKeepUserOnLoginPage()
    {
        LoginAs(Users.WrongPassword);

        Assert.Multiple(() =>
        {
            Assert.That(LoginPage.GetErrorMessage(), Is.Not.Empty);
            Assert.That(LoginPage.IsOnLoginPage, Is.True, "A failed login must not leave the login page.");
        });
    }
    [Test]
    [AllureStory("Access control")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Opening the Products URL directly without a session redirects to the login page with an access error.")]
    public void InventoryWithoutLogin_ShouldRedirectToLogin()
    {
        LoginPage.OpenDirect(Settings.BaseUrl, "inventory.html");

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("You can only access '/inventory.html' when you are logged in"));
    }
}
