using Microsoft.Extensions.DependencyInjection;

namespace CoffeShopMAUI.Pages;

public partial class HomePage : ContentPage
{
    private readonly HomeViewModel _homeViewModel;

    public HomePage() : this(MauiProgram.Services.GetRequiredService<HomeViewModel>())
    {
    }

    public HomePage(HomeViewModel homeViewModel)
    {
        InitializeComponent();
        _homeViewModel = homeViewModel;
        BindingContext = _homeViewModel;
    }
}
