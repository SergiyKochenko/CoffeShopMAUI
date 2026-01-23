using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Storage;

namespace CoffeShopMAUI.Services;

public static class AdminAccessService
{
    private const string AdminFlagKey = "IsAdminAuthenticated";
    private const string AdminPasscode = "BREWMASTER2024";

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

    public static void RevokeAccess()
    {
        if (!HasAccess)
        {
            return;
        }

        Preferences.Default.Remove(AdminFlagKey);
        OnAccessChanged(false);
    }

    private static void OnAccessChanged(bool state) => MainThread.BeginInvokeOnMainThread(() => AccessChanged?.Invoke(null, state));
}
