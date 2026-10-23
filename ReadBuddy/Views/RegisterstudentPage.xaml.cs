using Microsoft.Maui.Controls;
using ReadBuddy.Models;
using ReadBuddy.Services;
using System;
using System.Formats.Tar;

namespace ReadBuddy.Views
{
    public partial class RegisterStudentPage : ContentPage
    {
        public RegisterStudentPage()
        {
            InitializeComponent();
        }

        private async void OnGuardarPerfilClicked(object sender, EventArgs e)
        {
            string studentName = NameEntry.Text?.Trim() ?? string.Empty;
            string ageInput = AgeEntry.Text?.Trim() ?? string.Empty;
            string selectedAvatar = AvatarEntry.Text?.Trim() ?? string.Empty;
            string userName = UserNameEntry.Text?.Trim() ?? string.Empty;
            string userPassword = PasswordEntry.Text ?? string.Empty;

            // 1. VALIDATION: Missing mandatory fields
            if (string.IsNullOrWhiteSpace(studentName) ||
                string.IsNullOrWhiteSpace(ageInput) ||
                string.IsNullOrWhiteSpace(selectedAvatar) ||
                string.IsNullOrWhiteSpace(userName) ||
                string.IsNullOrWhiteSpace(userPassword))
            {
                
                await DisplayAlertAsync("Campos obligatorios", "Por favor, completa todos los campos marcados con (*).", "Aceptar");
                return;
            }

            // 2. VALIDATION: Numeric validation for age
            if (!int.TryParse(ageInput, out int studentAge) || studentAge <= 0)
            {
                
                await DisplayAlertAsync("Edad inválida", "Por favor ingresa una edad numérica válida.", "Aceptar");
                return;
            }

            // 3. VALIDATION: Check for duplicate usernames
            bool doesUserExist = AppDataService.Students.Any(s => s.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (doesUserExist)
            {
                
                await DisplayAlertAsync("Usuario ya existe", "¡Ese nombre de usuario ya está tomado! Intenta con otro más divertido.", "Aceptar");
                return;
            }

            // 4. CREATION: Instantiate the Student model
            var newStudent = new Student
            {
                Name = studentName,
                Age = studentAge,
                Avatar = selectedAvatar,
                UserName = userName.ToLower(),
                Password = userPassword
            };

            // 5. SAVE: Insert into data service
            AppDataService.AddStudent(newStudent);

            // 6. FEEDBACK & NAVIGATION
            
            await DisplayAlertAsync("¡Éxito!", $"Perfil de {studentName} creado correctamente. ¡Usa tu usuario '{newStudent.UserName}' para entrar!", "¡Genial!");

            // Clear input fields
            NameEntry.Text = string.Empty;
            AgeEntry.Text = string.Empty;
            AvatarEntry.Text = string.Empty;
            UserNameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;

            await Shell.Current.GoToAsync("//login");
        }



        private async void OnBackToLoginClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//login");
        }
    }
}
