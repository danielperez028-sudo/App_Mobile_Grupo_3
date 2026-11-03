using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class VocabularyPage : ContentPage
{
    private readonly VocabularyViewModel _viewModel = new();

    public VocabularyPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Refresh();   // pick up words added, edited or deleted elsewhere
    }
}