using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using LaVida.Helpers;
using LaVida.Models;
using LaVida.Services;
using LaVida.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaVida
{
    public partial class App : Application
    {
        public static DeviceIDMessage DeviceIdentifier = new DeviceIDMessage();
        public static Account myAccount;

        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new MainPage());
            MessagingCenter.Send(DeviceIdentifier, "GetDeviceID");
            Task.Run(async () =>
            {
                await ConnectToAccount();
            });
        }
        private async Task ConnectToAccount()
        {
           
            MongoAccountDB.Connect();
            await MongoAccountDB.GetAllAccountsFromDB();

            while (DeviceIdentifier.DeviceID == null)
            {
                await Task.Delay(1);
            }
            Boolean isInDB = false;
            foreach (var accountFromDB in MongoAccountDB.AccountsFromDB)
            {
                if (DeviceIdentifier.DeviceID == accountFromDB.AccountID)
                {
                    myAccount = accountFromDB;
                    isInDB = true;
                }
            }
            if (isInDB)
                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new ChatsOverviewPage()); });

            else
                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new RegistrationPage()); });
        }

        


        protected override void OnStart()
        {
            // MainPage.Navigation.PushModalAsync(new ChatPage());
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
