using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using LaVida.Models;
using LaVida.ViewModels;
using Xamarin.Forms;

namespace LaVida.Views
{
    public partial class ChatPage : ContentPage
    {

        public ChatPage( Connection connection)
        {
            InitializeComponent();
            Title = connection.ChatPartner;
            BindingContext = new ChatPageViewModel(connection);
        }

        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }

      
    }
}
