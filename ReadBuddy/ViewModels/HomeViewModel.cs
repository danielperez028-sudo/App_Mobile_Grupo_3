using CommunityToolkit.Mvvm.Input;

namespace ReadBuddy.ViewModels;

public partial class HomeViewModel : BaseViewModel
{
    [RelayCommand]
    private void OpenMenu() => Shell.Current.FlyoutIsPresented = true;

    [RelayCommand]
    private Task OpenStoriesAsync() => Shell.Current.GoToAsync(Routes.StudentStories);

    [RelayCommand]
    private Task OpenVocabularyAsync() => Shell.Current.GoToAsync(Routes.StudentVocabulary);
}