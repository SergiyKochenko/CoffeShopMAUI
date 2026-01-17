namespace CoffeShopMAUI.ViewModels
{
    [QueryProperty(nameof(FromSearch), nameof(FromSearch))]
    [QueryProperty(nameof(Category), nameof(Category))]
    [QueryProperty(nameof(DisplayTitle), nameof(DisplayTitle))]
    public partial class CoffeeMenuViewModel : ObservableObject
    {
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

        public string CategoryTitle => string.IsNullOrWhiteSpace(Category) || Category == "All" ? DisplayTitle : Category;

        partial void OnCategoryChanged(string value)
        {
            OnPropertyChanged(nameof(CategoryTitle));
            LoadMenu();
        }

        private void LoadMenu(string? searchTerm = null)
        {
            MenuItems.Clear();
            foreach (var drink in _menuService.SearchMenu(searchTerm, Category))
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

            foreach (var drink in _menuService.SearchMenu(searchTerm, Category))
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

