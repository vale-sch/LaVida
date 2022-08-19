using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using LaVida.Models;
using LaVida.Services;
using LaVida.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaVida.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {

        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;

        public Queue<MessageModel> DelayedMessages { get; set; } = new Queue<MessageModel>();
        public List<MessageModel> AllMessages { get; set; } = new List<MessageModel>();

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }
        private readonly RealTimeMessageStream MessageStream;
        private int startMessageFactor = 0;
        public ChatPageViewModel(RealTimeMessageStream _messageStream)
        {
            MessageStream = _messageStream;
            MessageAppearingCommand = new Command<MessageModel>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<MessageModel>(OnMessageDisappearing);
            OnSendCommand = new Command(() =>
            {

                if (!string.IsNullOrEmpty(TextToSend))
                {
                    SendMessage(new MessageModel() { Message = DateTime.Now.ToString() + "\n" + TextToSend, UserName = App.myAccount.Name, DateTime = DateTime.Now });
                    TextToSend = String.Empty;
                }

            });
            startMessageFactor = ChatPage.MessageShowFactor;
            GetMessagesFromStream();

        }

        private bool GetMessagesFromStream()
        {

            foreach (var message in MessageStream.Messages)
                if (!AllMessages.Contains(message))
                    AllMessages.Insert(0, message);


            Device.BeginInvokeOnMainThread(async () =>
            {
                foreach (var renderedMessage in AllMessages.Take(ChatPage.MessageShowFactor))
                {

                    if (!Messages.Contains(renderedMessage))
                    {
                        if (LastMessageVisible)
                            Messages.Insert(Messages.Count, renderedMessage);
                        else
                        {
                            Messages.Insert(Messages.Count, renderedMessage);
                            PendingMessageCount++;
                        }
                    }
                    if (ChatPage.MessageShowFactor == startMessageFactor)
                        if (Messages.Count >= ChatPage.MessageShowFactor)
                            Messages.RemoveAt(Messages.Count - 1);
                }
                await Task.Delay(50);
                GetMessagesFromStream();
            });

            return true;
        }

        private void SendMessage(MessageModel newMessage)
        {
            FirebaseDB.firebaseClient.Child(MessageStream.Connection.ChatID).PostAsync(newMessage, false);
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
