using System.IO;
using System.Text.Json;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.Services;

public class OrderStorageService
{
    private const string OrdersFileName = "orders.json";

    private string FilePath => Path.Combine(FileSystem.AppDataDirectory, OrdersFileName);

    public async Task SaveOrderAsync(Order order)
    {
        var orders = await LoadOrdersInternalAsync();
        orders.Add(order);
        await PersistAsync(orders);
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForDateAsync(DateOnly date)
    {
        var orders = await LoadOrdersInternalAsync();
        return orders
            .Where(o => DateOnly.FromDateTime(o.CreatedAt.LocalDateTime) == date)
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
    }

    private async Task<List<Order>> LoadOrdersInternalAsync()
    {
        if (!File.Exists(FilePath))
        {
            return new List<Order>();
        }

        await using var stream = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
        var orders = await JsonSerializer.DeserializeAsync<List<Order>>(stream) ?? new List<Order>();
        return orders;
    }

    private async Task PersistAsync(List<Order> orders)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(FilePath)!);
        await using var stream = File.Open(FilePath, FileMode.Create, FileAccess.Write, FileShare.None);
        var options = new JsonSerializerOptions { WriteIndented = true };
        await JsonSerializer.SerializeAsync(stream, orders, options);
    }
}
