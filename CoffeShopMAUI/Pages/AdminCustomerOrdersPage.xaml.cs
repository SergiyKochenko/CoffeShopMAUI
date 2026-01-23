using CoffeShopMAUI.ViewModels;

namespace CoffeShopMAUI.Pages;

public partial class AdminCustomerOrdersPage : ContentPage
{
    public AdminCustomerOrdersPage(AdminCustomerOrdersViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
