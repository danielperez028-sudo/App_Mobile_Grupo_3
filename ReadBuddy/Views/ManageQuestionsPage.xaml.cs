using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class ManageQuestionsPage : ContentPage
{
    private readonly ManageQuestionsViewModel _viewModel = new();

    public ManageQuestionsPage()
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