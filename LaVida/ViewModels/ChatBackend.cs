using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LaVida.Models;
using Xamarin.Forms;

namespace LaVida.ViewModels
{
    public class ChatBackend : INotifyPropertyChanged
    {
        public bool ShowScrollTap { get; set; } = false;
        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;
        public bool PendingMessageCountVisible { get { return PendingMessageCount > 0; } }

        public Queue<MessageModel> DelayedMessages { get; set; } = new Queue<MessageModel>();
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand MessageAppearingCommand { get; set; }
        public ICommand MessageDisappearingCommand { get; set; }

        public ChatBackend()
        {
            Console.WriteLine("Try to connect to Server...");
            try
            {
                App.StartService();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("...Connection established!");
            App.chatService.ReceiveMessage(ReceiveMessage);
            MessageAppearingCommand = new Command<MessageModel>(OnMessageAppearing);
            MessageDisappearingCommand = new Command<MessageModel>(OnMessageDisappearing);

            OnSendCommand = new Command(() =>
            {

                if (!string.IsNullOrEmpty(TextToSend))
                {
                    SendMessage(App.User, TextToSend);
                }

            });

        }
        private void ReceiveMessage(string userName, string text)
        {
            if (App.User != userName)
                RefreshMessages(userName, text);
        }
        public void RefreshMessages(string userName, string text)
        {
            if (!string.IsNullOrEmpty(text))
            {
                if (LastMessageVisible)
                {
                    Messages.Insert(0, new MessageModel() { Message = text, UserName = userName });
                }
                else
                {
                    DelayedMessages.Enqueue(new MessageModel() { Message = text, UserName = userName });
                    PendingMessageCount++;
                }
            }
        }
        private void SendMessage(string username, string text)
        {
            Task.Run(async () =>
            {
                await App.chatService.SendMessage(username, text);

            });
            RefreshMessages(username, text);
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
                    ShowScrollTap = false;
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
                    ShowScrollTap = true;
                    LastMessageVisible = false;
                });

            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
    }
}
