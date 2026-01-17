namespace CoffeShopMAUI.ViewModels;

[QueryProperty(nameof(Order), nameof(Order))]
public partial class OrderReceiptViewModel : ObservableObject
{
    [ObservableProperty]
    private Order _order = default!;

    partial void OnOrderChanged(Order value)
    {
        OnPropertyChanged(nameof(Items));
    }

    public IEnumerable<OrderLine> Items
    {
        get
        {
            // Explicitly reference the backing field to avoid ambiguity
            return _order?.Items ?? Enumerable.Empty<OrderLine>();
        }
    }
}
