using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class ManageStoriesPage : ContentPage
{
    private readonly StoriesViewModel _viewModel = new();

    public ManageStoriesPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Refresh();
    }
}