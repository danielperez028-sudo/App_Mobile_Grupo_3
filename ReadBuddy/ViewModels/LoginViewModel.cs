using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReadBuddy.Services;

namespace ReadBuddy.ViewModels;

public partial class LoginViewModel : BaseViewModel
{
    private const string AdminUser = "admin";
    private const string AdminPassword = "1234";

    private bool _showErrors;

    // ----- Fields -----
    [ObservableProperty] private string userName = string.Empty;
    [ObservableProperty] private string password = string.Empty;

    // ----- Inline errors -----
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasUserNameError))] private string userNameError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasPasswordError))] private string passwordError = string.Empty;
    [ObservableProperty][NotifyPropertyChangedFor(nameof(HasLoginError))] private string loginError = string.Empty;

    public bool HasUserNameError => UserNameError.Length > 0;
    public bool HasPasswordError => PasswordError.Length > 0;
    public bool HasLoginError => LoginError.Length > 0;

    partial void OnUserNameChanged(string value)
    {
        LoginError = string.Empty;   // editing a field dismisses the "wrong credentials" message
        if (_showErrors) ValidateUserName();
    }

    partial void OnPasswordChanged(string value)
    {
        LoginError = string.Empty;
        if (_showErrors) ValidatePassword();
    }

    // ----- Validation -----
    private void ValidateUserName() =>
        UserNameError = string.IsNullOrWhiteSpace(UserName)
            ? "Ingresa tu nombre de usuario."
            : string.Empty;

    private void ValidatePassword() =>
        PasswordError = string.IsNullOrEmpty(Password)
            ? "Ingresa tu contraseña."
            : string.Empty;

    private void ResetForm()
    {
        _showErrors = false;

        UserName = string.Empty;
        Password = string.Empty;

        UserNameError = PasswordError = LoginError = string.Empty;
    }

    // ----- Commands -----
    [RelayCommand]
    private async Task EnterAsync()
    {
        _showErrors = true;
        LoginError = string.Empty;

        ValidateUserName();
        ValidatePassword();
        if (HasUserNameError || HasPasswordError) return;

        var inputUser = UserName.Trim();

        // Case A: administrator
        if (inputUser.Equals(AdminUser, StringComparison.OrdinalIgnoreCase) && Password == AdminPassword)
        {
            ResetForm();
            await Shell.Current.GoToAsync(Routes.AdminHome);
            return;
        }

        // Case B: student
        var matchedStudent = AppDataService.Students.FirstOrDefault(s =>
            string.Equals(s.UserName, inputUser, StringComparison.OrdinalIgnoreCase) &&
            s.Password == Password);

        if (matchedStudent is null)
        {
            // Inline, and deliberately vague: it doesn't reveal which of the two was wrong
            LoginError = "El nombre de usuario o la contraseña son incorrectos.";
            return;
        }

        ResetForm();
        await Shell.Current.GoToAsync(Routes.StudentHome);
    }

    [RelayCommand]
    private Task GoToRegisterAsync() => Shell.Current.GoToAsync(Routes.RegisterStudent);
}