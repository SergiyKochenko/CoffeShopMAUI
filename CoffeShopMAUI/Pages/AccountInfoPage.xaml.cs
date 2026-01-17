using CoffeShopMAUI.ViewModels;

namespace CoffeShopMAUI.Pages;

public partial class AccountInfoPage : ContentPage
{
    public AccountInfoPage(AccountInfoViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is AccountInfoViewModel vm)
        {
            vm.RefreshCommand.Execute(null);
        }
    }
}
