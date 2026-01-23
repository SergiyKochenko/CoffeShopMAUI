using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.ViewModels
{
    public partial class HomeViewModel : ObservableObject
    {
        private const string FreshBrewsDisplayTitle = "Fresh Brews";
        private const string FreshBrewsFilter = "Fresh Brews";
        private readonly CoffeeMenuService _menuService;

        public HomeViewModel(CoffeeMenuService menuService)
        {
            ArgumentNullException.ThrowIfNull(menuService);
            _menuService = menuService;
            FeaturedDrinks = new ObservableCollection<CoffeeDrink>(_menuService.GetFeaturedDrinks());
        }

        public ObservableCollection<CoffeeDrink> FeaturedDrinks { get; }

        [RelayCommand]
        private static async Task Logout()
        {
            var page = Application.Current?.MainPage;
            if (page is null)
            {
                return;
            }

            if (!await page.DisplayAlert("Logout", "Do you want to sign out?", "Yes", "No"))
            {
                return;
            }

            Preferences.Default.Remove("LastCustomerName");
            Preferences.Default.Remove("LastCustomerPhone");
            AdminAccessService.RevokeAccess();

            MainThread.BeginInvokeOnMainThread(() => Application.Current!.MainPage = new Pages.MainPage());
        }

        [RelayCommand]
        private async Task GoToMenuPage() => await NavigateToMenuAsync(false, FreshBrewsDisplayTitle, FreshBrewsFilter);

        [RelayCommand]
        private async Task GoToMenuFromSearch() => await NavigateToMenuAsync(true, "Search for coffee");

        [RelayCommand]
        private async Task GoToAllItems() => await NavigateToMenuAsync(false, "All Items");

        [RelayCommand]
        private async Task GoToHistory()
        {
            var shell = Shell.Current;
            if (shell is null)
            {
                return;
            }

            await shell.GoToAsync(nameof(OrderHistoryPage), animate: true);
        }

        [RelayCommand]
        private async Task GoToAccountInfo()
        {
            var shell = Shell.Current;
            if (shell is null)
            {
                return;
            }

            await shell.GoToAsync(nameof(AccountInfoPage), animate: true);
        }

        [RelayCommand]
        private async Task GoToCategory(string category)
        {
            var shell = Shell.Current;
            if (shell is null)
            {
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                ["Category"] = category,
                ["FromSearch"] = false,
                [nameof(CoffeeMenuViewModel.DisplayTitle)] = category
            };

            await shell.GoToAsync(nameof(CoffeeMenuPage), animate: true, parameters);
        }

        [RelayCommand]
        private async Task GoToDrinkDetails(CoffeeDrink drink)
        {
            var shell = Shell.Current;
            if (shell is null)
            {
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                [nameof(CoffeeDetailsViewModel.Drink)] = drink
            };

            await shell.GoToAsync(nameof(CoffeeDetailPage), animate: true, parameters);
        }

        private async Task NavigateToMenuAsync(bool fromSearch, string displayTitle, string category = "All")
        {
            var shell = Shell.Current;
            if (shell is null)
            {
                return;
            }

            var parameters = new Dictionary<string, object>
            {
                ["FromSearch"] = fromSearch,
                ["Category"] = category,
                [nameof(CoffeeMenuViewModel.DisplayTitle)] = displayTitle
            };

            await shell.GoToAsync(nameof(CoffeeMenuPage), animate: true, parameters);
        }
    }
}

