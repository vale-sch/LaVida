using System;
using System.IO;
using System.Threading.Tasks;
using LaVida.Helpers;
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
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new MainPage());
            ConnectToAccount();
        }
        private async void ConnectToAccount()
        {

            var account = await SQLLLocalDB.GetMyAccount();
            Console.WriteLine("JOJO1");
            Console.WriteLine(account == null);
            if (account == null)
            {
                MessagingCenter.Send(DeviceIdentifier, "GetDeviceID");
                while (DeviceIdentifier.DeviceID == null)
                {
                    await Task.Delay(1);
                    Console.WriteLine(DeviceIdentifier.DeviceID == null);

                }
                Console.WriteLine("Peter");

                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new RegistrationPage()); });
                MongoAccountDB.Connect();
                await MongoAccountDB.GetAllAccountsFromDB();

            }
            else
            {
                Console.WriteLine("JOJO");

                foreach (var accountFromSQL in account)
                {
                    myAccount = accountFromSQL;
                    _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new ChatsOverviewPage()); });

                }
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
