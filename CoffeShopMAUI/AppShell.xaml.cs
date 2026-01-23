using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
using CoffeShopMAUI;

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
        Routing.RegisterRoute(nameof(AdminDashboardPage), typeof(AdminDashboardPage));
        Routing.RegisterRoute(nameof(AdminCustomerOrdersPage), typeof(AdminCustomerOrdersPage));

        UpdatePageTitle();
        UpdateAdminBadge(AdminAccessService.HasAccess);

        AdminAccessService.AccessChanged += OnAdminAccessChanged;
        Navigated += OnShellNavigated;

#if ANDROID || IOS
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.DarkGoldenrod,
            StatusBarStyle = StatusBarStyle.LightContent
        });
#endif
    }

    private void OnShellNavigated(object? sender, ShellNavigatedEventArgs e)
    {
        UpdatePageTitle();
        UpdateAdminBadge(AdminAccessService.HasAccess);
    }

    private void UpdatePageTitle()
    {
        if (PageTitleLabel is null)
        {
            return;
        }

        PageTitleLabel.Text = CurrentPage?.Title ?? "Coffee House";
    }

    private void UpdateAdminBadge(bool visible)
    {
        if (AdminBadge is null)
        {
            return;
        }

        var isDashboard = CurrentPage is AdminDashboardPage;
        AdminBadge.IsVisible = visible && !isDashboard;
    }

    private void OnAdminAccessChanged(object? sender, bool hasAccess) => UpdateAdminBadge(hasAccess);

    private async void AdminBadge_Tapped(object? sender, TappedEventArgs e)
    {
        if (!AdminAccessService.HasAccess)
        {
            return;
        }

        await GoToAsync(nameof(AdminDashboardPage), animate: true);
    }

    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        if (Handler is null)
        {
            AdminAccessService.AccessChanged -= OnAdminAccessChanged;
            Navigated -= OnShellNavigated;
        }
    }
}
