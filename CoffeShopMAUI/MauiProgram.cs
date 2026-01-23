using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI;

public static class MauiProgram
{
    public static IServiceProvider Services { get; private set; } = default!;

    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            })
            .UseMauiCommunityToolkit();

#if DEBUG
        builder.Logging.AddDebug();
#endif
        AddCoffeeServices(builder.Services);

        var app = builder.Build();
        Services = app.Services;
        return app;
    }

    private static IServiceCollection AddCoffeeServices(IServiceCollection services)
    {
        // Register existing services and pages/viewmodels used in the project
        services.AddSingleton<CoffeeMenuService>();
        services.AddSingleton(sp => new OrderStorageService(FileSystem.Current.AppDataDirectory));

        services.AddSingleton<HomePage>()
                .AddSingleton<HomeViewModel>();

        services.AddTransientWithShellRoute<CoffeeMenuPage, CoffeeMenuViewModel>(nameof(CoffeeMenuPage));
        services.AddTransientWithShellRoute<CoffeeDetailPage, CoffeeDetailsViewModel>(nameof(CoffeeDetailPage));

        services.AddSingleton<CartViewModel>();
        services.AddTransient<CartPage>();

        services.AddTransientWithShellRoute<CheckoutPage, CheckoutViewModel>(nameof(CheckoutPage));
        services.AddTransientWithShellRoute<OrderHistoryPage, OrderHistoryViewModel>(nameof(OrderHistoryPage));
        services.AddTransientWithShellRoute<OrderReceiptPage, OrderReceiptViewModel>(nameof(OrderReceiptPage));
        services.AddTransientWithShellRoute<AccountInfoPage, AccountInfoViewModel>(nameof(AccountInfoPage));
        services.AddTransientWithShellRoute<AdminDashboardPage, AdminDashboardViewModel>(nameof(AdminDashboardPage));
        services.AddTransientWithShellRoute<AdminCustomerOrdersPage, AdminCustomerOrdersViewModel>(nameof(AdminCustomerOrdersPage));

        return services;
    }
}
