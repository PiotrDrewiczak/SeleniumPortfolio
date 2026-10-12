using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Inventory")]
[Category("Inventory")]
public sealed class InventoryTests : TestBase
{
    [Test, Category("Smoke")]
    public void SortByPriceLowToHigh_ShouldPutCheapestProductFirst()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.SortByPriceLowToHigh();

        Assert.That(inventory.GetFirstProductPrice(), Is.EqualTo(7.99m));
    }

    [Test]
    public void Inventory_ShouldListSixProducts()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        Assert.That(inventory.ProductCount, Is.EqualTo(6));
    }

    [TestCase("az")]
    [TestCase("za")]
    public void SortByName_ShouldOrderAllProducts(string option)
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.SortBy(option);

        var ascending = inventory.GetProductNames().OrderBy(n => n, StringComparer.Ordinal).ToList();
        var expected = option == "az" ? ascending : Enumerable.Reverse(ascending).ToList();
        Assert.That(inventory.GetProductNames(), Is.EqualTo(expected));
    }

    [TestCase("lohi")]
    [TestCase("hilo")]
    public void SortByPrice_ShouldOrderAllProducts(string option)
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.SortBy(option);

        var ascending = inventory.GetProductPrices().OrderBy(p => p).ToList();
        var expected = option == "lohi" ? ascending : Enumerable.Reverse(ascending).ToList();
        Assert.That(inventory.GetProductPrices(), Is.EqualTo(expected));
    }
}
