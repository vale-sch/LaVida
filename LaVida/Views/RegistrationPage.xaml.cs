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
       

            if (String.IsNullOrEmpty(userName.Text) || String.IsNullOrEmpty(phoneNumber.Text))
            {
                await App.Current.MainPage.DisplayAlert("Info", "You must enter your contact details to continue", "OK");
                return;
            }
            if (String.IsNullOrEmpty(App.DeviceIdentifier.DeviceID))
            {
               await App.Current.MainPage.DisplayAlert("Info", "Device Identifier could not be found! Please restart the App.", "OK");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }
            ChatPage chatPage = new ChatPage();
            _ = App.Current.MainPage.Navigation.PushAsync(chatPage);
            NavigationPage.SetHasBackButton(chatPage, false);
            Account newAccount = new Account() { Name = userName.Text, Password = password.Text, PhoneNumber = phoneNumber.Text, AccountID = App.DeviceIdentifier.DeviceID, Connections = new List<Connection>() };
            App.myAccount = newAccount;
            await App.mongoCollection.InsertOneAsync(newAccount);
        }

    }
}