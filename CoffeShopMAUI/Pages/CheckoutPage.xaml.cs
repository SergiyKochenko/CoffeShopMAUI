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

    private static readonly Regex _nameRegex = new("^[A-Za-zÀ-ÿ'\\- ]*$", RegexOptions.Compiled);

    private void NameEntry_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is not Entry entry)
        {
            return;
        }

        if (_nameRegex.IsMatch(e.NewTextValue ?? string.Empty))
        {
            return;
        }

        // Revert to previous valid value when invalid characters are entered.
        entry.Text = e.OldTextValue;
    }
}
