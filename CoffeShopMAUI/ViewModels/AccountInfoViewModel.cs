using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels;

public partial class AccountInfoViewModel : ObservableObject
{
    public AccountInfoViewModel()
    {
        LoadAccountInfo();
    }

    [ObservableProperty]
    private string _customerName = string.Empty;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private bool _hasInfo;

    [RelayCommand]
    private void Refresh() => LoadAccountInfo();

    private void LoadAccountInfo()
    {
        CustomerName = Preferences.Default.Get("LastCustomerName", string.Empty);
        PhoneNumber = Preferences.Default.Get("LastCustomerPhone", string.Empty);
        HasInfo = !string.IsNullOrWhiteSpace(CustomerName) && !string.IsNullOrWhiteSpace(PhoneNumber);
    }
}
