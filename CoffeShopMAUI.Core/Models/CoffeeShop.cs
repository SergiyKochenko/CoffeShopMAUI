using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CoffeShopMAUI.Models
{
    public partial class CoffeeDrink : ObservableObject
    {
        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public double Price { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(Amount))]
        private int _cartQuantity;

        public double Amount => CartQuantity * Price;

        public CoffeeDrink Clone() => (CoffeeDrink)MemberwiseClone();
    }
}

