using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels;

public partial class AccountInfoViewModel : ObservableObject
{
    public AccountInfoViewModel()
    {
        UpdateAdminState();
        LoadAccountInfo();
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
        UpdateAdminState();
        LoadAccountInfo();
    }

    private void LoadAccountInfo()
    {
        var savedName = Preferences.Default.Get("LastCustomerName", string.Empty);
        var savedPhone = Preferences.Default.Get("LastCustomerPhone", string.Empty);

        if (IsAdmin)
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
        HasInfo = !string.IsNullOrWhiteSpace(CustomerName) && !string.IsNullOrWhiteSpace(PhoneNumber);
    }

    private void UpdateAdminState()
    {
        IsAdmin = AdminAccessService.HasAccess;
        PageTitle = IsAdmin ? "Admin Account" : "My Account";
        AdminPasscode = IsAdmin ? AdminAccessService.Passcode : string.Empty;
    }
}
