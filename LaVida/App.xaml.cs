using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using LaVida.Helpers;
using LaVida.Models;
using LaVida.ViewModels;
using LaVida.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaVida
{
    public partial class App : Application
    {
        public static IMongoCollection<Account> mongoCollection;
        public static ObservableCollection<Account> AccountsFromDB = new ObservableCollection<Account>();
        public static DeviceIDMessage DeviceIdentifier = new DeviceIDMessage();
        public static Account myAccount;

        private MongoClient Client;
        private IMongoDatabase Database;
        private readonly string dbName = "AccountsDB";
        private readonly string collectionName = "Account";
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            


            Connect();
        }
        private void Connect()
        {
            Task.Run(async () =>
            {
                await ConnectToAccount();
            });
        }
        private async Task ConnectToAccount()
        {
            try
            {
                var connectionString = "mongodb://LaVidaAdmin:pO85OZbNjw1iNxvV@ac-jhy5v3n-shard-00-00.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-01.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-02.x5tlyr9.mongodb.net:27017/?ssl=true&replicaSet=atlas-9uw66t-shard-0&authSource=admin&retryWrites=true&w=majority";
                Client = new MongoClient(connectionString);
                Database = Client.GetDatabase(dbName);

                mongoCollection = Database.GetCollection<Account>(collectionName);

                Console.WriteLine("Accounts DB Connection established!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            MessagingCenter.Send(DeviceIdentifier, "GetDeviceID");
            var accounts = await GettAllAccountsFromDB();
            foreach (var account in accounts)
                AccountsFromDB.Add(account);
      

          
            while (DeviceIdentifier.DeviceID == null)
                await Task.Delay(1);

            Boolean isInDB = false;
            foreach (var accountFromDB in AccountsFromDB)
            {
                if (DeviceIdentifier.DeviceID == accountFromDB.AccountID)
                {
                    myAccount = accountFromDB;
                    isInDB = true;
                }
            }


            if (isInDB)
            {
                _ = Device.InvokeOnMainThreadAsync(() => { NavigateToNextPage(new ChatPage()); });
            }
            else
                _ = Device.InvokeOnMainThreadAsync(() => { NavigateToNextPage(new RegistrationPage()); });
        }
        public async Task<List<Account>> GettAllAccountsFromDB()
        {
            try
            {
                var allItems = await mongoCollection
           .Find(new BsonDocument())
           .ToListAsync();
                return allItems;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);


            }
            return null;
        }
        private void NavigateToNextPage(Page page)
        {
            MainPage.Navigation.PushAsync(page);
            NavigationPage.SetHasBackButton(page, false);

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
