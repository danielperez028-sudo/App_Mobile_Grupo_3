using System;
using Microsoft.Maui.Controls;

namespace ReadBuddy.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        
        private async void OnEnterButtonClicked(object sender, EventArgs e)
        {  
            await Shell.Current.GoToAsync("//home");
        }
    }
}
