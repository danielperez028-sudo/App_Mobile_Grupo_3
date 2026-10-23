using System;
using System.Linq;
using Microsoft.Maui.Controls;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class VocabularyPage : ContentPage
    {
        public VocabularyPage()
        {
            InitializeComponent();
        }

        // Refreshes the dictionary items every time the user opens this tab page
        protected override void OnAppearing()
        {
            base.OnAppearing();
            FilterVocabulary();
        }

        private void OnSearchBarTextChanged(object sender, TextChangedEventArgs e)
        {
            FilterVocabulary();
        }

        // engine to process current search terms against the in-memory array data
        private void FilterVocabulary()
        {
            var searchText = VocabularySearchBar.Text?.Trim() ?? string.Empty;

            // Extract baseline elements from global service
            var vocabularyList = AppDataService.VocabularyWords.AsEnumerable();

            // Apply lookup filters if the user types into the SearchBar container
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                vocabularyList = vocabularyList.Where(v => v.Term.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            // Order alphabetically for clean dictionary tracking structure
            VocabularyCollectionView.ItemsSource = vocabularyList.OrderBy(v => v.Term).ToList();
        }
    }
}
