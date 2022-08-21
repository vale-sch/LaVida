using System;
using System.IO;
using System.Threading.Tasks;
using LaVida.Messages;
using LaVida.Models;
using LaVida.Services;
using LaVida.Database;
using LaVida.Views;
using Xamarin.Forms;
namespace LaVida
{
    public partial class App : Application
    {
        public static DeviceIDMessage DeviceIdentifier = new DeviceIDMessage();

        public static Account myAccount;
        private static SQLLocalDB sQLLocalDB;

        public static SQLLocalDB SQLLLocalDB
        {
            get
            {
                if (sQLLocalDB == null)
                    sQLLocalDB = new SQLLocalDB(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "myAccount.db3"));

                return sQLLocalDB;
            }
        }
        private static MongoDBDatabase mongoDBDatabase;

        public static MongoDBDatabase MongoDBDatabase
        {
            get
            {
                if (mongoDBDatabase == null)
                    mongoDBDatabase = new MongoDBDatabase("mongodb://LaVidaAdmin:pO85OZbNjw1iNxvV@ac-jhy5v3n-shard-00-00.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-01.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-02.x5tlyr9.mongodb.net:27017/?ssl=true&replicaSet=atlas-9uw66t-shard-0&authSource=admin&retryWrites=true&w=majority");

                return mongoDBDatabase;
            }
        }
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();

            MainPage = new NavigationPage(new MainPage());
            InitializeAccount();
        }
        private async void InitializeAccount()
        {

            var account = await SQLLLocalDB.GetMyAccount();
            if (account.Count == 0)
            {
                MessagingCenter.Send(DeviceIdentifier, "GetDeviceID");
                while (DeviceIdentifier.DeviceID == null)
                {
                    await Task.Delay(1);
                }
                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new RegistrationPage()); });
            }
            else
            {
                var mySQLAccount = account.ToArray()[0];
               var myAccountFromDB =  await MongoDBDatabase.GetAccountById(mySQLAccount.AccountID);

                myAccount = myAccountFromDB;
                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new ChatsOverviewPage()); });

            }
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
