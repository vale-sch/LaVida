using Firebase.Database;
using LaVida.Models;
using LaVida.ViewModels;
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
        readonly FirebaseClient  firebaseClient;
        public RegistrationPage(FirebaseClient firebaseClient)
        {
            InitializeComponent();
            Title = "LAVIDA - Registration";
            this.firebaseClient = firebaseClient;
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
       
            NavigationManager.NextPageWithoutBack(new ChatsOverviewPage(firebaseClient));
            Account newAccount = new Account() { Name = userName.Text, Password = password.Text, PhoneNumber = phoneNumber.Text, AccountID = App.DeviceIdentifier.DeviceID, Connections = new List<Connection>() };
            App.myAccount = newAccount;
            await App.mongoCollection.InsertOneAsync(newAccount);
        }

    }
}