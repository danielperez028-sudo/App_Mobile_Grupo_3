using System;
using Microsoft.Maui.Controls;
using ReadBuddy.Services;

namespace ReadBuddy.Views
{
    public partial class AdminDashboardPage : ContentPage
    {
        public AdminDashboardPage()
        {
            InitializeComponent();
        }

        private void OnAdminMenuNavigationButtonClicked(object sender, EventArgs e)
        {
            Shell.Current.FlyoutIsPresented = true;
        }

        // Overriding this ensures the student records list refreshes immediately
        protected override void OnAppearing()
        {
            base.OnAppearing();
            StudentsCollectionView.ItemsSource = AppDataService.Students;
        }

        private async void OnAddStoryButtonClicked(object sender, EventArgs e)
        {
            
            await Shell.Current.GoToAsync("//admin-panel/admin-home/story-form");
        }

        private async void OnAddQuestionButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//admin-panel/admin-home/question-form");
        }

        private async void OnAddVocabularyButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//admin-panel/admin-home/vocabulary-form");
        }
    }
}
