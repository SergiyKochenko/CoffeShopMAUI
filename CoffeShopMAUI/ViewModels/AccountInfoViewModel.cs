using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels;

public partial class AccountInfoViewModel : ObservableObject
{
    public AccountInfoViewModel()
    {
        LoadAccountInfo();
        UpdateAdminState();
    }

    [ObservableProperty]
    private string _customerName = string.Empty;

    [ObservableProperty]
    private string _phoneNumber = string.Empty;

    [ObservableProperty]
    private bool _hasInfo;

    [ObservableProperty]
    private bool _isAdmin;

    [ObservableProperty]
    private string _adminPasscode = string.Empty;

    [ObservableProperty]
    private string _pageTitle = "My Account";

    [RelayCommand]
    private void Refresh()
    {
        LoadAccountInfo();
        UpdateAdminState();
    }

    private void LoadAccountInfo()
    {
        CustomerName = Preferences.Default.Get("LastCustomerName", string.Empty);
        PhoneNumber = Preferences.Default.Get("LastCustomerPhone", string.Empty);
        HasInfo = !string.IsNullOrWhiteSpace(CustomerName) && !string.IsNullOrWhiteSpace(PhoneNumber);
    }

    private void UpdateAdminState()
    {
        IsAdmin = AdminAccessService.HasAccess;
        PageTitle = IsAdmin ? "Admin Account" : "My Account";
        AdminPasscode = IsAdmin ? AdminAccessService.Passcode : string.Empty;
    }
}
