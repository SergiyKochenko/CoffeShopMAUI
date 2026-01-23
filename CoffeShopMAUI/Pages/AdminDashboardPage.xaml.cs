using System.Linq;
using CoffeShopMAUI.Services;
using CoffeShopMAUI.ViewModels;

namespace CoffeShopMAUI.Pages;

public partial class AdminDashboardPage : ContentPage
{
    private readonly AdminDashboardViewModel _viewModel;

    public AdminDashboardPage(AdminDashboardViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!AdminAccessService.HasAccess)
        {
            await DisplayAlert("Access denied", "Please sign in as an admin from the login screen.", "OK");
            Application.Current.MainPage = new MainPage();
            return;
        }

        if (_viewModel.Orders.Any() || _viewModel.CustomerAccounts.Any())
        {
            return;
        }

        await _viewModel.LoadDataCommand.ExecuteAsync(null);
    }
}
