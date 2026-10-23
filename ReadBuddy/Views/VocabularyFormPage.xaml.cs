using System;
using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class VocabularyFormPage : ContentPage
    {
        public VocabularyFormPage()
        {
            InitializeComponent();
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            var word = WordEntry.Text?.Trim() ?? string.Empty;
            var definition = DefinitionEditor.Text?.Trim() ?? string.Empty;
            var example = ExampleEntry.Text?.Trim() ?? string.Empty;

            // 1. VALIDATION: Check required text properties
            if (string.IsNullOrWhiteSpace(word) || string.IsNullOrWhiteSpace(definition))
            {
                await DisplayAlertAsync("Datos incompletos", "Por favor, ingresa la palabra y su definición.", "Aceptar");
                return;
            }

            // 2. CREATION: Bind to VocabularyWord entity properties
            var newVocabularyWord = new VocabularyWord
            {
                Term= word,
                Definition = definition,
                Example = string.IsNullOrWhiteSpace(example) ? "No se añadió un ejemplo." : example
            };

            // 3. STORAGE: Append record into the static memory array
            AppDataService.AddVocabularyWord(newVocabularyWord);

            // 4. FEEDBACK AND ROUTING BIFURCATION
            await DisplayAlertAsync("¡Guardado!", "La palabra ha sido agregada exitosamente al diccionario.", "Excelente");

            // Clean inputs
            WordEntry.Text = string.Empty;
            DefinitionEditor.Text = string.Empty;
            ExampleEntry.Text = string.Empty;

            // Take admin back to dashboard route
            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }

        private async void OnCancelButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//admin-panel/admin-home");
        }
    }
}
