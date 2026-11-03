using ReadBuddy.ViewModels;
using ReadBuddy.Views;

namespace ReadBuddy
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            BindingContext = new AppShellViewModel();

            // Route registration is navigation wiring, not logic, so it stays here.
            // These names must match the constants in Routes.
            Routing.RegisterRoute(Routes.StoryForm, typeof(StoryFormPage));
            Routing.RegisterRoute(Routes.QuestionForm, typeof(QuestionFormPage));
            Routing.RegisterRoute(Routes.VocabularyForm, typeof(VocabularyFormPage));
            Routing.RegisterRoute(Routes.ManageStories, typeof(ManageStoriesPage));
            Routing.RegisterRoute(Routes.ManageQuestions, typeof(ManageQuestionsPage));
            Routing.RegisterRoute(Routes.ManageVocabulary, typeof(ManageVocabularyPage));
            Routing.RegisterRoute("story-questions", typeof(QuestionsPage));
            Routing.RegisterRoute("vocabulary-page", typeof(VocabularyPage));
        }
    }
}