using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class ManageVocabularyPage : ContentPage
{
    private readonly VocabularyViewModel _viewModel = new();

    public ManageVocabularyPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Refresh();   // pick up changes made in the form
    }
}