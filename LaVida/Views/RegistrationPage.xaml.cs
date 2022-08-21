using Firebase.Database;
using LaVida.Models;
using LaVida.Services;
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
        private readonly RegistrationViewModel registrationViewModel;
        public RegistrationPage()
        {
            InitializeComponent();
            Title = "LAVIDA - Registration";
            BindingContext = registrationViewModel = new RegistrationViewModel();
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

            NavigationManager.NextPageWithoutBack(new MainPage());
            Account newAccount = new Account() { Name = userName.Text, Password = password.Text, PhoneNumber = phoneNumber.Text, AccountID = App.DeviceIdentifier.DeviceID, HasToRefreshConnections = false, Connections = new List<Connection>() };
            App.myAccount = newAccount;
            await App.MongoDBDatabase.InsertOne(newAccount);
            await registrationViewModel.LoadPossibleConnectionsFromDB();
            await App.SQLLLocalDB.SaveMyAccount(new SQLAccount() { Id = newAccount.Id, Name = newAccount.Name, AccountID = newAccount.AccountID });
            NavigationManager.NextPageWithoutBack(new ChatsOverviewPage());
        }

    }
}