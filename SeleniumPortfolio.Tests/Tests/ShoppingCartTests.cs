using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Cart")]
[AllureEpic("SauceDemo web shop")]
[AllureFeature("Shopping cart")]
[Category("Cart")]
public sealed class ShoppingCartTests : TestBase
{
    [Test]
    [AllureStory("Add and remove products")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("A product added to the cart appears on the cart page and disappears after it is removed.")]
    public void AddThenRemoveProduct_ShouldUpdateCart()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddBackpackToCart();
        inventory.OpenCart();
        CartPage.WaitForItemCount(1);

        Assert.That(CartPage.ItemCount, Is.EqualTo(1));
        Assert.That(CartPage.ContainsBackpack, Is.True);

        CartPage.RemoveBackpack();

        Assert.That(CartPage.WaitUntilEmpty(), Is.True);
        Assert.That(CartPage.ItemCount, Is.EqualTo(0));
    }

    [Test]
    [AllureStory("Add and remove products")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Adding two products raises the cart badge to 2 and both products are listed in the cart.")]
    public void AddMultipleProducts_ShouldUpdateBadgeAndCart()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddToCart("sauce-labs-backpack");
        inventory.AddToCart("sauce-labs-bike-light");

        inventory.WaitForCartBadge(2);
        inventory.OpenCart();
        CartPage.WaitForItemCount(2);

        Assert.That(CartPage.ItemCount, Is.EqualTo(2));
    }

    [Test]
    [AllureStory("Add and remove products")]
    [AllureSeverity(SeverityLevel.minor)]
    [AllureDescription("Removing a product directly from the product list clears the cart badge.")]
    public void RemoveFromInventoryPage_ShouldClearCartBadge()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddToCart("sauce-labs-backpack");
        inventory.WaitForCartBadge(1);

        inventory.RemoveFromCart("sauce-labs-backpack");

        inventory.WaitForCartBadge(0);
        Assert.That(inventory.CartBadgeCount, Is.EqualTo(0));
    }

    [Test]
    [AllureStory("Cart contents")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Products stay in the cart after the user continues shopping and returns to the cart.")]
    public void CartContents_ShouldPersistAfterContinueShopping()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.AddToCart("sauce-labs-backpack");
        inventory.OpenCart();

        CartPage.ContinueShopping();
        inventory.WaitUntilLoaded();
        inventory.OpenCart();
        CartPage.WaitForItemCount(1);

        Assert.That(CartPage.GetItemNames(), Is.EqualTo(new[] { "Sauce Labs Backpack" }));
    }

    [Test]
    [AllureStory("Product details")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("The product details page shows the same name and price as the product list, and links back to it.")]
    public void ProductDetails_ShouldMatchProductList()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.OpenProduct("Sauce Labs Bike Light");

        Assert.Multiple(() =>
        {
            Assert.That(ProductDetailsPage.GetName(), Is.EqualTo("Sauce Labs Bike Light"));
            Assert.That(ProductDetailsPage.GetPrice(), Is.EqualTo(9.99m));
        });

        ProductDetailsPage.BackToProducts();
        inventory.WaitUntilLoaded();
        Assert.That(inventory.ProductCount, Is.EqualTo(6));
    }
}
