using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

public partial class AdminDashboardViewModel : BaseViewModel
{
    // Live collection: new students appear without any refresh code
    public ObservableCollection<Student> Students => AppDataService.Students;

    [RelayCommand]
    private void OpenMenu() => Shell.Current.FlyoutIsPresented = true;

    [RelayCommand] private Task AddStoryAsync() => Shell.Current.GoToAsync(Routes.StoryForm);
    [RelayCommand] private Task AddQuestionAsync() => Shell.Current.GoToAsync(Routes.QuestionForm);
    [RelayCommand] private Task AddVocabularyAsync() => Shell.Current.GoToAsync(Routes.VocabularyForm);

    [RelayCommand] private Task ManageStoriesAsync() => Shell.Current.GoToAsync(Routes.ManageStories);
    [RelayCommand] private Task ManageQuestionsAsync() => Shell.Current.GoToAsync(Routes.ManageQuestions);
    [RelayCommand] private Task ManageVocabularyAsync() => Shell.Current.GoToAsync(Routes.ManageVocabulary);
}