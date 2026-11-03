using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

public partial class StoriesViewModel : BaseViewModel
{
    private readonly bool _onlyActive;

    // onlyActive = true for the student catalog (hidden stories are not shown)
    public StoriesViewModel(bool onlyActive = false) => _onlyActive = onlyActive;

    [ObservableProperty] private List<Story> stories = new();
    [ObservableProperty] private string searchText = string.Empty;
    [ObservableProperty] private string selectedLevel = "Todas";

    partial void OnSearchTextChanged(string value) => Refresh();
    partial void OnSelectedLevelChanged(string value) => Refresh();

    public void Refresh()
    {
        var query = AppDataService.Stories.AsEnumerable();

        if (_onlyActive)
            query = query.Where(s => s.IsActive);

        if (!SelectedLevel.Equals("Todas", StringComparison.OrdinalIgnoreCase))
            query = query.Where(s => s.Level.Equals(SelectedLevel, StringComparison.OrdinalIgnoreCase));

        var text = SearchText?.Trim() ?? string.Empty;
        if (text.Length > 0)
            query = query.Where(s => s.Title.Contains(text, StringComparison.OrdinalIgnoreCase));

        Stories = query.ToList();
    }

    [RelayCommand]
    private void Filter(string level) => SelectedLevel = level;

    [RelayCommand]
    private async Task OpenQuestionsAsync(Story? story)
    {
        if (story is null) return;
        await Shell.Current.GoToAsync(
            $"story-questions?storyId={story.Id}&storyTitle={Uri.EscapeDataString(story.Title)}");
    }

    [RelayCommand]
    private Task AddAsync() => Shell.Current.GoToAsync(Routes.StoryForm);

    [RelayCommand]
    private async Task EditAsync(Story? story)
    {
        if (story is null) return;
        await Shell.Current.GoToAsync(Routes.StoryForm,
            new Dictionary<string, object> { [Routes.StoryParam] = story });
    }

    [RelayCommand]
    private async Task DeleteAsync(Story? story)
    {
        if (story is null) return;

        var count = AppDataService.CountQuestionsForStory(story.Id);
        var warning = count > 0
            ? $"Se eliminarán también sus {count} pregunta(s). ¿Continuar?"
            : "Esta acción no se puede deshacer. ¿Continuar?";

        if (!await ConfirmAsync($"Eliminar \"{story.Title}\"", warning)) return;

        AppDataService.DeleteStory(story);
        Refresh();
        await ShowSuccessAsync("Historia eliminada", "La historia se eliminó correctamente.");
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}