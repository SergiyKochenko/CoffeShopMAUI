namespace CoffeShopMAUI.ViewModels
{
    [QueryProperty(nameof(FromSearch), nameof(FromSearch))]
    [QueryProperty(nameof(Category), nameof(Category))]
    [QueryProperty(nameof(DisplayTitle), nameof(DisplayTitle))]
    public partial class CoffeeMenuViewModel : ObservableObject
    {
        private const string FreshBrewsDisplayTitle = "Fresh Brews";
        private const string FreshBrewsFilter = "Fresh Brews";
        private static readonly string[] DrinkCategories = { "Hot drinks", "Cold drinks" };
        private readonly CoffeeMenuService _menuService;

        public CoffeeMenuViewModel(CoffeeMenuService menuService)
        {
            _menuService = menuService;
            MenuItems = new();
            LoadMenu();
        }

        public ObservableCollection<CoffeeDrink> MenuItems { get; }

        [ObservableProperty]
        private bool _fromSearch;

        [ObservableProperty]
        private bool _searching;

        [ObservableProperty]
        private string _category = "All";

        private string _displayTitle = "Coffee Menu";

        public string DisplayTitle
        {
            get => _displayTitle;
            set
            {
                if (SetProperty(ref _displayTitle, value))
                {
                    OnPropertyChanged(nameof(CategoryTitle));
                }
            }
        }

        public string CategoryTitle
        {
            get
            {
                if (FromSearch)
                {
                    return DisplayTitle;
                }

                var normalizedCategory = NormalizeCategory(Category);
                if (IsFreshBrewsCategory(normalizedCategory))
                {
                    return FreshBrewsDisplayTitle;
                }

                if (string.IsNullOrWhiteSpace(normalizedCategory) ||
                    string.Equals(normalizedCategory, "All", StringComparison.OrdinalIgnoreCase))
                {
                    return DisplayTitle;
                }

                return normalizedCategory!;
            }
        }

        public bool IsSearchBarVisible => !IsFreshBrewsCategory(Category);

        private static bool IsFreshBrewsCategory(string? value)
        {
            var normalized = NormalizeCategory(value);
            return !string.IsNullOrWhiteSpace(normalized) &&
                   (string.Equals(normalized, FreshBrewsDisplayTitle, StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(normalized, FreshBrewsFilter, StringComparison.OrdinalIgnoreCase));
        }

        private static string? NormalizeCategory(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return value;
            }

            var trimmed = value.Trim().Replace('+', ' ');
            try
            {
                return Uri.UnescapeDataString(trimmed);
            }
            catch (UriFormatException)
            {
                return trimmed;
            }
        }

        private string? GetResolvedCategory()
        {
            var normalized = NormalizeCategory(Category);
            return IsFreshBrewsCategory(normalized) ? FreshBrewsFilter : normalized;
        }

        private IEnumerable<CoffeeDrink> GetMenuDrinks(string? searchTerm)
        {
            if (IsFreshBrewsCategory(Category))
            {
                return DrinkCategories
                    .SelectMany(category => _menuService.SearchMenu(searchTerm, category))
                    .DistinctBy(drink => drink.Name);
            }

            return _menuService.SearchMenu(searchTerm, GetResolvedCategory());
        }

        partial void OnCategoryChanged(string value)
        {
            OnPropertyChanged(nameof(CategoryTitle));
            OnPropertyChanged(nameof(IsSearchBarVisible));
            LoadMenu();
        }

        partial void OnFromSearchChanged(bool value) => OnPropertyChanged(nameof(CategoryTitle));

        private void LoadMenu(string? searchTerm = null)
        {
            MenuItems.Clear();
            foreach (var drink in GetMenuDrinks(searchTerm))
            {
                MenuItems.Add(drink);
            }
        }

        [RelayCommand]
        private async Task SearchMenu(string searchTerm)
        {
            MenuItems.Clear();
            Searching = true;
            await Task.Delay(300);

            foreach (var drink in GetMenuDrinks(searchTerm))
            {
                MenuItems.Add(drink);
            }

            Searching = false;
        }

        [RelayCommand]
        private async Task GoToDrinkDetails(CoffeeDrink drink)
        {
            var parameters = new Dictionary<string, object>
            {
                [nameof(CoffeeDetailsViewModel.Drink)] = drink
            };

            await Shell.Current.GoToAsync(nameof(CoffeeDetailPage), animate: true, parameters);
        }
    }
}

