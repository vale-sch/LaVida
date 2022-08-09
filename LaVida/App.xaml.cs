using System;
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
        /*private MongoClient client;
       private IMongoDatabase database;
       private string dbName = "AccountsDB";
       private string collectionName = "Account";*/
       public App()
       {
           InitializeComponent();

           MainPage = new AppShell();


       }

       protected async override void OnStart()
       {
          /* try
           {
               var connectionString = "mongodb://LaVidaAdmin:pO85OZbNjw1iNxvV@ac-jhy5v3n-shard-00-00.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-01.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-02.x5tlyr9.mongodb.net:27017/?ssl=true&replicaSet=atlas-9uw66t-shard-0&authSource=admin&retryWrites=true&w=majority";
              MongoClientSettings settings =  MongoClientSettings.FromUrl(new MongoUrl(connectionString));
               settings.SslSettings = new SslSettings()
               {
                   EnabledSslProtocols = System.Security.Authentication.SslProtocols.None
               };
               client = new MongoClient(settings);

               database = client.GetDatabase(dbName);

               var collection = database.GetCollection<Account>(collectionName);
               Account account = new Account() { Name = "Alfred", Password = "1234", PhoneNumber = "+49145039399" };
               await collection.InsertOneAsync(account);
               Console.WriteLine("HIER:   " + account);
               Device.BeginInvokeOnMainThread(() =>
               {
                   App.Current.MainPage.DisplayAlert("Alert", "SUCCESS", "OK");
               });
           }
           catch (Exception ex)
           {
               Device.BeginInvokeOnMainThread(() =>
               {
                   App.Current.MainPage.DisplayAlert("Alert", ex.ToString(), "OK");
               });
           }*/
    }

    protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
