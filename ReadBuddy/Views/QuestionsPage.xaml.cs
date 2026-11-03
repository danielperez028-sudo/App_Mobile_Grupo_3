using ReadBuddy.ViewModels;

namespace ReadBuddy.Views;

public partial class QuestionsPage : ContentPage
{
    public QuestionsPage()
    {
        InitializeComponent();
        BindingContext = new QuestionsViewModel();
    }
}