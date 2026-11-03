using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class HomePage : ContentPage
{
    public HomePage()
    {
        InitializeComponent();
        BindingContext = new HomeViewModel();
    }
}