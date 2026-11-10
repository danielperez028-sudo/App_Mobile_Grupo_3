using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Models;
using ReadBuddy.Services;
using System.Globalization;
using System.Text.RegularExpressions;

namespace ReadBuddy.ViewModels;

public partial class StudentRegisterViewModel : BaseViewModel
{
    private const int MinAge = 3;
    private const int MaxAge = 80;

    // Letters (with accents), spaces, apostrophes and hyphens, starting with a letter
    private static readonly Regex NamePattern = new(@"^\p{L}[\p{L}\s'\-]*$");
    private static readonly Regex UserNamePattern = new(@"^[a-zA-Z0-9_]+$");

    private bool _showErrors;   // live validation starts after the first Save attempt

    // ----- Fields -----
    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string ageText = string.Empty;   // text, parsed on save
    [ObservableProperty] private string avatar = string.Empty;
    [ObservableProperty] private string userName = string.Empty;
    [ObservableProperty] private string password = string.Empty;

    // ----- Inline errors -----
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasNameError))] private string nameError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasAgeError))] private string ageError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasAvatarError))] private string avatarError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasUserNameError))] private string userNameError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasPasswordError))] private string passwordError = string.Empty;

    public bool HasNameError => NameError.Length > 0;
    public bool HasAgeError => AgeError.Length > 0;
    public bool HasAvatarError => AvatarError.Length > 0;
    public bool HasUserNameError => UserNameError.Length > 0;
    public bool HasPasswordError => PasswordError.Length > 0;

    partial void OnNameChanged(string value) { if (_showErrors) ValidateName(); }
    partial void OnAgeTextChanged(string value) { if (_showErrors) ValidateAge(); }
    partial void OnAvatarChanged(string value) { if (_showErrors) ValidateAvatar(); }
    partial void OnUserNameChanged(string value) { if (_showErrors) ValidateUserName(); }
    partial void OnPasswordChanged(string value) { if (_showErrors) ValidatePassword(); }

    // ----- Validation -----
    private void ValidateName()
    {
        var value = Name?.Trim() ?? string.Empty;

        if (value.Length == 0)
            NameError = "El nombre del estudiante es obligatorio.";
        else if (value.Length < 2)
            NameError = "El nombre debe tener al menos 2 caracteres.";
        else if (value.Length > 50)
            NameError = "El nombre no puede superar los 50 caracteres.";
        else if (!NamePattern.IsMatch(value))
            NameError = "El nombre solo puede contener letras y espacios.";
        else
            NameError = string.Empty;
    }

    private void ValidateAge()
    {
        var value = AgeText?.Trim() ?? string.Empty;

        if (value.Length == 0)
        {
            AgeError = "La edad es obligatoria.";
            return;
        }

        // NumberStyles.None: digits only (no sign, spaces or decimals)
        if (!int.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out var age))
            AgeError = "La edad debe ser un número entero (solo dígitos).";
        else if (age < MinAge || age > MaxAge)
            AgeError = $"La edad debe estar entre {MinAge} y {MaxAge} años.";
        else
            AgeError = string.Empty;
    }

    private void ValidateAvatar()
    {
        var value = Avatar?.Trim() ?? string.Empty;

        if (value.Length == 0)
            AvatarError = "Elige un emoji como avatar.";
        else if (new StringInfo(value).LengthInTextElements > 1)
            AvatarError = "El avatar debe ser un solo emoji.";
        else if (char.IsLetterOrDigit(value[0]))
            AvatarError = "El avatar debe ser un emoji, no una letra o un número.";
        else
            AvatarError = string.Empty;
    }

    private void ValidateUserName()
    {
        var value = UserName?.Trim() ?? string.Empty;

        if (value.Length == 0)
            UserNameError = "El nombre de usuario es obligatorio.";
        else if (value.Length < 4)
            UserNameError = "El usuario debe tener al menos 4 caracteres.";
        else if (value.Length > 20)
            UserNameError = "El usuario no puede superar los 20 caracteres.";
        else if (!UserNamePattern.IsMatch(value))
            UserNameError = "Usa solo letras, números y guion bajo (_), sin espacios.";
        else if (value.Equals("admin", StringComparison.OrdinalIgnoreCase))
            UserNameError = "Ese nombre de usuario está reservado. Elige otro.";
        else if (AppDataService.UserNameExists(value))
            UserNameError = "¡Ese nombre de usuario ya está tomado! Intenta con otro más divertido.";
        else
            UserNameError = string.Empty;
    }

    private void ValidatePassword()
    {
        var value = Password ?? string.Empty;

        if (value.Length == 0)
            PasswordError = "La contraseña es obligatoria.";
        else if (value.Length < 4)
            PasswordError = "La contraseña debe tener al menos 4 caracteres.";
        else if (value.Length > 30)
            PasswordError = "La contraseña no puede superar los 30 caracteres.";
        else if (value.Any(char.IsWhiteSpace))
            PasswordError = "La contraseña no puede tener espacios.";
        else
            PasswordError = string.Empty;
    }

    private bool Validate()
    {
        ValidateName();
        ValidateAge();
        ValidateAvatar();
        ValidateUserName();
        ValidatePassword();

        return new[] { NameError, AgeError, AvatarError, UserNameError, PasswordError }
            .All(string.IsNullOrEmpty);
    }

    private void ResetForm()
    {
        _showErrors = false;

        Name = string.Empty;
        AgeText = string.Empty;
        Avatar = string.Empty;
        UserName = string.Empty;
        Password = string.Empty;

        NameError = AgeError = AvatarError = UserNameError = PasswordError = string.Empty;
    }

    // ----- Commands -----
    [RelayCommand]
    private async Task SaveAsync()
    {
        _showErrors = true;
        if (!Validate()) return;   // errors are already inline, no popup

        var student = new Student
        {
            Name = Name.Trim(),
            Age = int.Parse(AgeText.Trim(), CultureInfo.InvariantCulture),
            Avatar = Avatar.Trim(),
            UserName = UserName.Trim().ToLowerInvariant(),
            Password = Password
        };

        AppDataService.AddStudent(student);

        await Shell.Current.DisplayAlertAsync(
            "¡Éxito!",
            $"Perfil de {student.Name} creado correctamente. ¡Usa tu usuario '{student.UserName}' para entrar!",
            "¡Genial!");

        ResetForm();
        await Shell.Current.GoToAsync(Routes.Login);
    }

    [RelayCommand]
    private async Task BackToLoginAsync()
    {
        ResetForm();   // the page instance is kept alive by Shell, so clear it when leaving
        await Shell.Current.GoToAsync(Routes.Login);
    }
}