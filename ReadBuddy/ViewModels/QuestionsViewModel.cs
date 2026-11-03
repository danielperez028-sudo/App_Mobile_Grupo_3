using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

[QueryProperty(nameof(StoryId), "storyId")]
[QueryProperty(nameof(StoryTitle), "storyTitle")]
public partial class QuestionsViewModel : BaseViewModel
{
    [ObservableProperty] private string storyId = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(StoryTitleText))]
    private string storyTitle = string.Empty;

    [ObservableProperty] private List<QuestionItemViewModel> questions = new();

    public string StoryTitleText => StoryTitle.Length == 0
        ? "Historia: Cargando..."
        : $"Historia: {Uri.UnescapeDataString(StoryTitle)}";

    // Shell sets StoryId when the page opens: load that story's questions
    partial void OnStoryIdChanged(string value)
    {
        Questions = AppDataService.Questions
            .Where(q => q.StoryId == value)
            .Select(q => new QuestionItemViewModel(q))
            .ToList();
    }

    [RelayCommand]
    private Task SubmitAsync() => Shell.Current.GoToAsync("..");
}