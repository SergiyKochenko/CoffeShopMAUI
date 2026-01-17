using Microsoft.Extensions.DependencyInjection;

namespace CoffeShopMAUI.Pages;

[QueryProperty(nameof(Order), nameof(Order))]
public partial class OrderReceiptPage : ContentPage
{
    public OrderReceiptPage() : this(MauiProgram.Services.GetRequiredService<OrderReceiptViewModel>())
    {
    }

    public OrderReceiptPage(OrderReceiptViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Receives the Shell navigation parameter and forwards it to the ViewModel
    public Order Order
    {
        get => ((OrderReceiptViewModel)BindingContext).Order;
        set => ((OrderReceiptViewModel)BindingContext).Order = value;
    }

    private async void Done_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
    }
}
