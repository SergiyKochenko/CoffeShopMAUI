using System.Collections.ObjectModel;
using CoffeShopMAUI.Models;
using CoffeShopMAUI.Services;

namespace CoffeShopMAUI.ViewModels;

public partial class AdminDashboardViewModel : ObservableObject
{
    private readonly OrderStorageService _orderStorageService;
    private bool _isBusy;

    public AdminDashboardViewModel(OrderStorageService orderStorageService)
    {
        _orderStorageService = orderStorageService;
        Orders = new ObservableCollection<Order>();
        CustomerAccounts = new ObservableCollection<CustomerAccountSummary>();
        LoadDataCommand = new AsyncRelayCommand(LoadDataAsync);
        ViewOrderCommand = new AsyncRelayCommand<Order>(ShowOrderAsync);
        ViewCustomerOrdersCommand = new AsyncRelayCommand<CustomerAccountSummary>(ShowCustomerOrdersAsync);
    }

    public ObservableCollection<Order> Orders { get; }
    public ObservableCollection<CustomerAccountSummary> CustomerAccounts { get; }

    public IAsyncRelayCommand LoadDataCommand { get; }
    public IAsyncRelayCommand<Order> ViewOrderCommand { get; }
    public IAsyncRelayCommand<CustomerAccountSummary> ViewCustomerOrdersCommand { get; }

    public bool IsBusy
    {
        get => _isBusy;
        set => SetProperty(ref _isBusy, value);
    }

    private async Task LoadDataAsync()
    {
        if (IsBusy)
        {
            return;
        }

        try
        {
            IsBusy = true;
            Orders.Clear();
            CustomerAccounts.Clear();

            var orders = await _orderStorageService.GetAllOrdersAsync();
            foreach (var order in orders)
            {
                Orders.Add(order);
            }

            var grouped = orders
                .GroupBy(o => new { o.CustomerName, o.PhoneNumber })
                .OrderByDescending(g => g.Max(o => o.CreatedAt));

            foreach (var group in grouped)
            {
                CustomerAccounts.Add(new CustomerAccountSummary
                {
                    CustomerName = group.Key.CustomerName,
                    PhoneNumber = group.Key.PhoneNumber,
                    OrdersCount = group.Count(),
                    TotalSpent = group.Sum(o => o.TotalAmount),
                    LastOrder = group.Max(o => o.CreatedAt)
                });
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

    private static async Task ShowCustomerOrdersAsync(CustomerAccountSummary? summary)
    {
        if (summary is null || Shell.Current is null)
        {
            return;
        }

        await Shell.Current.GoToAsync(nameof(AdminCustomerOrdersPage), true, new Dictionary<string, object>
        {
            ["CustomerName"] = summary.CustomerName,
            ["PhoneNumber"] = summary.PhoneNumber
        });
    }
}
