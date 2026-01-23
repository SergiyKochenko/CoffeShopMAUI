using System.Collections.ObjectModel;
using CoffeShopMAUI.Services;

namespace CoffeShopMAUI.ViewModels;

public partial class AdminCustomerOrdersViewModel : ObservableObject, IQueryAttributable
{
    private readonly OrderStorageService _orderStorageService;

    public AdminCustomerOrdersViewModel(OrderStorageService orderStorageService)
    {
        _orderStorageService = orderStorageService;
        Orders = new ObservableCollection<Order>();
        RefreshCommand = new AsyncRelayCommand(LoadOrdersAsync);
        ViewOrderCommand = new AsyncRelayCommand<Order>(ShowOrderAsync);
    }

    public ObservableCollection<Order> Orders { get; }

    private string _customerName = string.Empty;
    public string CustomerName
    {
        get => _customerName;
        set => SetProperty(ref _customerName, value);
    }

    private string _phoneNumber = string.Empty;
    public string PhoneNumber
    {
        get => _phoneNumber;
        set => SetProperty(ref _phoneNumber, value);
    }

    private bool _isBusy;
    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    public IAsyncRelayCommand RefreshCommand { get; }
    public IAsyncRelayCommand<Order> ViewOrderCommand { get; }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (query.TryGetValue("CustomerName", out var name) && name is string nameString)
        {
            CustomerName = nameString;
        }

        if (query.TryGetValue("PhoneNumber", out var phone) && phone is string phoneString)
        {
            PhoneNumber = phoneString;
        }

        _ = LoadOrdersAsync();
    }

    private async Task LoadOrdersAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Orders.Clear();

            if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(PhoneNumber))
            {
                return;
            }

            var orders = await _orderStorageService.GetOrdersForCustomerAsync(CustomerName, PhoneNumber);
            foreach (var order in orders)
            {
                Orders.Add(order);
            }
        }
        finally
        {
            IsBusy = false;
        }
    }

    private static async Task ShowOrderAsync(Order? order)
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
