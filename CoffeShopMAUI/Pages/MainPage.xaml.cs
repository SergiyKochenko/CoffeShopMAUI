using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.Pages;

public partial class MainPage : ContentPage
{
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

        if (Application.Current is not null)
        {
            Application.Current.MainPage = new AppShell();
        }
    }
}

