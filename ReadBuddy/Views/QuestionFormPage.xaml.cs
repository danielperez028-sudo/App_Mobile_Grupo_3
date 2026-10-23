using System;
using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class QuestionFormPage : ContentPage
    {
        public QuestionFormPage()
        {
            InitializeComponent();
        }

        
        protected override void OnAppearing()
        {
            base.OnAppearing();
            StoryPicker.ItemsSource = AppDataService.Stories;
        }
        
        private void OnOptionsEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var optionsText = OptionsEntry.Text?.Trim() ?? string.Empty;

            if (!string.IsNullOrWhiteSpace(optionsText))
            {
                
                var optionsList = optionsText.Split(',')
                                             .Select(o => o.Trim())
                                             .Where(o => !string.IsNullOrEmpty(o)) 
                                             .ToList();

                
                CorrectAnswerPicker.ItemsSource = optionsList;
            }
            else
            {
                
                CorrectAnswerPicker.ItemsSource = null;
            }
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var selectedStory = StoryPicker.SelectedItem as Story;
            var questionText = QuestionEntry.Text?.Trim() ?? string.Empty;
            var skill = SkillPicker.SelectedItem?.ToString() ?? string.Empty;
            var options = OptionsEntry.Text?.Trim() ?? string.Empty;
            var correctAnswer = CorrectAnswerPicker.SelectedItem?.ToString() ?? string.Empty;

            // 1. VALIDATION: Ensure all target inputs are filled out correctly
            if (selectedStory == null ||
                string.IsNullOrWhiteSpace(questionText) ||
                string.IsNullOrWhiteSpace(skill) ||
                string.IsNullOrWhiteSpace(options) ||
                string.IsNullOrWhiteSpace(correctAnswer))
            {
                await DisplayAlertAsync("Datos incompletos", "Por favor, completa todos los campos requeridos.", "Aceptar");
                return;
            }

            // 2. CREATION: Setup model mapping with cross-referenced data
            var newQuestion = new Question
            {
                Text = questionText,
                Skill = skill,
                CorrectAnswer = correctAnswer,
                StoryId = selectedStory.Id.ToString(), 
                StoryTitle = selectedStory.Title,
                Options = options
            };

            // 3. STORAGE: Commit directly into memory array
            AppDataService.AddQuestion(newQuestion);

            // 4. ACTION FEEDBACK & REDIRECTION
            await DisplayAlertAsync("¡Guardado!", "La pregunta de trivia se ha enlazado exitosamente.", "Aceptar");

            // Flush inputs
            StoryPicker.SelectedIndex = -1;
            QuestionEntry.Text = string.Empty;
            SkillPicker.SelectedIndex = -1;
            OptionsEntry.Text = string.Empty;
            CorrectAnswerPicker.SelectedIndex = -1;

            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }
    }
}
