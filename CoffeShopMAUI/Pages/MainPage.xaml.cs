using CoffeShopMAUI.Services;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.Pages;

public partial class MainPage : ContentPage
{
    private Entry? AdminPasscodeEntry => this.FindByName<Entry>("AdminCodeEntry");

    public MainPage()
    {
        InitializeComponent();
        var storedName = Preferences.Default.Get("LastCustomerName", string.Empty);
        var storedPhone = Preferences.Default.Get("LastCustomerPhone", string.Empty);

        if (NameEntry is not null && !string.IsNullOrWhiteSpace(storedName))
        {
            NameEntry.Text = storedName;
        }

        if (PhoneEntry is not null && !string.IsNullOrWhiteSpace(storedPhone))
        {
            PhoneEntry.Text = storedPhone;
        }
    }

    private async void EnterButton_Clicked(object sender, EventArgs e)
    {
        var name = NameEntry?.Text?.Trim();
        var phone = PhoneEntry?.Text?.Trim();

        if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(phone))
        {
            await DisplayAlert("Welcome", "Please enter your name and phone number to continue.", "OK");
            return;
        }

        var safeName = name!;
        var safePhone = phone!;

        Preferences.Default.Set("LastCustomerName", safeName);
        Preferences.Default.Set("LastCustomerPhone", safePhone);
        AdminAccessService.RevokeAccess();

        if (Application.Current is not null)
        {
            Application.Current.MainPage = new AppShell();
        }
    }

    private async void AdminButton_Clicked(object sender, EventArgs e)
    {
        var adminName = NameEntry?.Text?.Trim();
        var adminPhone = PhoneEntry?.Text?.Trim();

        if (string.IsNullOrWhiteSpace(adminName) || string.IsNullOrWhiteSpace(adminPhone))
        {
            await DisplayAlert("Admin access", "Enter your admin name and phone number before opening the dashboard.", "OK");
            return;
        }

        var code = AdminPasscodeEntry?.Text;
        if (!AdminAccessService.TryAuthenticate(code))
        {
            await DisplayAlert("Access denied", "Invalid admin passcode.", "OK");
            return;
        }

        var safeName = adminName!;
        var safePhone = adminPhone!;

        Preferences.Default.Set("LastCustomerName", safeName);
        Preferences.Default.Set("LastCustomerPhone", safePhone);
        AdminAccessService.RecordActiveAdmin(safeName, safePhone);

        if (AdminPasscodeEntry is not null)
        {
            AdminPasscodeEntry.Text = string.Empty;
        }
        if (Application.Current is null)
        {
            return;
        }

        var shell = new AppShell();
        Application.Current.MainPage = shell;
        await shell.GoToAsync(nameof(AdminDashboardPage), true);
    }
}

