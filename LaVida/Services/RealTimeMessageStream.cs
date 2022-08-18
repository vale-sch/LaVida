using LaVida.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LaVida.Services
{
    public class RealTimeMessageStream
    {
        public Connection Connection { get; set; }
        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();

        public RealTimeMessageStream(Connection connection, ObservableCollection<MessageModel> messageModels)
        {
            this.Connection = connection;
            this.Messages = messageModels;
            StreamMessagesFromServer();
        }
        private void StreamMessagesFromServer()
        {

            FirebaseDB.firebaseClient.Child(Connection.ChatID).AsObservable<MessageModel>().Subscribe((dbevent) =>
            {
                if (dbevent.Object != null && !string.IsNullOrEmpty(dbevent.Object.Message))
                {
                    Messages.Insert(0,dbevent.Object);
                }
            });
        }
    }
}
