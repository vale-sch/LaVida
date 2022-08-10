using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaVida.Models;
using LaVida.Views;
using MongoDB.Driver;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LaVida
{
    public partial class App : Application
    {
        public static string User = "";
        public static IMongoCollection<Account> mongoCollection;

        private  MongoClient Client;
        private  IMongoDatabase Database;
        private readonly string dbName = "AccountsDB";
        private readonly string collectionName = "Account";
        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

           ConnectToAccount();
           

        }
        private void ConnectToAccount()
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
