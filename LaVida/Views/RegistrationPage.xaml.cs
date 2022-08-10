using LaVida.Models;
using System;
using System.Collections.Generic;
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

            //   await collection.InsertOneAsync(account);
            if (String.IsNullOrEmpty(userName.Text)) return;
            Account test = new Account() { Name = userName.Text, Password = password.Text, PhoneNumber = phoneNumber.Text, AccountID = phoneNumber.Text.GetHashCode(), Connections = new List<Connection>() };
            await App.mongoCollection.InsertOneAsync(test);
        }

    }
}