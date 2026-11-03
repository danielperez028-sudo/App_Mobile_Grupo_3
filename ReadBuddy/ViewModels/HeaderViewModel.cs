using CommunityToolkit.Mvvm.Input;

namespace ReadBuddy.ViewModels;

public partial class HeaderViewModel : BaseViewModel
{
    [RelayCommand]
    private void OpenMenu() => Shell.Current.FlyoutIsPresented = true;
}