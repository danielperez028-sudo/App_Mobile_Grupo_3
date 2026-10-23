using System;
using System.Linq;
using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class StoriesPage : ContentPage
    {
        private string _currentLevelFilter = "Todas";

        public StoriesPage()
        {
            InitializeComponent();
        }

        // Trigger updates automatically when user swaps to this tab view
        protected override void OnAppearing()
        {
            base.OnAppearing();
            ApplyFilters();
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            ApplyFilters();
        }

        private void OnFilterButtonClicked(object sender, EventArgs e)
        {
            if (sender is Button clickedButton)
            {
                _currentLevelFilter = clickedButton.Text;

                // Reset visual styles to toggle active button color state
                BtnAll.BackgroundColor = Colors.White;
                BtnAll.TextColor = (Color)Application.Current.Resources["TextDark"];
                BtnEasy.BackgroundColor = Colors.White;
                BtnEasy.TextColor = (Color)Application.Current.Resources["TextDark"];
                BtnMedium.BackgroundColor = Colors.White;
                BtnMedium.TextColor = (Color)Application.Current.Resources["TextDark"];
                BtnHard.BackgroundColor = Colors.White;
                BtnHard.TextColor = (Color)Application.Current.Resources["TextDark"];

                // Highlight target sender selection
                clickedButton.BackgroundColor = (Color)Application.Current.Resources["SecondaryOrange"];
                clickedButton.TextColor = Colors.White;

                ApplyFilters();
            }
        }

        private async void OnStorySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (e.CurrentSelection.FirstOrDefault() is ReadBuddy.Models.Story selectedStory)
            {
                
                ((CollectionView)sender).SelectedItem = null;

                
                string targetRoute = $"story-questions?storyId={selectedStory.Id}&storyTitle={Uri.EscapeDataString(selectedStory.Title)}";
                await Shell.Current.GoToAsync(targetRoute);
            }
        }


        // processing engine to manage current filtering states dynamically
        private void ApplyFilters()
        {
            var searchText = StorySearchBar.Text?.Trim() ?? string.Empty;

            // Query source array safely
            var filteredList = AppDataService.Stories.AsEnumerable();

            // 1. Evaluate level filter match
            if (!_currentLevelFilter.Equals("Todas", StringComparison.OrdinalIgnoreCase))
            {
                filteredList = filteredList.Where(s => s.Level.Equals(_currentLevelFilter, StringComparison.OrdinalIgnoreCase));
            }

            // 2. Evaluate search bar matching characters
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                filteredList = filteredList.Where(s => s.Title.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            // Bind filtered logic results down into target visual container elements
            StoriesCollectionView.ItemsSource = filteredList.ToList();
        }
    }
}
