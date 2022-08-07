using LaVida.Services;
using LaVida.Views;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaVida
{
    public partial class App : Application
    {
        private HubConnection hubConnection;
        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected async override void OnStart()
        {
            hubConnection = new HubConnectionBuilder().WithUrl("https://192.168.178.114" + "/chatHub").Build();

            _ = Task.Run(async () =>
            {
                await Connect();
            });
            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                Console.WriteLine(message);

            });
            await SendMessage("peter", "leck ´mich doch fett am arsch");

           

        }
        async Task SendMessage(string user, string message)
        {
            Console.WriteLine(message + user);

            await hubConnection.InvokeAsync("SendMessage", user, message);
        }
        async Task Connect()
        {
          
            await hubConnection.StartAsync();
            Console.WriteLine("DRIN11");
        }
        async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }
        protected override void OnSleep()
        {
            _ = Task.Run(async () =>
            {
                await Disconnect();
            });
        }

        protected override void OnResume()
        {
            _ = Task.Run(async () =>
            {
                await Connect();
            });
        }
    }
}
