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

        public void ScrollTap(object sender, System.EventArgs e)
        {
            lock (new object())
            {
                if (BindingContext != null)
                {
                    var vm = BindingContext as ChatPageViewModel;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        while (vm.DelayedMessages.Count > 0)
                        {
                            vm.Messages.Insert(0, vm.DelayedMessages.Dequeue());
                        }
                        vm.ShowScrollTap = false;
                        vm.LastMessageVisible = true;
                        vm.PendingMessageCount = 0;
                        ChatList?.ScrollToFirst();
                    });


                }

            }
        }
      
        public void OnListTapped(object sender, ItemTappedEventArgs e)
        {
            chatInput.UnFocusEntry();
        }
    }
}
