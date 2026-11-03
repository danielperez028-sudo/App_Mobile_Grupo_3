using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class QuestionFormPage : ContentPage
{
    public QuestionFormPage()
    {
        InitializeComponent();
        BindingContext = new QuestionFormViewModel();
    }
}