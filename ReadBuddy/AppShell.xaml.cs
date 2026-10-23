using ReadBuddy.Views;

namespace ReadBuddy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            // REGISTRO DE RUTAS DINÁMICAS PARA LOS FORMULARIOS
            Routing.RegisterRoute("story-form", typeof(StoryFormPage));
            Routing.RegisterRoute("question-form", typeof(QuestionFormPage));
            Routing.RegisterRoute("vocabulary-form", typeof(VocabularyFormPage));
            Routing.RegisterRoute("story-questions", typeof(QuestionsPage));
            Routing.RegisterRoute("vocabulary-page", typeof(VocabularyPage));
        }


        private async void OnLogoutButtonClicked(object? sender, EventArgs e)
        {
            var confirm = await DisplayAlertAsync(
                "Cerrar sesión",
                "¿Seguro que quieres salir?",
                "Sí",
                "No");

            if (confirm)
            {
                FlyoutIsPresented = false;
                await GoToAsync("//login");
            }

        }
    }
}
