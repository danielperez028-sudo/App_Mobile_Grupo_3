using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

[QueryProperty(nameof(QuestionToEdit), Routes.QuestionParam)]
public partial class QuestionFormViewModel : BaseViewModel
{
    private const int MaxOptions = 6;

    private Question? _editing;
    private bool _showErrors;   // live validation starts after the first Save attempt

    // Set by Shell when the form is opened from "Edit"
    public Question? QuestionToEdit
    {
        get => _editing;
        set { if (value is not null) LoadForEditing(value); }
    }

    public IReadOnlyList<Story> Stories { get; } = AppDataService.Stories.ToList();
    public IReadOnlyList<string> Skills => QuestionOptions.Skills;
    public bool HasNoStories => Stories.Count == 0;

    // ----- Fields -----
    [ObservableProperty] private Story? selectedStory;
    [ObservableProperty] private string questionText = string.Empty;
    [ObservableProperty] private string? skill;
    [ObservableProperty] private string optionsText = string.Empty;
    [ObservableProperty] private string? correctAnswer;
    [ObservableProperty] private List<string> answerChoices = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(FormTitle), nameof(SaveButtonText))]
    private bool isEditing;

    public string FormTitle => IsEditing ? "Editar pregunta" : "Nueva pregunta";
    public string SaveButtonText => IsEditing ? "Guardar cambios" : "Guardar pregunta";

    // ----- Inline errors -----
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasStoryError))] private string storyError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasQuestionError))] private string questionError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasSkillError))] private string skillError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasOptionsError))] private string optionsError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasCorrectAnswerError))] private string correctAnswerError = string.Empty;

    public bool HasStoryError => StoryError.Length > 0;
    public bool HasQuestionError => QuestionError.Length > 0;
    public bool HasSkillError => SkillError.Length > 0;
    public bool HasOptionsError => OptionsError.Length > 0;
    public bool HasCorrectAnswerError => CorrectAnswerError.Length > 0;

    // ----- Reactions to changes -----
    partial void OnSelectedStoryChanged(Story? value)
    {
        if (!_showErrors) return;
        ValidateStory();
        ValidateQuestion();   // the duplicate check depends on the story
    }

    partial void OnQuestionTextChanged(string value)
    {
        if (_showErrors) ValidateQuestion();
    }

    partial void OnSkillChanged(string? value)
    {
        if (_showErrors) ValidateSkill();
    }

    partial void OnOptionsTextChanged(string value)
    {
        var parsed = QuestionOptions.Parse(value);

        // Only rebuild the picker when the options really changed
        // (typing a trailing comma or space should not reset the chosen answer).
        if (!parsed.SequenceEqual(AnswerChoices))
        {
            AnswerChoices = parsed;
            if (CorrectAnswer is not null && !parsed.Contains(CorrectAnswer))
                CorrectAnswer = null;
        }

        if (_showErrors)
        {
            ValidateOptions();
            ValidateCorrectAnswer();
        }
    }

    partial void OnCorrectAnswerChanged(string? value)
    {
        if (_showErrors) ValidateCorrectAnswer();
    }

    // ----- Validation -----
    private void ValidateStory() =>
        StoryError = SelectedStory is null
            ? "Selecciona la historia a la que pertenece la pregunta."
            : string.Empty;

    private void ValidateQuestion()
    {
        var value = QuestionText?.Trim() ?? string.Empty;
        var currentId = _editing?.Id ?? 0;

        if (value.Length == 0)
            QuestionError = "La pregunta es obligatoria.";
        else if (value.Length < 5)
            QuestionError = "La pregunta debe tener al menos 5 caracteres.";
        else if (value.Length > 200)
            QuestionError = "La pregunta no puede superar los 200 caracteres.";
        else if (!value.EndsWith('?'))
            QuestionError = "La pregunta debe terminar con un signo de interrogación (?).";
        else if (SelectedStory is not null && AppDataService.Questions.Any(q =>
                     q.Id != currentId &&
                     q.StoryId == SelectedStory.Id.ToString() &&
                     q.Text.Equals(value, StringComparison.OrdinalIgnoreCase)))
            QuestionError = "Ya existe esa pregunta en esta historia.";
        else
            QuestionError = string.Empty;
    }

    private void ValidateSkill() =>
        SkillError = string.IsNullOrWhiteSpace(Skill)
            ? "Selecciona la habilidad que evalúa la pregunta."
            : string.Empty;

    private void ValidateOptions()
    {
        var options = QuestionOptions.Parse(OptionsText);

        if (options.Count == 0)
            OptionsError = "Escribe las opciones separadas por comas (ej. Azul, Rojo, Verde).";
        else if (options.Count < 2)
            OptionsError = "Agrega al menos 2 opciones separadas por comas.";
        else if (options.Count > MaxOptions)
            OptionsError = $"Máximo {MaxOptions} opciones por pregunta.";
        else if (options.Any(o => o.Length > 50))
            OptionsError = "Cada opción puede tener como máximo 50 caracteres.";
        else if (options.Distinct(StringComparer.OrdinalIgnoreCase).Count() != options.Count)
            OptionsError = "Las opciones no pueden repetirse.";
        else
            OptionsError = string.Empty;
    }

    private void ValidateCorrectAnswer()
    {
        if (AnswerChoices.Count == 0)
            CorrectAnswerError = "Primero escribe las opciones de respuesta.";
        else if (string.IsNullOrWhiteSpace(CorrectAnswer))
            CorrectAnswerError = "Selecciona cuál es la respuesta correcta.";
        else if (!AnswerChoices.Contains(CorrectAnswer))
            CorrectAnswerError = "La respuesta correcta debe ser una de las opciones.";
        else
            CorrectAnswerError = string.Empty;
    }

    private bool Validate()
    {
        ValidateStory();
        ValidateQuestion();
        ValidateSkill();
        ValidateOptions();
        ValidateCorrectAnswer();

        return new[] { StoryError, QuestionError, SkillError, OptionsError, CorrectAnswerError }
            .All(string.IsNullOrEmpty);
    }

    // ----- Helpers -----
    private void LoadForEditing(Question question)
    {
        _editing = question;
        IsEditing = true;

        SelectedStory = Stories.FirstOrDefault(s => s.Id.ToString() == question.StoryId);
        QuestionText = question.Text;
        Skill = question.Skill;
        OptionsText = question.Options;          // first: this rebuilds the answer choices
        CorrectAnswer = question.CorrectAnswer;  // then: pick the saved answer
    }

    private void ResetForm()
    {
        _showErrors = false;
        _editing = null;
        IsEditing = false;

        SelectedStory = null;
        QuestionText = string.Empty;
        Skill = null;
        OptionsText = string.Empty;
        CorrectAnswer = null;

        StoryError = QuestionError = SkillError = OptionsError = CorrectAnswerError = string.Empty;
    }

    // ----- Commands -----
    [RelayCommand]
    private async Task SaveAsync()
    {
        _showErrors = true;
        if (!Validate()) return;   // errors are already inline, no popup

        var question = new Question
        {
            Text = QuestionText.Trim(),
            Skill = Skill!,
            Options = string.Join(",", QuestionOptions.Parse(OptionsText)),   // saved clean, no stray spaces
            CorrectAnswer = CorrectAnswer!,
            StoryId = SelectedStory!.Id.ToString(),
            StoryTitle = SelectedStory.Title
        };

        var wasEditing = _editing is not null;

        if (_editing is null)
        {
            AppDataService.AddQuestion(question);
        }
        else
        {
            question.Id = _editing.Id;
            AppDataService.UpdateQuestion(question);
        }

        await ShowSuccessAsync(
            wasEditing ? "¡Cambios guardados!" : "¡Guardado!",
            wasEditing ? "La pregunta se actualizó correctamente."
                       : "La pregunta de trivia se ha enlazado exitosamente.");

        ResetForm();
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private Task CancelAsync() => Shell.Current.GoToAsync("..");
}