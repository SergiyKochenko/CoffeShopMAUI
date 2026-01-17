namespace CoffeShopMAUI.Models;

public class Order
{
    public string OrderNumber { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public List<OrderLine> Items { get; set; } = new();
    public double TotalAmount { get; set; }
}

public class OrderLine
{
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public double UnitPrice { get; set; }
    public double LineTotal => Quantity * UnitPrice;
}
