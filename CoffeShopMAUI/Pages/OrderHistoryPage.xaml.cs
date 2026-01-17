using Microsoft.Extensions.DependencyInjection;

namespace CoffeShopMAUI.Pages;

public partial class OrderHistoryPage : ContentPage
{
    private readonly OrderHistoryViewModel _viewModel;

    public OrderHistoryPage() : this(MauiProgram.Services.GetRequiredService<OrderHistoryViewModel>())
    {
    }

    public OrderHistoryPage(OrderHistoryViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.LoadOrdersCommand.Execute(null);
    }
}
