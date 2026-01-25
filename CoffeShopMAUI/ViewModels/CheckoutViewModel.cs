using System.ComponentModel;
using System.Text.RegularExpressions;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels;

public partial class CheckoutViewModel : ObservableObject
{
    private readonly CartViewModel _cartViewModel;
    private readonly OrderStorageService _orderStorageService;
    private static readonly Regex CustomerNameRegex = new("^[A-Za-zÀ-ÿ'\\- ]+$", RegexOptions.Compiled);
    private static readonly Regex AdminNameRegex = new("^[A-Za-zÀ-ÿ0-9'\\- ]+$", RegexOptions.Compiled);

    public CheckoutViewModel(CartViewModel cartViewModel, OrderStorageService orderStorageService)
    {
        _cartViewModel = cartViewModel;
        _orderStorageService = orderStorageService;
        _cartViewModel.PropertyChanged += OnCartPropertyChanged;
        OrderNumber = GenerateOrderNumber();
        LoadCustomerInfo();
    }

    public ObservableCollection<CoffeeDrink> Items => _cartViewModel.Items;

    public double TotalAmount => _cartViewModel.TotalAmount;

    [ObservableProperty]
    private string _customerName = string.Empty;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private string _orderNumber = string.Empty;

    [RelayCommand]
    private async Task SubmitOrder()
    {
        if (!Items.Any())
        {
            await Toast.Make("Your cart is empty", ToastDuration.Short).Show();
            return;
        }

        if (string.IsNullOrWhiteSpace(CustomerName) || string.IsNullOrWhiteSpace(PhoneNumber))
        {
            await Toast.Make("Please enter your name and phone number", ToastDuration.Short).Show();
            return;
        }

        var trimmedName = CustomerName.Trim();
        var trimmedPhone = PhoneNumber.Trim();

        var nameRegex = AdminAccessService.HasAccess ? AdminNameRegex : CustomerNameRegex;
        if (!nameRegex.IsMatch(trimmedName))
        {
            var message = AdminAccessService.HasAccess
                ? "Admin name can contain letters and numbers"
                : "Name must contain letters only";
            await Toast.Make(message, ToastDuration.Short).Show();
            return;
        }

        Preferences.Default.Set("LastCustomerName", trimmedName);
        Preferences.Default.Set("LastCustomerPhone", trimmedPhone);

        var order = new Order
        {
            OrderNumber = OrderNumber,
            CreatedAt = DateTimeOffset.Now,
            CustomerName = trimmedName,
            PhoneNumber = trimmedPhone,
            Items = Items.Select(i => new OrderLine
            {
                Name = i.Name,
                Quantity = i.CartQuantity,
                UnitPrice = i.Price
            }).ToList(),
            TotalAmount = TotalAmount
        };

        await _orderStorageService.SaveOrderAsync(order);

        _cartViewModel.CompleteCheckout();

        await Shell.Current.GoToAsync(nameof(OrderReceiptPage), true, new Dictionary<string, object>
        {
            [nameof(OrderReceiptViewModel.Order)] = order
        });

        OrderNumber = GenerateOrderNumber();
        LoadCustomerInfo();
    }

    public void RefreshIdentity() => LoadCustomerInfo();

    private void OnCartPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(CartViewModel.TotalAmount))
        {
            OnPropertyChanged(nameof(TotalAmount));
        }
    }

    private void LoadCustomerInfo()
    {
        var savedName = Preferences.Default.Get("LastCustomerName", string.Empty);
        var savedPhone = Preferences.Default.Get("LastCustomerPhone", string.Empty);

        if (AdminAccessService.HasAccess)
        {
            var (adminName, adminPhone) = AdminAccessService.GetActiveAdmin();
            if (!string.IsNullOrWhiteSpace(adminName))
            {
                savedName = adminName;
            }
            if (!string.IsNullOrWhiteSpace(adminPhone))
            {
                savedPhone = adminPhone;
            }
        }

        CustomerName = savedName;
        PhoneNumber = savedPhone;
    }

    private static string GenerateOrderNumber() => $"ORD-{DateTimeOffset.Now:yyyyMMddHHmmss}";
}
