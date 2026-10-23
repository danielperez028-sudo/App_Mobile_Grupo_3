using Microsoft.Maui.Controls;
using ReadBuddy.Services;
using System;
using System.Linq;

namespace ReadBuddy.Views
{
    public partial class LoginPage : ContentPage
    {
        private const string AdminUser = "admin";
        private const string AdminPassword = "1234";

        public LoginPage()
        {
            InitializeComponent();
        }

        private async void OnEnterButtonClicked(object sender, EventArgs e)
        {
            // user inputs
            string inputUser = UsernameEntry.Text?.Trim() ?? string.Empty;
            string inputPassword = PasswordEntry.Text ?? string.Empty;

            // 1. Basic validation for empty fields
            if (string.IsNullOrWhiteSpace(inputUser) || string.IsNullOrWhiteSpace(inputPassword))
            {
                await DisplayAlertAsync("Datos incompletos", "Ingresa usuario y contraseña.", "Aceptar");
                return;
            }

            // 2. CASE A: Check if it's the Administrator
            if (inputUser.Equals(AdminUser, StringComparison.OrdinalIgnoreCase) && inputPassword == AdminPassword)
            {
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;

                // Send to Admin management flow
                await Shell.Current.GoToAsync("//admin-panel/admin-home");
                return;
            }

            // 3. CASE B: Check if it's a Student
            var matchedStudent = AppDataService.Students
                .FirstOrDefault(student => student.UserName.Equals(inputUser, StringComparison.OrdinalIgnoreCase)
                                && student.Password == inputPassword);

            if (matchedStudent != null)
            {
                UsernameEntry.Text = string.Empty;
                PasswordEntry.Text = string.Empty;

                // Send to Student main view
                await Shell.Current.GoToAsync("//student-app/home");
            }
            else
            {
                // Fallback warning if no records match the criteria
                await DisplayAlertAsync("Usuario no encontrado", "El nombre de usuario o la contraseña son incorrectos.", "Aceptar");
            }
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//register-student");
        }
    }
}
