using System.IO;
using System.Linq;
using CoffeShopMAUI.Models;
using CoffeShopMAUI.Services;

namespace CoffeShopMAUI.Tests;

public class CoffeeMenuServiceTests
{
    [Fact]
    public void GetByCategory_ReturnsOnlyMatchingDrinks()
    {
        var service = new CoffeeMenuService();

        var results = service.GetByCategory("Food").ToList();

        Assert.NotEmpty(results);
        Assert.All(results, drink => Assert.Equal("Food", drink.Category));
    }

    [Fact]
    public void SearchMenu_FiltersByTermWithinCategory()
    {
        var service = new CoffeeMenuService();

        var results = service.SearchMenu("latte", "Hot drinks").ToList();

        Assert.All(results, drink => Assert.Contains("latte", drink.Name, StringComparison.OrdinalIgnoreCase));
    }
}

public class OrderStorageServiceTests
{
    [Fact]
    public async Task SaveOrderAsync_PersistsAndFiltersByDate()
    {
        var tempDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(tempDir);
        try
        {
            var service = new OrderStorageService(tempDir);
            var order = new Order
            {
                OrderNumber = "ORD-TEST",
                CreatedAt = DateTimeOffset.Now,
                CustomerName = "Test User",
                PhoneNumber = "1234567890",
                Items =
                {
                    new OrderLine { Name = "Latte", Quantity = 2, UnitPrice = 3.5 }
                },
                TotalAmount = 7.0
            };

            await service.SaveOrderAsync(order);

            var today = DateOnly.FromDateTime(DateTime.Now);
            var todaysOrders = await service.GetOrdersForDateAsync(today);

            Assert.Single(todaysOrders);
            Assert.Equal("ORD-TEST", todaysOrders[0].OrderNumber);
        }
        finally
        {
            if (Directory.Exists(tempDir))
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
