using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Cart")]
[Category("Cart")]
public sealed class ShoppingCartTests : TestBase
{
    [Test]
    public void AddThenRemoveProduct_ShouldUpdateCart()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddBackpackToCart();
        inventory.OpenCart();

        Assert.That(CartPage.ItemCount, Is.EqualTo(1));
        Assert.That(CartPage.ContainsBackpack, Is.True);

        CartPage.RemoveBackpack();

        Assert.That(CartPage.WaitUntilEmpty(), Is.True);
        Assert.That(CartPage.ItemCount, Is.EqualTo(0));
    }

    [Test]
    public void AddMultipleProducts_ShouldUpdateBadgeAndCart()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddToCart("sauce-labs-backpack");
        inventory.AddToCart("sauce-labs-bike-light");

        inventory.WaitForCartBadge(2);
        inventory.OpenCart();

        Assert.That(CartPage.ItemCount, Is.EqualTo(2));
    }

    [Test]
    public void RemoveFromInventoryPage_ShouldClearCartBadge()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddToCart("sauce-labs-backpack");
        inventory.WaitForCartBadge(1);

        inventory.RemoveFromCart("sauce-labs-backpack");

        inventory.WaitForCartBadge(0);
        Assert.That(inventory.CartBadgeCount, Is.EqualTo(0));
    }
}
