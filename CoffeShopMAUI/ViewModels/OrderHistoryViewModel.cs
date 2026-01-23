using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels;

public partial class OrderHistoryViewModel : ObservableObject
{
    private readonly OrderStorageService _orderStorageService;

    public OrderHistoryViewModel(OrderStorageService orderStorageService)
    {
        _orderStorageService = orderStorageService;
        Orders = new ObservableCollection<Order>();
    }

    public ObservableCollection<Order> Orders { get; }

    [ObservableProperty]
    private bool _isBusy;

    [RelayCommand]
    private async Task LoadOrders()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Orders.Clear();
            var (customerName, phoneNumber) = GetCurrentUser();

            if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(phoneNumber))
            {
                return;
            }

            var todayOrders = await _orderStorageService.GetOrdersForDateAsync(DateOnly.FromDateTime(DateTime.Now));
            foreach (var order in todayOrders.Where(o => string.Equals(o.CustomerName, customerName, StringComparison.OrdinalIgnoreCase)
                                                        && string.Equals(o.PhoneNumber, phoneNumber, StringComparison.OrdinalIgnoreCase)))
            {
                Orders.Add(order);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static (string Name, string Phone) GetCurrentUser()
    {
        var name = Preferences.Default.Get("LastCustomerName", string.Empty);
        var phone = Preferences.Default.Get("LastCustomerPhone", string.Empty);
        return (name, phone);
    }

    [RelayCommand]
    private async Task ViewOrder(Order? order)
    {
        if (order is null || Shell.Current is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(OrderReceiptPage), true, new Dictionary<string, object>
        {
            [nameof(OrderReceiptViewModel.Order)] = order
        });
    }
}
