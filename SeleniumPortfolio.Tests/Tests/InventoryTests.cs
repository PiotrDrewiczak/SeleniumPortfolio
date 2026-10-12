using Allure.Net.Commons;
using Allure.NUnit;
using Allure.NUnit.Attributes;
using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

[AllureNUnit]
[AllureSuite("Inventory")]
[AllureEpic("SauceDemo web shop")]
[AllureFeature("Product catalogue")]
[Category("Inventory")]
public sealed class InventoryTests : TestBase
{
    [Test, Category("Smoke")]
    [AllureStory("Sorting")]
    [AllureSeverity(SeverityLevel.critical)]
    [AllureDescription("Sorting by price from low to high puts the cheapest product (7.99) first.")]
    public void SortByPriceLowToHigh_ShouldPutCheapestProductFirst()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        inventory.SortByPriceLowToHigh();

        Assert.That(inventory.GetFirstProductPrice(), Is.EqualTo(7.99m));
    }

    [Test]
    [AllureStory("Product list")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("After login the catalogue shows all six products.")]
    public void Inventory_ShouldListSixProducts()
    {
        var inventory = LoginAs(Users.Standard);
        inventory.WaitUntilLoaded();

        Assert.That(inventory.ProductCount, Is.EqualTo(6));
    }

    [TestCase("az")]
    [TestCase("za")]
    [AllureStory("Sorting")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Sorting by name orders the whole product list alphabetically, A to Z and Z to A.")]
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
    [AllureStory("Sorting")]
    [AllureSeverity(SeverityLevel.normal)]
    [AllureDescription("Sorting by price orders the whole product list, low to high and high to low.")]
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
