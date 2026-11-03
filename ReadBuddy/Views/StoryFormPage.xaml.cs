using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class StoryFormPage : ContentPage
{
    public StoryFormPage()
    {
        InitializeComponent();
        BindingContext = new StoryFormViewModel();
    }
}