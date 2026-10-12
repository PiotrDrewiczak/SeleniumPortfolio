using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

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
}
