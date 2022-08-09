using System;
using LaVida.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace LaVida
{
    public partial class App : Application
    {
        public static string User = "";

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

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
