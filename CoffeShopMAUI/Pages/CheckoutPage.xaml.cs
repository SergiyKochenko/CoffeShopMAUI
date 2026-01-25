using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace CoffeShopMAUI.Pages;

public partial class CheckoutPage : ContentPage
{
    public CheckoutPage() : this(MauiProgram.Services.GetRequiredService<CheckoutViewModel>())
    {
    }

    public CheckoutPage(CheckoutViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is CheckoutViewModel vm)
        {
            vm.RefreshIdentity();
        }
    }

    private static readonly Regex _customerNameRegex = new("^[A-Za-zÀ-ÿ'\\- ]*$", RegexOptions.Compiled);
    private static readonly Regex _adminNameRegex = new("^[A-Za-zÀ-ÿ0-9'\\- ]*$", RegexOptions.Compiled);

    private static Regex CurrentRegex => AdminAccessService.HasAccess ? _adminNameRegex : _customerNameRegex;

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
        {
            return;
        }

        if (CurrentRegex.IsMatch(e.NewTextValue ?? string.Empty))
        {
            return;
        }

        // Revert to previous valid value when invalid characters are entered.
        entry.Text = e.OldTextValue;
    }
}
