using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaVida.Views
{
    public partial class RegistrationPage : ContentPage
    {
        public RegistrationPage()
        {
            InitializeComponent();
            Title = "LAVIDA - Registration";
        }

        private async void NavigateToChatRoom(object sender, EventArgs e)
        {
            App.User = userName.Text;
            await Shell.Current.GoToAsync("//main");        
        }
    }
}