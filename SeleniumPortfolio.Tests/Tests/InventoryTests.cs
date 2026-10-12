using NUnit.Framework;
using SeleniumPortfolio.Tests.Core;
using SeleniumPortfolio.Tests.Data;

namespace SeleniumPortfolio.Tests.Tests;

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
}
