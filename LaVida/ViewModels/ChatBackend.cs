using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using LaVida.Models;
using MongoDB.Driver;
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
        private readonly FirebaseClient firebaseClient;
        private readonly Connection connection;
        private readonly ContactCore contactsFromIntern;

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
            StreamMessagesFromServer();
            MessageAppearingCommand = new Xamarin.Forms.Command<MessageModel>(OnMessageAppearing);
            MessageDisappearingCommand = new Xamarin.Forms.Command<MessageModel>(OnMessageDisappearing);

            OnSendCommand = new Command(() =>
            {

                if (!string.IsNullOrEmpty(TextToSend))
                {
                    SendMessage(App.User, TextToSend, DateTime.Now);
                }

            });
            contactsFromIntern = new ContactCore();


            LoadPossibleConnections();


        }
        private void LoadPossibleConnections()
        {
            foreach (var contactFromIntern in contactsFromIntern.GetContacts())
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    foreach (var accountFromDB in App.AccountsFromDB)
                        if (phoneFromIntern.PhoneNumber == accountFromDB.PhoneNumber)
                            Console.WriteLine("TREFFER");
        }
        private void StreamMessagesFromServer()
        {

            var collection = firebaseClient.Child("Message").AsObservable<MessageModel>().Subscribe((dbevent) =>
            {
                if (dbevent.Object != null)
                {
                    RefreshMessages(dbevent.Object.UserName, dbevent.Object.Message, dbevent.Object.DateTime);
                }
            });


        }
        public void RefreshMessages(string userName, string message, DateTime dateTime)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (LastMessageVisible)
                {
                    Messages.Insert(0, new MessageModel() { Message = dateTime.ToString() + "\n" + message, UserName = userName, DateTime = dateTime });
                }
                else
                {
                    DelayedMessages.Enqueue(new MessageModel() { Message = dateTime.ToString() + "\n" + message, UserName = userName, DateTime = dateTime });
                    PendingMessageCount++;
                }
            }
        }

        /* private void SendMessageFirstTime(string myUsername, string UsernameFromOther, string text)
         {
         }*/
        private void SendMessage(string username, string message, DateTime dateTime)
        {

            firebaseClient.Child("Message").PostAsync(new MessageModel() { Message = message, UserName = username, DateTime = dateTime });
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
