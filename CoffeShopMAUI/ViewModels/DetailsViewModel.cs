using System;


namespace CoffeShopMAUI.ViewModels
{
    [QueryProperty(nameof(Drink), nameof(Drink))]
    public partial class CoffeeDetailsViewModel : ObservableObject, IDisposable
    {
        private readonly CartViewModel _cartViewModel;
        private bool _disposed;

        public CoffeeDetailsViewModel(CartViewModel cartViewModel)
        {
            _cartViewModel = cartViewModel;
            _cartViewModel.CartCleared += OnCartCleared;
            _cartViewModel.CartItemRemoved += OnCartItemRemoved;
            _cartViewModel.CartItemUpdated += OnCartItemUpdated;
        }

        private void OnCartCleared(object? _, EventArgs __)
        {
            if (Drink is null) return;
            Drink.CartQuantity = 0;
        }

        private void OnCartItemRemoved(object? _, CoffeeDrink drink) => OnCartItemChanged(drink, 0);

        private void OnCartItemUpdated(object? _, CoffeeDrink drink) => OnCartItemChanged(drink, drink.CartQuantity);

        private void OnCartItemChanged(CoffeeDrink drink, int quantity)
        {
            if (Drink is null || drink.Name != Drink.Name) return;
            Drink.CartQuantity = quantity;
        }

        [ObservableProperty]
        private CoffeeDrink? _drink;

        [RelayCommand]
        private void AddToCart()
        {
            if (Drink is null) return;
            Drink.CartQuantity++;
            _cartViewModel.UpdateCartItemCommand.Execute(Drink);
        }

        [RelayCommand]
        private void RemoveFromCart()
        {
                if (Drink is null || Drink.CartQuantity <= 0) return;
            Drink.CartQuantity--;
            _cartViewModel.UpdateCartItemCommand.Execute(Drink);
        }

        [RelayCommand]
        private async Task ViewCart()
        {
            if (Drink is null) return;
            if (Drink.CartQuantity > 0)
                await Shell.Current.GoToAsync(nameof(CartPage), animate: true);
            else
                await Toast.Make("Please select the quantity using the plus (+) button", ToastDuration.Short).Show();
        }

        public void Dispose()
        {
            if (_disposed) return;
            _cartViewModel.CartCleared -= OnCartCleared;
            _cartViewModel.CartItemRemoved -= OnCartItemRemoved;
            _cartViewModel.CartItemUpdated -= OnCartItemUpdated;
            _disposed = true;
        }
    }
}

