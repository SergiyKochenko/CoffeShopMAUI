using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.Services;

public static class AdminAccessService
{
    private const string AdminFlagKey = "IsAdminAuthenticated";
    private const string AdminPasscode = "BREWMASTER2024";
    private const string AdminNameKey = "ActiveAdminName";
    private const string AdminPhoneKey = "ActiveAdminPhone";

    public static string Passcode => AdminPasscode;

    public static event EventHandler<bool>? AccessChanged;

    public static bool HasAccess => Preferences.Default.Get(AdminFlagKey, false);

    public static bool TryAuthenticate(string? passcode)
    {
        if (string.IsNullOrWhiteSpace(passcode))
        {
            return false;
        }

        if (!string.Equals(passcode.Trim(), AdminPasscode, StringComparison.Ordinal))
        {
            return false;
        }

        Preferences.Default.Set(AdminFlagKey, true);
        OnAccessChanged(true);
        return true;
    }

    public static void RecordActiveAdmin(string name, string phone)
    {
        Preferences.Default.Set(AdminNameKey, name);
        Preferences.Default.Set(AdminPhoneKey, phone);
    }

    public static (string Name, string Phone) GetActiveAdmin()
    {
        var name = Preferences.Default.Get(AdminNameKey, string.Empty);
        var phone = Preferences.Default.Get(AdminPhoneKey, string.Empty);
        return (name, phone);
    }

    public static void RevokeAccess()
    {
        if (!HasAccess)
        {
            return;
        }

        Preferences.Default.Remove(AdminFlagKey);
        Preferences.Default.Remove(AdminNameKey);
        Preferences.Default.Remove(AdminPhoneKey);
        OnAccessChanged(false);
    }

    private static void OnAccessChanged(bool state) => MainThread.BeginInvokeOnMainThread(() => AccessChanged?.Invoke(null, state));
}
