using System;
using LaVida.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using LaVida.Services;
using LaVida.Services.Interfaces;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LaVida
{
    public partial class App : Application
    {
        public static string User = "";
        public static ChatService chatService = new ChatService();

        public App()
        {
            InitializeComponent();

            DependencyService.Register<IChatService, ChatService>();
            MainPage = new AppShell();

        }

        protected override void OnStart()
        {

        }
        public async static void StartService()
        {

            await chatService.Connect();

        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
