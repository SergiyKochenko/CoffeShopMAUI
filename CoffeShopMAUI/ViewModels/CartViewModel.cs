using System;
namespace CoffeShopMAUI.ViewModels
{
    public partial class CartViewModel : ObservableObject
    {
        public event EventHandler<CoffeeDrink>? CartItemRemoved;
        public event EventHandler<CoffeeDrink>? CartItemUpdated;
        public event EventHandler? CartCleared;

        public ObservableCollection<CoffeeDrink> Items { get; } = new();

        [ObservableProperty]
        private double _totalAmount;

        private void RecalculateTotalAmount() => TotalAmount = Items.Sum(i => i.Amount);

        [RelayCommand]
        private void UpdateCartItem(CoffeeDrink drink)
        {
            var item = Items.FirstOrDefault(i => i.Name == drink.Name);
            if (item is not null)
            {
                item.CartQuantity = drink.CartQuantity;
            }
            else
            {
                Items.Add(drink.Clone());
            }

            RecalculateTotalAmount();
        }

        [RelayCommand]
        private async void RemoveCartItem(string name)
        {
            var item = Items.FirstOrDefault(i => i.Name == name);
            if (item is null)
            {
                return;
            }

            Items.Remove(item);
            RecalculateTotalAmount();
            CartItemRemoved?.Invoke(this, item);

            var snackbarOptions = new SnackbarOptions
            {
                CornerRadius = 10,
                BackgroundColor = Colors.PaleGoldenrod
            };

            var snackbar = Snackbar.Make($"'{item.Name}' removed from cart",
                () =>
                {
                    Items.Add(item);
                    RecalculateTotalAmount();
                    CartItemUpdated?.Invoke(this, item);
                },
                "Undo",
                TimeSpan.FromSeconds(5),
                snackbarOptions);

            await snackbar.Show();
        }

        [RelayCommand]
        private async Task ClearCart()
        {
            if (await Shell.Current.DisplayAlert("Clear Cart?", "Do you really want to remove all drinks?", "Yes", "No"))
            {
                Items.Clear();
                RecalculateTotalAmount();
                CartCleared?.Invoke(this, EventArgs.Empty);
                await Toast.Make("Cart cleared", ToastDuration.Short).Show();
            }
        }

        [RelayCommand]
        private async Task PlaceOrder()
        {
            if (!Items.Any())
            {
                await Toast.Make("Your cart is empty", ToastDuration.Short).Show();
                return;
            }

            await Shell.Current.GoToAsync(nameof(CheckoutPage), animate: true);
        }

        public void CompleteCheckout()
        {
            Items.Clear();
            RecalculateTotalAmount();
            CartCleared?.Invoke(this, EventArgs.Empty);
        }
    }
}

