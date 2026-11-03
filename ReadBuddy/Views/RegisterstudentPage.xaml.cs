using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class RegisterStudentPage : ContentPage
{
    public RegisterStudentPage()
    {
        InitializeComponent();
        BindingContext = new StudentRegisterViewModel();
    }
}