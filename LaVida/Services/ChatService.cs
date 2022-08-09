using LaVida.Services.Interfaces;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace LaVida.Services
{
    public class ChatService : IChatService
    {
        private readonly HubConnection hubConnection;
        public ChatService()
        {
            hubConnection = new HubConnectionBuilder()
    .WithUrl("http://10.0.2.2:5145/chatHub")
    .ConfigureLogging(logging =>
    {
        logging.SetMinimumLevel(LogLevel.Information);
        logging.AddConsole();
    })
    .Build();
        }

        public async Task SendMessage(string user, string message)
        {
            await hubConnection.InvokeAsync("SendMessage", user, message);
        }
        public void ReceiveMessage(Action<string, string> GetMessageAndUser)
        {
            hubConnection.On("ReceiveMessage", GetMessageAndUser);
            Console.WriteLine("RECEIVE MESSAGES FROM SERVER");
        }
        public async Task Connect()
        {
            await hubConnection.StartAsync();
        }
        public async Task Disconnect()
        {
            await hubConnection.StopAsync();
        }
    }
}
