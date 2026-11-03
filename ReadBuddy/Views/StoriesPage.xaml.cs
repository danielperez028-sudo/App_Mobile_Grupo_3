using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class StoriesPage : ContentPage
{
    private readonly StoriesViewModel _viewModel = new(onlyActive: true);

    public StoriesPage()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.Refresh();   // pick up stories added, edited or deleted elsewhere
    }
}