using System;
using System.Linq;
using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    [QueryProperty(nameof(TargetStoryId), "storyId")]
    [QueryProperty(nameof(TargetStoryTitle), "storyTitle")]
    public partial class QuestionsPage : ContentPage
    {
        private string _targetStoryId = string.Empty;

        public string TargetStoryId
        {
            get => _targetStoryId;
            set { _targetStoryId = value; LoadQuestions(); }
        }

        public string TargetStoryTitle
        {
            get => string.Empty;
            set { StoryTitleLabel.Text = $"Historia: {Uri.UnescapeDataString(value ?? string.Empty)}"; }
        }

        public QuestionsPage()
        {
            InitializeComponent();
        }

        private void LoadQuestions()
        {
            if (string.IsNullOrEmpty(TargetStoryId)) return;

            var filteredQuestions = AppDataService.Questions
                .Where(q => q.StoryId == TargetStoryId)
                .ToList();

            QuestionsCollectionView.ItemsSource = filteredQuestions;
        }

        private void OnStudentAnswerPickerLoaded(object sender, EventArgs e)
        {
            if (sender is Picker standalonePicker)
            {
                var questionData = standalonePicker.BindingContext as Question;

                if (questionData != null && !string.IsNullOrWhiteSpace(questionData.Options))
                {
                    var optionsList = questionData.Options
                        .Split(',')
                        .Select(option => option.Trim())
                        .Where(option => !string.IsNullOrEmpty(option))
                        .ToList();

                    standalonePicker.ItemsSource = optionsList;
                }
            }
        }

        private async void OnAnswerPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is Picker answerPicker && answerPicker.SelectedItem != null)
            {
                var selectedQuestion = answerPicker.BindingContext as Question;
                if (selectedQuestion == null) return;

                
                string selectedAnswer = answerPicker.SelectedItem?.ToString()?.Trim() ?? string.Empty;
                string correctAnswer = selectedQuestion.CorrectAnswer?.Trim() ?? string.Empty;

                if (selectedAnswer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
                {
                    
                    answerPicker.BackgroundColor = Color.FromRgba(76, 175, 80, 50);
                    await DisplayAlertAsync("¡Excelente! 🎉", "¡Tu respuesta es correcta!", "¡Súper!");
                }
                else
                {
                    answerPicker.BackgroundColor = Color.FromRgba(244, 67, 54, 50);
                    await DisplayAlertAsync("Inténtalo de nuevo 🦊", "Esa no es la respuesta correcta. ¡Sigue buscando!", "Intentar otra vez");

                    answerPicker.SelectedIndex = -1;
                    answerPicker.BackgroundColor = Color.FromArgb("#F3F4F6");
                }
            }
        }

        private async void OnSubmitTriviaButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}
