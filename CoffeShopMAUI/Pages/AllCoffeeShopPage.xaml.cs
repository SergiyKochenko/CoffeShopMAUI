using CoffeShopMAUI.ViewModels;

namespace CoffeShopMAUI.Pages;

public partial class CoffeeMenuPage : ContentPage
{
    private readonly CoffeeMenuViewModel _coffeeMenuViewModel;

    public CoffeeMenuPage(CoffeeMenuViewModel coffeeMenuViewModel)
    {
        InitializeComponent();
        _coffeeMenuViewModel = coffeeMenuViewModel;
        BindingContext = _coffeeMenuViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_coffeeMenuViewModel.FromSearch)
        {
            await Task.Delay(100);
            searchBar.Focus();
        }
    }

    void searchBar_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (!string.IsNullOrWhiteSpace(e.OldTextValue)
            && string.IsNullOrWhiteSpace(e.NewTextValue))
        {
            _coffeeMenuViewModel.SearchMenuCommand.Execute(null);
        }
    }
}
