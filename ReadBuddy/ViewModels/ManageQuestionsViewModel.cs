using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

public partial class ManageQuestionsViewModel : BaseViewModel
{
    [ObservableProperty] private List<Question> questions = new();
    [ObservableProperty] private string searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => Refresh();

    public void Refresh()
    {
        var query = AppDataService.Questions.AsEnumerable();

        var text = SearchText?.Trim() ?? string.Empty;
        if (text.Length > 0)
            query = query.Where(q =>
                q.Text.Contains(text, StringComparison.OrdinalIgnoreCase) ||
                q.StoryTitle.Contains(text, StringComparison.OrdinalIgnoreCase));

        Questions = query.ToList();
    }

    [RelayCommand]
    private Task AddAsync() => Shell.Current.GoToAsync(Routes.QuestionForm);

    [RelayCommand]
    private async Task EditAsync(Question? question)
    {
        if (question is null) return;
        await Shell.Current.GoToAsync(Routes.QuestionForm,
            new Dictionary<string, object> { [Routes.QuestionParam] = question });
    }

    [RelayCommand]
    private async Task DeleteAsync(Question? question)
    {
        if (question is null) return;

        if (!await ConfirmAsync("Eliminar pregunta",
                $"¿Eliminar \"{question.Text}\"? Esta acción no se puede deshacer.")) return;

        AppDataService.DeleteQuestion(question);
        Refresh();
        await ShowSuccessAsync("Pregunta eliminada", "La pregunta se eliminó correctamente.");
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}