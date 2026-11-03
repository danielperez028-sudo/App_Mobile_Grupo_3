using System;
using System.Collections.Generic;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ReadBuddy.ViewModels;

public abstract partial class BaseViewModel : ObservableObject
{
    protected static Task ShowSuccessAsync(string title, string message) =>
        Shell.Current.DisplayAlertAsync(title, message, "Aceptar");

    protected static Task<bool> ConfirmAsync(string title, string message,
        string accept = "Sí, eliminar", string cancel = "Cancelar") =>
        Shell.Current.DisplayAlertAsync(title, message, accept, cancel);
}

public static class Routes
{
    // Absolute routes
    public const string Login = "//login";
    public const string RegisterStudent = "//register-student";
    public const string AdminHome = "//admin-panel/admin-home";
    public const string StudentHome = "//student-app/home";
    public const string StudentStories = "//student-app/stories";
    public const string StudentVocabulary = "//student-app/vocabulary-page";

    // Form routes
    public const string StoryForm = "story-form";
    public const string StoryParam = "Story";
    public const string QuestionForm = "question-form";
    public const string QuestionParam = "Question";
    public const string VocabularyForm = "vocabulary-form";
    public const string VocabularyParam = "Word";

    // Admin list routes
    public const string ManageStories = "admin-stories";
    public const string ManageQuestions = "admin-questions";
    public const string ManageVocabulary = "admin-vocabulary";
}
public static class QuestionOptions
{
    public static readonly IReadOnlyList<string> Skills =
        new[] { "Idea principal", "Detalles específicos", "Inferencias", "Vocabulario" };

    // "Azul, Rojo ,, Verde" -> ["Azul", "Rojo", "Verde"]
    public static List<string> Parse(string? text) =>
        (text ?? string.Empty)
            .Split(',')
            .Select(o => o.Trim())
            .Where(o => o.Length > 0)
            .ToList();
}

public static class StoryOptions
{
    public static readonly IReadOnlyList<string> Levels = new[] { "Facil", "Medio", "Dificil" };
    public static readonly IReadOnlyList<string> Skills =
        new[] { "Idea principal", "Detalles", "Inferencias", "Vocabulario", "Secuencia" };
}