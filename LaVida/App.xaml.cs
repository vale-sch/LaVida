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
        public static string User = "";
        public static IMongoCollection<Account> mongoCollection;
        public static ObservableCollection<Account> AccountsFromDB = new ObservableCollection<Account>();
        public static DeviceIDMessage DeviceIdentifier = new DeviceIDMessage();
        private MongoClient Client;
        private IMongoDatabase Database;
        private readonly string dbName = "AccountsDB";
        private readonly string collectionName = "Account";
        private Account myAccount;
        private LandingPage landingPage = new LandingPage();
        public App()
        {
            InitializeComponent();
            Task.Run(async () =>
            {
                await ConnectToAccount();
            });
            MainPage = new NavigationPage(landingPage);



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
            var accounts = await GettALlAccountsFromDB();
            foreach (var account in accounts)
                AccountsFromDB.Add(account);

            MessagingCenter.Send<DeviceIDMessage>(DeviceIdentifier, "GetDeviceID");
            await Task.Delay(TimeSpan.FromMilliseconds(50));
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
               await Device.InvokeOnMainThreadAsync( () =>
               {
                    Current.MainPage.Navigation.PushModalAsync(new AppShell());
                    Current.MainPage.Navigation.PopModalAsync();
               });

                User = myAccount.Name;
            }
            else
            {
                await Device.InvokeOnMainThreadAsync( () =>
                {
                     Current.MainPage.Navigation.PushModalAsync(new RegistrationPage());
                     Current.MainPage.Navigation.PopModalAsync();
                });
            }
        }
        public async Task<List<Account>> GettALlAccountsFromDB()
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
        protected override void OnStart()
        {

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
