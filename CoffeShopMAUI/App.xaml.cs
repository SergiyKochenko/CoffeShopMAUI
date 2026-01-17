using CoffeShopMAUI.Pages;

namespace CoffeShopMAUI;

public partial class App : Application
{
    public App()
    {
#if WINDOWS || ANDROID || IOS || MACCATALYST
        InitializeComponent();
#endif
        MainPage = new CoffeShopMAUI.Pages.MainPage();
    }
}
