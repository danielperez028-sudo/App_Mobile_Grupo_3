using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;


namespace ReadBuddy.ViewModels;

public partial class VocabularyViewModel : BaseViewModel
{
    [ObservableProperty] private List<VocabularyWord> words = new();
    [ObservableProperty] private string searchText = string.Empty;

    partial void OnSearchTextChanged(string value) => Refresh();

    public void Refresh()
    {
        var query = AppDataService.VocabularyWords.AsEnumerable();

        var text = SearchText?.Trim() ?? string.Empty;
        if (text.Length > 0)
            query = query.Where(v => v.Term.Contains(text, StringComparison.OrdinalIgnoreCase));

        // Alphabetical, like a real dictionary
        Words = query.OrderBy(v => v.Term, StringComparer.CurrentCultureIgnoreCase).ToList();
    }

    [RelayCommand]
    private Task AddAsync() => Shell.Current.GoToAsync(Routes.VocabularyForm);

    [RelayCommand]
    private async Task EditAsync(VocabularyWord? word)
    {
        if (word is null) return;
        await Shell.Current.GoToAsync(Routes.VocabularyForm,
            new Dictionary<string, object> { [Routes.VocabularyParam] = word });
    }

    [RelayCommand]
    private async Task DeleteAsync(VocabularyWord? word)
    {
        if (word is null) return;

        if (!await ConfirmAsync("Eliminar palabra",
                $"¿Eliminar \"{word.Term}\" del diccionario? Esta acción no se puede deshacer.")) return;

        AppDataService.DeleteVocabularyWord(word);
        Refresh();
        await ShowSuccessAsync("Palabra eliminada", "La palabra se eliminó correctamente.");
    }

    [RelayCommand]
    private Task BackAsync() => Shell.Current.GoToAsync("..");
}