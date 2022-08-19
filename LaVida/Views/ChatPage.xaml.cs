using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using LaVida.Models;
using LaVida.Services;
using LaVida.ViewModels;
using Xamarin.Forms;

namespace LaVida.Views
{
    public partial class ChatPage : ContentPage
    {
        public static int ScrollingFactor = 15;
        public ChatPage(RealTimeMessageStream realTimeMessageStream)
        {
            InitializeComponent();
            Title = realTimeMessageStream.Connection.ChatPartner;
            BindingContext = new ChatPageViewModel(realTimeMessageStream);
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

        private void ChatList_Scrolled(object sender, ScrolledEventArgs e)
        {
            ScrollingFactor =  15 + (int)(e.ScrollY / 50);
            Console.WriteLine(ScrollingFactor);
        }
    }
}
