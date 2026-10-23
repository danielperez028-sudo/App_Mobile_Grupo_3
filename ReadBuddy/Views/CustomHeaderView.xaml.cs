using System;
using Microsoft.Maui.Controls;

namespace ReadBuddy.Views
{
    public partial class CustomHeaderView : ContentView
    {
        public CustomHeaderView()
        {
            InitializeComponent();
        }

        private void OnMenuNavigationButtonClicked(object sender, EventArgs e)
        {
            
            Shell.Current.FlyoutIsPresented = true;
        }
    }
}
