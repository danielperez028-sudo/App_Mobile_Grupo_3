using CommunityToolkit.Mvvm.ComponentModel;
using ReadBuddy.Models;

namespace ReadBuddy.ViewModels;

public partial class QuestionItemViewModel : ObservableObject
{
    private static readonly Color DefaultColor = Color.FromArgb("#F3F4F6");
    private static readonly Color CorrectColor = Color.FromRgba(76, 175, 80, 50);
    private static readonly Color WrongColor = Color.FromRgba(244, 67, 54, 50);

    public QuestionItemViewModel(Question question)
    {
        Question = question;
        Options = QuestionOptions.Parse(question.Options);
    }

    public Question Question { get; }
    public string Text => Question.Text;
    public string Skill => Question.Skill;
    public IReadOnlyList<string> Options { get; }

    [ObservableProperty] private string? selectedAnswer;
    [ObservableProperty] private Color pickerColor = DefaultColor;

    partial void OnSelectedAnswerChanged(string? value)
    {
        // Setting SelectedAnswer = null (reset) also lands here, so ignore empty values.
        if (string.IsNullOrWhiteSpace(value)) return;
        _ = EvaluateAsync(value);
    }

    private async Task EvaluateAsync(string answer)
    {
        var correct = Question.CorrectAnswer.Trim();

        if (answer.Trim().Equals(correct, StringComparison.OrdinalIgnoreCase))
        {
            PickerColor = CorrectColor;
            await Shell.Current.DisplayAlertAsync(
                "¡Excelente! 🎉", "¡Tu respuesta es correcta!", "¡Súper!");
        }
        else
        {
            PickerColor = WrongColor;
            await Shell.Current.DisplayAlertAsync(
                "Inténtalo de nuevo 🦊", "Esa no es la respuesta correcta. ¡Sigue buscando!", "Intentar otra vez");

            SelectedAnswer = null;
            PickerColor = DefaultColor;
        }
    }
}