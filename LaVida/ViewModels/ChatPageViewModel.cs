using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using LaVida.Models;
using LaVida.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaVida.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;

        public Queue<MessageModel> DelayedMessages { get; set; } = new Queue<MessageModel>();
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }
        private readonly  Connection Connection;
        public ChatPageViewModel( Connection _connection)
        {
            Connection = _connection;
            MessageAppearingCommand = new Command<MessageModel>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<MessageModel>(OnMessageDisappearing);
            OnSendCommand = new Command(() =>
            {

                if (!string.IsNullOrEmpty(TextToSend))
                {
                    SendMessage(App.myAccount.Name, TextToSend, DateTime.Now);
                }

            });
            StreamMessagesFromServer();
        }
        private void StreamMessagesFromServer()
        {

            FirebaseDB.firebaseClient.Child(Connection.ChatID).AsObservable<MessageModel>().Subscribe( (dbevent) =>
            {
                if (dbevent.Object != null && !string.IsNullOrEmpty(dbevent.Object.Message))
                {
                    if (LastMessageVisible)
                    {
                        Messages.Insert(0, new MessageModel() { Message = dbevent.Object.DateTime.ToString() + "\n" + dbevent.Object.Message, UserName = dbevent.Object.UserName, DateTime = dbevent.Object.DateTime });
                    }
                    else
                    {
                        Messages.Insert(0, new MessageModel() { Message = dbevent.Object.DateTime.ToString() + "\n" + dbevent.Object.Message, UserName = dbevent.Object.UserName, DateTime = dbevent.Object.DateTime });
                        PendingMessageCount++;
                    }
                }
            });
        }
        private void SendMessage(string username, string message, DateTime dateTime)
        {

            FirebaseDB.firebaseClient.Child(Connection.ChatID).PostAsync(new MessageModel() { Message = message, UserName = username, DateTime = dateTime });
            TextToSend = string.Empty;

        }
        void OnMessageAppearing(MessageModel message)
        {
            var idx = Messages.IndexOf(message);
            if (idx <= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    while (DelayedMessages.Count > 0)
                    {
                        Messages.Insert(0, DelayedMessages.Dequeue());
                    }
                    LastMessageVisible = true;
                    PendingMessageCount = 0;
                });
            }
        }

        void OnMessageDisappearing(MessageModel message)
        {
            var idx = Messages.IndexOf(message);
            if (idx >= 6)
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    LastMessageVisible = false;
                });

            }
        }
    }
}
