using System;
using System.IO;
using System.Threading.Tasks;
using LaVida.Messages;
using LaVida.Models;
using LaVida.Services;
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
        public App()
        {
            InitializeComponent();
            MainPage = new NavigationPage(new MainPage());
            Initialize();
        }
        private async void Initialize()
        {
            MongoAccountDB.Connect();
            
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
                await MongoAccountDB.GetAllAccountsFromDB();
                var mySQLAccount = account.ToArray()[0];
                foreach (var accountDB in MongoAccountDB.AccountsFromDB)
                    if (mySQLAccount.Id == accountDB.Id)
                        myAccount = accountDB;
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
