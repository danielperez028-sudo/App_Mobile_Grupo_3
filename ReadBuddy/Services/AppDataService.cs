using System.Collections.ObjectModel;
using ReadBuddy.Models;

namespace ReadBuddy.Services;

public static class AppDataService
{
    private static int _nextStudentId = 1;
    private static int _nextStoryId = 1;
    private static int _nextQuestionId = 1;
    private static int _nextVocabularyWordId = 1;

    public static ObservableCollection<Student> Students { get; } = new();
    public static ObservableCollection<Story> Stories { get; } = new();
    public static ObservableCollection<Question> Questions { get; } = new();
    public static ObservableCollection<VocabularyWord> VocabularyWords { get; } = new();

    private static int IndexOf<T>(ObservableCollection<T> list, Func<T, bool> match)
    {
        for (var i = 0; i < list.Count; i++)
            if (match(list[i])) return i;
        return -1;
    }

    // ---------- Students  ----------
    public static void AddStudent(Student student)
    {
        student.Id = _nextStudentId++;
        Students.Add(student);
    }

    public static bool UserNameExists(string userName) =>
        Students.Any(s => s.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));

    // ---------- Stories ----------
    public static void AddStory(Story story)
    {
        story.Id = _nextStoryId++;
        Stories.Add(story);
    }

    public static void UpdateStory(Story updated)
    {
        var index = IndexOf(Stories, s => s.Id == updated.Id);
        if (index < 0) return;
        Stories[index] = updated;

        // Questions store a copy of the story title, so keep it in sync.
        var storyId = updated.Id.ToString();
        for (var i = 0; i < Questions.Count; i++)
        {
            if (Questions[i].StoryId != storyId) continue;
            Questions[i].StoryTitle = updated.Title;
            Questions[i] = Questions[i];   // same instance, re-raises Replace so the UI refreshes
        }
    }

    public static int CountQuestionsForStory(int storyId) =>
        Questions.Count(q => q.StoryId == storyId.ToString());

    public static void DeleteStory(Story story)
    {
        // A question can't exist without its story, so remove them too.
        var storyId = story.Id.ToString();
        foreach (var q in Questions.Where(q => q.StoryId == storyId).ToList())
            Questions.Remove(q);

        var index = IndexOf(Stories, s => s.Id == story.Id);
        if (index >= 0) Stories.RemoveAt(index);
    }

    // ---------- Questions ----------
    public static void AddQuestion(Question question)
    {
        question.Id = _nextQuestionId++;
        Questions.Add(question);
    }

    public static void UpdateQuestion(Question updated)
    {
        var index = IndexOf(Questions, q => q.Id == updated.Id);
        if (index >= 0) Questions[index] = updated;
    }

    public static void DeleteQuestion(Question question)
    {
        var index = IndexOf(Questions, q => q.Id == question.Id);
        if (index >= 0) Questions.RemoveAt(index);
    }

    // ---------- Vocabulary ----------
    public static void AddVocabularyWord(VocabularyWord word)
    {
        word.Id = _nextVocabularyWordId++;
        VocabularyWords.Add(word);
    }

    public static void UpdateVocabularyWord(VocabularyWord updated)
    {
        var index = IndexOf(VocabularyWords, w => w.Id == updated.Id);
        if (index >= 0) VocabularyWords[index] = updated;
    }

    public static void DeleteVocabularyWord(VocabularyWord word)
    {
        var index = IndexOf(VocabularyWords, w => w.Id == word.Id);
        if (index >= 0) VocabularyWords.RemoveAt(index);
    }
}