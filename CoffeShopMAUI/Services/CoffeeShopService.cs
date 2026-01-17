namespace CoffeShopMAUI.Services
{
    public class CoffeeMenuService
    {
        private static readonly IEnumerable<CoffeeDrink> _drinks = new List<CoffeeDrink>
        {
            new() { Name = "Espresso", Image = "espresso", Price = 2.50, Category = "Hot drinks", Description = "Concentrated shot of rich coffee with a bold flavor." },
            new() { Name = "Cappuccino", Image = "cappuccino", Price = 3.10, Category = "Hot drinks", Description = "Espresso topped with steamed milk and thick milk foam." },
            new() { Name = "Latte", Image = "latte", Price = 3.40, Category = "Hot drinks", Description = "Smooth espresso blended with plenty of steamed milk." },
            new() { Name = "Mocha", Image = "mocha", Price = 3.80, Category = "Hot drinks", Description = "Chocolate-infused espresso with steamed milk and cocoa." },
            new() { Name = "Flat White", Image = "flat_white", Price = 3.65, Category = "Hot drinks", Description = "Velvety microfoam over a double shot of espresso." },
            new() { Name = "Americano", Image = "americano", Price = 2.95, Category = "Hot drinks", Description = "Espresso diluted with hot water for a smooth, long drink." },
            new() { Name = "Green Tea", Image = "green_tea", Price = 2.75, Category = "Hot drinks", Description = "Freshly brewed green tea with delicate herbal notes." },
            new() { Name = "Chai Latte", Image = "chai_latte", Price = 3.25, Category = "Hot drinks", Description = "Spiced black tea blended with steamed milk." },
            new() { Name = "Iced Latte", Image = "iced_latte", Price = 4.10, Category = "Cold drinks", Description = "Chilled espresso over ice with cold milk." },
            new() { Name = "Cold Brew", Image = "cold_brew", Price = 4.20, Category = "Cold drinks", Description = "Slow-steeped coffee served cold, smooth and low-acid." },
            new() { Name = "Nitro Brew", Image = "nitro_brew", Price = 4.95, Category = "Cold drinks", Description = "Nitrogen-infused cold brew with a creamy, cascading head." },
            new() { Name = "Bottled Water", Image = "bottled_water", Price = 1.75, Category = "Cold drinks", Description = "Refreshing still water." },
            new() { Name = "Sparkling Water", Image = "sparkling_water", Price = 2.10, Category = "Cold drinks", Description = "Carbonated mineral water with crisp bubbles." },
            new() { Name = "Orange Juice", Image = "orange_juice", Price = 3.05, Category = "Cold drinks", Description = "Freshly squeezed orange juice, naturally sweet and tart." },
            new() { Name = "Blueberry Muffin", Image = "blueberry_muffin", Price = 2.90, Category = "Food", Description = "Soft muffin packed with juicy blueberries." },
            new() { Name = "Chocolate Croissant", Image = "chocolatecroissant", Price = 3.50, Category = "Food", Description = "Flaky croissant filled with rich dark chocolate." },
            new() { Name = "Bagel & Cream Cheese", Image = "bagel_cream_cheese", Price = 3.20, Category = "Food", Description = "Toasted bagel served with smooth cream cheese." },
            new() { Name = "Ham & Cheese Sandwich", Image = "ham_cheese_sandwich", Price = 5.50, Category = "Food", Description = "Toasted sourdough loaded with smoked ham, melted cheddar, and a hint of mustard." },
            new() { Name = "Turkey Wrap", Image = "turkeywrap", Price = 5.95, Category = "Food", Description = "Whole-wheat wrap filled with roast turkey, crisp veggies, and herbed yogurt sauce." }
        };

        public IEnumerable<CoffeeDrink> GetAllDrinks() => _drinks;

        public IEnumerable<CoffeeDrink> GetByCategory(string? category)
        {
            if (string.IsNullOrWhiteSpace(category) || category == "All")
            {
                return _drinks;
            }

            return _drinks.Where(d => d.Category.Equals(category, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<CoffeeDrink> GetFeaturedDrinks(int count = 6) =>
            _drinks.OrderBy(_ => Guid.NewGuid()).Take(count);

        public IEnumerable<CoffeeDrink> SearchMenu(string? searchTerm, string? category = null)
        {
            var source = GetByCategory(category);

            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return source;
            }

            return source.Where(d => d.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }
    }
}

