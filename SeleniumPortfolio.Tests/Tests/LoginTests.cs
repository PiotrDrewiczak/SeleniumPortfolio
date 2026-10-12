using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Login")]
[Category("Login")]
public sealed class LoginTests : TestBase
{
    [Test, Category("Smoke")]
    public void ValidCredentials_ShouldOpenInventory()
    {
        var inventory = LoginAs(Users.Standard);

        Assert.That(inventory.IsLoaded, Is.True, "The Products page should be displayed after a successful login.");
        Assert.That(inventory.ProductCount, Is.GreaterThan(0));
    }

    [Test]
    public void Logout_ShouldReturnToLoginPage()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.Logout();

        Assert.That(LoginPage.WaitUntilLoginFormShown(), Is.True, "Logging out should show the login form.");
    }

    [Test]
    public void InvalidPassword_ShouldShowHelpfulError()
    {
        LoginAs(Users.WrongPassword);

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("Username and password do not match"));
    }

    [Test]
    public void LockedOutUser_ShouldBeRejected()
    {
        LoginAs(Users.LockedOut);

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("this user has been locked out"));
    }

    [TestCase("", "secret_sauce", "Username is required")]
    [TestCase("standard_user", "", "Password is required")]
    public void MissingCredential_ShouldShowRequiredFieldError(string username, string password, string expected)
    {
        LoginAs(new TestUser(username, password));

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain(expected));
    }

    [TestCase("no_such_user", "secret_sauce")]
    [TestCase("STANDARD_USER", "secret_sauce")]
    [TestCase("standard_user", "SECRET_SAUCE")]
    public void UnknownOrWrongCaseCredentials_ShouldBeRejected(string username, string password)
    {
        LoginAs(new TestUser(username, password));

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("Username and password do not match"));
    }
    [Test]
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
    public void InventoryWithoutLogin_ShouldRedirectToLogin()
    {
        LoginPage.OpenDirect(Settings.BaseUrl, "inventory.html");

        Assert.That(LoginPage.GetErrorMessage(), Does.Contain("You can only access '/inventory.html' when you are logged in"));
    }
}
