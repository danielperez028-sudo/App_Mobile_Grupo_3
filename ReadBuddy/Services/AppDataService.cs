using System.Collections.ObjectModel;
using ReadBuddy.Models;

namespace ReadBuddy.Services;

public static class AppDataService
{
    // Counters to assign unique identifiers
    private static int _nextStudentId = 1;
    private static int _nextStoryId = 1;
    private static int _nextQuestionId = 1;
    private static int _nextVocabularyWordId = 1;

    // Data collections
    public static ObservableCollection<Student> Students { get; } = new();

    public static ObservableCollection<Story> Stories { get; } = new();

    public static ObservableCollection<Question> Questions { get; } = new();

    public static ObservableCollection<VocabularyWord> VocabularyWords { get; } = new();

    // Add a new student record
    public static void AddStudent(Student student)
    {
        student.Id = _nextStudentId++;
        Students.Add(student);
    }

    // Add a new story record
    public static void AddStory(Story story)
    {
        story.Id = _nextStoryId++;
        Stories.Add(story);
    }

    // Add a new question record
    public static void AddQuestion(Question question)
    {
        question.Id = _nextQuestionId++;
        Questions.Add(question);
    }

    // Add a new vocabulary word record
    public static void AddVocabularyWord(VocabularyWord word)
    {
        word.Id = _nextVocabularyWordId++;
        VocabularyWords.Add(word);
    }
}