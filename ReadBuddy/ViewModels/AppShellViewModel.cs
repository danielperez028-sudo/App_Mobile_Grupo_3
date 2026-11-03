using CommunityToolkit.Mvvm.Input;

namespace ReadBuddy.ViewModels;

public partial class AppShellViewModel : BaseViewModel
{
    [RelayCommand]
    private async Task LogoutAsync()
    {
        var confirm = await Shell.Current.DisplayAlertAsync(
            "Cerrar sesión", "¿Seguro que quieres salir?", "Sí", "No");

        if (!confirm) return;

        Shell.Current.FlyoutIsPresented = false;
        await Shell.Current.GoToAsync(Routes.Login);
    }
}