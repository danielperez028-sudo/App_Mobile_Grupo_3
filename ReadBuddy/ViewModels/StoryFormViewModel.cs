using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;
using System.Globalization;


namespace ReadBuddy.ViewModels;

[QueryProperty(nameof(StoryToEdit), Routes.StoryParam)]
public partial class StoryFormViewModel : BaseViewModel
{
    private Story? _editing;
    private bool _showErrors;   // live validation starts after the first Save attempt

    // Set by Shell when the form is opened from "Edit"
    public Story? StoryToEdit
    {
        get => _editing;
        set { if (value is not null) LoadForEditing(value); }
    }

    public IReadOnlyList<string> Levels => StoryOptions.Levels;
    public IReadOnlyList<string> Skills => StoryOptions.Skills;

    // ----- Fields -----
    [ObservableProperty] private string cover = string.Empty;
    [ObservableProperty] private string title = string.Empty;
    [ObservableProperty] private string? level;
    [ObservableProperty] private string? skill;
    [ObservableProperty] private string category = string.Empty;
    [ObservableProperty] private string content = string.Empty;
    [ObservableProperty] private bool isActive = true;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormTitle), nameof(SaveButtonText))]
    private bool isEditing;

    public string FormTitle => IsEditing ? "Editar historia" : "Nueva historia";
    public string SaveButtonText => IsEditing ? "Guardar cambios" : "Guardar";

    // ----- Inline errors -----
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasCoverError))] private string coverError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasTitleError))] private string titleError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasLevelError))] private string levelError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasSkillError))] private string skillError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasCategoryError))] private string categoryError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasContentError))] private string contentError = string.Empty;

    public bool HasCoverError => CoverError.Length > 0;
    public bool HasTitleError => TitleError.Length > 0;
    public bool HasLevelError => LevelError.Length > 0;
    public bool HasSkillError => SkillError.Length > 0;
    public bool HasCategoryError => CategoryError.Length > 0;
    public bool HasContentError => ContentError.Length > 0;

    // Re-validate the field being edited once the user has tried to save
    partial void OnCoverChanged(string value) { if (_showErrors) ValidateCover(); }
    partial void OnTitleChanged(string value) { if (_showErrors) ValidateTitle(); }
    partial void OnLevelChanged(string? value) { if (_showErrors) ValidateLevel(); }
    partial void OnSkillChanged(string? value) { if (_showErrors) ValidateSkill(); }
    partial void OnCategoryChanged(string value) { if (_showErrors) ValidateCategory(); }
    partial void OnContentChanged(string value) { if (_showErrors) ValidateContent(); }

    // ----- Validation -----
    private void ValidateCover()
    {
        var value = Cover?.Trim() ?? string.Empty;

        if (value.Length == 0)
            CoverError = "Elige un emoji para la portada.";
        else if (new StringInfo(value).LengthInTextElements > 1)
            CoverError = "La portada debe ser un solo emoji.";
        else if (char.IsLetterOrDigit(value[0]))
            CoverError = "La portada debe ser un emoji, no una letra o un número.";
        else
            CoverError = string.Empty;
    }

    private void ValidateTitle()
    {
        var value = Title?.Trim() ?? string.Empty;
        var currentId = _editing?.Id ?? 0;

        if (value.Length == 0)
            TitleError = "El título es obligatorio.";
        else if (value.Length < 3)
            TitleError = "El título debe tener al menos 3 caracteres.";
        else if (value.Length > 60)
            TitleError = "El título no puede superar los 60 caracteres.";
        else if (AppDataService.Stories.Any(s => s.Id != currentId &&
                 s.Title.Equals(value, StringComparison.OrdinalIgnoreCase)))
            TitleError = "Ya existe una historia con ese título.";
        else
            TitleError = string.Empty;
    }

    private void ValidateLevel() =>
        LevelError = string.IsNullOrWhiteSpace(Level) ? "Selecciona el nivel de la historia." : string.Empty;

    private void ValidateSkill() =>
        SkillError = string.IsNullOrWhiteSpace(Skill) ? "Selecciona la habilidad que trabaja la historia." : string.Empty;

    private void ValidateCategory()
    {
        var value = Category?.Trim() ?? string.Empty;

        if (value.Length == 0)
            CategoryError = "La categoría es obligatoria (ej. Animales, Aventura).";
        else if (value.Length < 3)
            CategoryError = "La categoría debe tener al menos 3 caracteres.";
        else if (value.Length > 30)
            CategoryError = "La categoría no puede superar los 30 caracteres.";
        else
            CategoryError = string.Empty;
    }

    private void ValidateContent()
    {
        var value = Content?.Trim() ?? string.Empty;

        if (value.Length == 0)
            ContentError = "El contenido de la historia es obligatorio.";
        else if (value.Length < 50)
            ContentError = $"El contenido es muy corto ({value.Length}/50 caracteres como mínimo).";
        else if (value.Length > 5000)
            ContentError = "El contenido no puede superar los 5000 caracteres.";
        else
            ContentError = string.Empty;
    }

    private bool Validate()
    {
        ValidateCover();
        ValidateTitle();
        ValidateLevel();
        ValidateSkill();
        ValidateCategory();
        ValidateContent();

        return new[] { CoverError, TitleError, LevelError, SkillError, CategoryError, ContentError }
            .All(string.IsNullOrEmpty);
    }

    // ----- Helpers -----
    private void LoadForEditing(Story story)
    {
        _editing = story;
        IsEditing = true;

        Cover = story.Cover;
        Title = story.Title;
        Level = story.Level;
        Skill = story.Skill;
        Category = story.Category;
        Content = story.Content;
        IsActive = story.IsActive;
    }

    private void ResetForm()
    {
        _showErrors = false;
        _editing = null;
        IsEditing = false;

        Cover = string.Empty;
        Title = string.Empty;
        Level = null;
        Skill = null;
        Category = string.Empty;
        Content = string.Empty;
        IsActive = true;

        CoverError = TitleError = LevelError = SkillError = CategoryError = ContentError = string.Empty;
    }

    // ----- Commands -----
    [RelayCommand]
    private async Task SaveAsync()
    {
        _showErrors = true;
        if (!Validate()) return;   // errors are already inline, no popup

        var story = new Story
        {
            Cover = Cover.Trim(),
            Title = Title.Trim(),
            Level = Level!,
            Skill = Skill!,
            Category = Category.Trim(),
            Content = Content.Trim(),
            IsActive = IsActive
        };

        var wasEditing = _editing is not null;

        if (_editing is null)
        {
            AppDataService.AddStory(story);
        }
        else
        {
            story.Id = _editing.Id;
            AppDataService.UpdateStory(story);
        }

        await ShowSuccessAsync(
            wasEditing ? "¡Cambios guardados!" : "¡Historia guardada!",
            wasEditing ? "La historia se actualizó correctamente."
                       : "La historia se creó exitosamente.");

        ResetForm();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private Task CancelAsync() => Shell.Current.GoToAsync("..");
}