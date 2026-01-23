namespace CoffeShopMAUI.Models;

public class CustomerAccountSummary
{
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int OrdersCount { get; set; }
    public double TotalSpent { get; set; }
    public DateTimeOffset LastOrder { get; set; }
}
