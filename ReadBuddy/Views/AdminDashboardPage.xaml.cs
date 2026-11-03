using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class AdminDashboardPage : ContentPage
{
    public AdminDashboardPage()
    {
        InitializeComponent();
        BindingContext = new AdminDashboardViewModel();
    }
}