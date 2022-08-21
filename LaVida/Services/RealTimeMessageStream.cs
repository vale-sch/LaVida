using LaVida.Models;
using LaVida.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace LaVida.Services
{
    public class RealTimeMessageStream
    {
        public Connection Connection { get; set; }
        public List<MessageModel> Messages { get; set; } = new List<MessageModel>();

        public RealTimeMessageStream(Connection connection, List<MessageModel> messageModels)
        {
            this.Connection = connection;
            this.Messages = messageModels;
            StreamMessagesFromServer();
        }
        private void StreamMessagesFromServer()
        {

            ChatsViewModel.FirebaseDB.StreamMessagesFromServer(Connection, Messages);
        }
    }
}
