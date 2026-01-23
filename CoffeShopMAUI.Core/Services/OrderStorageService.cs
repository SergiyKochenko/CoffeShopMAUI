using System.IO;
using System.Text.Json;
using CoffeShopMAUI.Models;

namespace CoffeShopMAUI.Services;

public class OrderStorageService
{
    private const string OrdersFileName = "orders.json";
    private readonly string _baseDirectory;

    public OrderStorageService(string baseDirectory)
    {
        if (string.IsNullOrWhiteSpace(baseDirectory))
        {
            throw new ArgumentException("Base directory must be provided", nameof(baseDirectory));
        }

        _baseDirectory = baseDirectory;
    }

    private string FilePath => Path.Combine(_baseDirectory, OrdersFileName);

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

    public async Task<IReadOnlyList<Order>> GetAllOrdersAsync()
    {
        var orders = await LoadOrdersInternalAsync();
        return orders
            .OrderByDescending(o => o.CreatedAt)
            .ToList();
    }

    public async Task<IReadOnlyList<Order>> GetOrdersForCustomerAsync(string customerName, string phoneNumber)
    {
        var orders = await LoadOrdersInternalAsync();
        return orders
            .Where(o => string.Equals(o.CustomerName, customerName, StringComparison.OrdinalIgnoreCase)
                     && string.Equals(o.PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase))
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
