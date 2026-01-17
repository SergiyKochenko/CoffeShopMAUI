#if IOS
using UIKit;
#endif
#if ANDROID || IOS
using CommunityToolkit.Maui.Behaviors;
using CommunityToolkit.Maui.Core;
#endif

namespace CoffeShopMAUI.Pages;

public partial class CoffeeDetailPage : ContentPage
{
    private readonly CoffeeDetailsViewModel _detailsViewModel;

    public CoffeeDetailPage(CoffeeDetailsViewModel detailsViewModel)
    {
        _detailsViewModel = detailsViewModel;
        InitializeComponent();
        BindingContext = _detailsViewModel;

#if ANDROID || IOS
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.White,
            StatusBarStyle = StatusBarStyle.DarkContent
        });
#endif
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
#if IOS
        var bottom = UIApplication.SharedApplication.Delegate.GetWindow().SafeAreaInsets.Bottom;
        bottomBox.Margin = new Thickness(-1, 0, -1, (bottom + 1) * -1);
#endif
    }

    async void ImageButton_Clicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..", animate: true);
    }

    protected override void OnNavigatingFrom(NavigatingFromEventArgs args)
    {
        base.OnNavigatingFrom(args);
#if ANDROID || IOS
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.DarkGoldenrod,
            StatusBarStyle = StatusBarStyle.LightContent
        });
#endif
    }
}
