using Firebase.Database;
using Firebase.Database.Query;
using LaVida.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace LaVida.Database
{
    public class FirebaseDB
    {
        private readonly FirebaseClient firebaseClient;

        public FirebaseDB(string dbPath)
        {
            Console.WriteLine("Try to connect to RealtimeDB...");

            try
            {
                firebaseClient = new FirebaseClient(dbPath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("...Connection established!");
        }
        public void SendMessageInStream(Connection connection, MessageModel message)
        {
            firebaseClient.Child(connection.ChatID).PostAsync(message, false);

        }
        public void StreamMessagesFromServer(Connection connection, List<MessageModel> messageModels)
        {
            firebaseClient.Child(connection.ChatID).AsObservable<MessageModel>().Subscribe((dbevent) =>
            {
                if (dbevent.Object != null && !string.IsNullOrEmpty(dbevent.Object.Message))
                {
                    messageModels.Add(dbevent.Object);
                }
            });
        }

    }
}
