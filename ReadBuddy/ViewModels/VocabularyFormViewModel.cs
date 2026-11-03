using System.Text.RegularExpressions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

[QueryProperty(nameof(WordToEdit), Routes.VocabularyParam)]
public partial class VocabularyFormViewModel : BaseViewModel
{
    // Letters (with accents), spaces and hyphens, starting with a letter
    private static readonly Regex TermPattern = new(@"^\p{L}[\p{L}\s\-]*$");

    private VocabularyWord? _editing;
    private bool _showErrors;   // live validation starts after the first Save attempt

    // Set by Shell when the form is opened from "Edit"
    public VocabularyWord? WordToEdit
    {
        get => _editing;
        set { if (value is not null) LoadForEditing(value); }
    }

    // ----- Fields -----
    [ObservableProperty] private string term = string.Empty;
    [ObservableProperty] private string definition = string.Empty;
    [ObservableProperty] private string example = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormTitle), nameof(SaveButtonText))]
    private bool isEditing;

    public string FormTitle => IsEditing ? "Editar palabra" : "Nueva Palabra de Vocabulario";
    public string SaveButtonText => IsEditing ? "Guardar cambios" : "Guardar Palabra";

    // ----- Inline errors -----
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasTermError))] private string termError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasDefinitionError))] private string definitionError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasExampleError))] private string exampleError = string.Empty;

    public bool HasTermError => TermError.Length > 0;
    public bool HasDefinitionError => DefinitionError.Length > 0;
    public bool HasExampleError => ExampleError.Length > 0;

    partial void OnTermChanged(string value) { if (_showErrors) ValidateTerm(); }
    partial void OnDefinitionChanged(string value) { if (_showErrors) ValidateDefinition(); }
    partial void OnExampleChanged(string value) { if (_showErrors) ValidateExample(); }

    // ----- Validation -----
    private void ValidateTerm()
    {
        var value = Term?.Trim() ?? string.Empty;
        var currentId = _editing?.Id ?? 0;

        if (value.Length == 0)
            TermError = "La palabra es obligatoria.";
        else if (value.Length < 2)
            TermError = "La palabra debe tener al menos 2 caracteres.";
        else if (value.Length > 30)
            TermError = "La palabra no puede superar los 30 caracteres.";
        else if (!TermPattern.IsMatch(value))
            TermError = "La palabra solo puede contener letras, espacios y guiones.";
        else if (AppDataService.VocabularyWords.Any(w => w.Id != currentId &&
                 w.Term.Equals(value, StringComparison.OrdinalIgnoreCase)))
            TermError = "Esa palabra ya está en el diccionario.";
        else
            TermError = string.Empty;
    }

    private void ValidateDefinition()
    {
        var value = Definition?.Trim() ?? string.Empty;

        if (value.Length == 0)
            DefinitionError = "La definición es obligatoria.";
        else if (value.Length < 10)
            DefinitionError = $"La definición es muy corta ({value.Length}/10 caracteres como mínimo).";
        else if (value.Length > 300)
            DefinitionError = "La definición no puede superar los 300 caracteres.";
        else
            DefinitionError = string.Empty;
    }

    private void ValidateExample()
    {
        var value = Example?.Trim() ?? string.Empty;

        // The example is optional, so empty is fine
        if (value.Length == 0)
            ExampleError = string.Empty;
        else if (value.Length < 10)
            ExampleError = "El ejemplo debe tener al menos 10 caracteres (o déjalo vacío).";
        else if (value.Length > 200)
            ExampleError = "El ejemplo no puede superar los 200 caracteres.";
        else
            ExampleError = string.Empty;
    }

    private bool Validate()
    {
        ValidateTerm();
        ValidateDefinition();
        ValidateExample();

        return new[] { TermError, DefinitionError, ExampleError }.All(string.IsNullOrEmpty);
    }

    // ----- Helpers -----
    private void LoadForEditing(VocabularyWord word)
    {
        _editing = word;
        IsEditing = true;

        Term = word.Term;
        Definition = word.Definition;
        Example = word.Example;
    }

    private void ResetForm()
    {
        _showErrors = false;
        _editing = null;
        IsEditing = false;

        Term = string.Empty;
        Definition = string.Empty;
        Example = string.Empty;

        TermError = DefinitionError = ExampleError = string.Empty;
    }

    // ----- Commands -----
    [RelayCommand]
    private async Task SaveAsync()
    {
        _showErrors = true;
        if (!Validate()) return;   // errors are already inline, no popup

        var word = new VocabularyWord
        {
            Term = Term.Trim(),
            Definition = Definition.Trim(),
            Example = Example.Trim()
        };

        var wasEditing = _editing is not null;

        if (_editing is null)
        {
            AppDataService.AddVocabularyWord(word);
        }
        else
        {
            word.Id = _editing.Id;
            AppDataService.UpdateVocabularyWord(word);
        }

        await ShowSuccessAsync(
            wasEditing ? "¡Cambios guardados!" : "¡Guardado!",
            wasEditing ? "La palabra se actualizó correctamente."
                       : "La palabra ha sido agregada exitosamente al diccionario.");

        ResetForm();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private Task CancelAsync() => Shell.Current.GoToAsync("..");
}