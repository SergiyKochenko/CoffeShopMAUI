using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;

namespace CoffeShopMAUI;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        Routing.RegisterRoute(nameof(CartPage), typeof(CartPage));
        Routing.RegisterRoute(nameof(CheckoutPage), typeof(CheckoutPage));
        Routing.RegisterRoute(nameof(CoffeeMenuPage), typeof(CoffeeMenuPage));
        Routing.RegisterRoute(nameof(CoffeeDetailPage), typeof(CoffeeDetailPage));
        Routing.RegisterRoute(nameof(OrderHistoryPage), typeof(OrderHistoryPage));
        Routing.RegisterRoute(nameof(OrderReceiptPage), typeof(OrderReceiptPage));

#if ANDROID || IOS
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.DarkGoldenrod,
            StatusBarStyle = StatusBarStyle.LightContent
        });
#endif
    }
}
