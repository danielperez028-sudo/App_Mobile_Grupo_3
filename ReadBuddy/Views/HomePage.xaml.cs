namespace ReadBuddy.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
	}
    private void OnMenuNavigationButtonClicked(object sender, EventArgs e)
    {
        Shell.Current.FlyoutIsPresented = true;
    }
    
    private async void OnVocabularyCardTapped(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("//student-app/vocabulary-page");
    }
    private async void OnStoriesCardTapped(object sender, EventArgs e)
    {
        
        await Shell.Current.GoToAsync("//student-app/stories");
    }

    
}