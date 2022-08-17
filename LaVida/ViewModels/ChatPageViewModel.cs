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

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
 
        private readonly  Connection Connection;
        public ChatPageViewModel( Connection _connection)
        {
            Connection = _connection;
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
                    Messages.Insert(0, new MessageModel() { Message = dbevent.Object.DateTime.ToString() + "\n" + dbevent.Object.Message, UserName = dbevent.Object.UserName, DateTime = dbevent.Object.DateTime });
                }
            });
        }
        private void SendMessage(string username, string message, DateTime dateTime)
        {

            FirebaseDB.firebaseClient.Child(Connection.ChatID).PostAsync(new MessageModel() { Message = message, UserName = username, DateTime = dateTime });
            TextToSend = string.Empty;

        }
    }
}
