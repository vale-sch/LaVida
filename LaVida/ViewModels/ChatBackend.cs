using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
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
        private FirebaseClient firebaseClient;


        public ChatBackend()
        {
            Console.WriteLine("Try to connect to Server...");
            try
            {
                firebaseClient = new FirebaseClient("https://lavida-b6aca-default-rtdb.europe-west1.firebasedatabase.app/");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("...Connection established!");
            ReceiveMessage();
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
        private void ReceiveMessage()
        {

            var collection = firebaseClient.Child("Messages").AsObservable<MessageModel>().Subscribe((dbevent) =>
            {
                if (dbevent.Object != null)
                {
                    if (dbevent.Object.UserName != App.User)
                        RefreshMessages(dbevent.Object.UserName, dbevent.Object.Message);
                }
            });


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
            firebaseClient.Child("Messages").PostAsync(new MessageModel() { Message = text, UserName = username });
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
