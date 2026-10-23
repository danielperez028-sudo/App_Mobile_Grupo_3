using System;
using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class StoryFormPage : ContentPage
    {
        public StoryFormPage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var title = TitleEntry.Text?.Trim() ?? string.Empty;
            var level = LevelPicker.SelectedItem?.ToString() ?? string.Empty;
            var skill = SkillPicker.SelectedItem?.ToString() ?? string.Empty;
            var category = CategoryEntry.Text?.Trim() ?? string.Empty;
            var content = ContentEditor.Text?.Trim() ?? string.Empty;
            var cover = CoverEntry.Text?.Trim() ?? string.Empty;
            var isActive = IsActiveSwitch.IsToggled;

            // 1. VALIDATION: Check for required text inputs and picker selections
            if (string.IsNullOrWhiteSpace(title) ||
                string.IsNullOrWhiteSpace(level) ||
                string.IsNullOrWhiteSpace(skill) ||
                string.IsNullOrWhiteSpace(content)||
                string.IsNullOrWhiteSpace(cover))

            {
                await DisplayAlertAsync("Datos incompletos", "Por favor, completa todos los campos requeridos.", "Aceptar");
                return;
            }

            // 2. CREATION: Map properties directly to the Story model
            var newStory = new Story
            {
                Title = title,
                Level = level,
                Skill = skill,
                Content = content,
                Category= string.IsNullOrWhiteSpace(category) ? "General" : category,
                Cover = cover,
                IsActive = isActive
            };

            // 3. SERVICE STORAGE: Insert new record into the static ObservableCollection
            AppDataService.AddStory(newStory);

            // 4. ACTION FEEDBACK & ROUTING
            await DisplayAlertAsync("¡Guardado!", "La historia se ha creado exitosamente.", "Aceptar");

            // Clear inputs for clean future entries
            TitleEntry.Text = string.Empty;
            CoverEntry.Text = string.Empty;
            LevelPicker.SelectedIndex = -1;
            SkillPicker.SelectedIndex = -1;
            CategoryEntry.Text = string.Empty;
            ContentEditor.Text = string.Empty;
            IsActiveSwitch.IsToggled = true;

            // Redirect back to Admin dashboard view
            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            // Clear current inputs and abort back to safety
            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }
    }
}
