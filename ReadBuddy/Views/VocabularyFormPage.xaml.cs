using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class VocabularyFormPage : ContentPage
{
    public VocabularyFormPage()
    {
        InitializeComponent();
        BindingContext = new VocabularyFormViewModel();
    }
}