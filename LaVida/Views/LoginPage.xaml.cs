using LaVida.Services;
using LaVida.Services.Interfaces;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaVida.Views
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            Title = "Login";
            InitializeComponent();
            
        }

        private async void NavigateToChatRoom(object sender, EventArgs e)
        {
            App.User = userName.Text;
            await Shell.Current.GoToAsync($"{nameof(ChatPage)}");
        }
    }
}