﻿using LaVida.Models;
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
            ChatPage chatPage = new ChatPage();
           _ =  App.Current.MainPage.Navigation.PushAsync(chatPage);
            NavigationPage.SetHasBackButton(chatPage, false);


            if (String.IsNullOrEmpty(App.DeviceIdentifier.DeviceID))
            {
               await App.Current.MainPage.DisplayAlert("Info", "Device Identifier couldnt be recognized! Please restart the App.", "OK");
                System.Diagnostics.Process.GetCurrentProcess().Kill();
                return;
            }

            Account test = new Account() { Name = userName.Text, Password = password.Text, PhoneNumber = phoneNumber.Text, AccountID = App.DeviceIdentifier.DeviceID, Connections = new List<Connection>() };
            await App.mongoCollection.InsertOneAsync(test);
        }

    }
}