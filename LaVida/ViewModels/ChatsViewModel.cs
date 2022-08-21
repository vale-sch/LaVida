using Firebase.Database;
using LaVida.Models;
using LaVida.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using MongoDB.Bson;
using MongoDB.Driver;
using LaVida.Views;
using Xamarin.Forms;
using System.Diagnostics;

namespace LaVida.ViewModels
{
    public class ChatsViewModel : BaseViewModel
    {

        private RealTimeMessageStream _selectedChat;
        public ObservableCollection<RealTimeMessageStream> RealTimeMessages { get; set; }
        public Command AddConnectionCommand { get; set; }
        public Xamarin.Forms.Command<RealTimeMessageStream> ChatTapped { get; set; }
        private readonly Dictionary<RealTimeMessageStream, Page> chatPages = new Dictionary<RealTimeMessageStream, Page>();
        private static FirebaseDB firebaseDB;

        public static FirebaseDB FirebaseDB
        {
            get
            {
                if (firebaseDB == null)
                    firebaseDB = new FirebaseDB("https://lavida-b6aca-default-rtdb.europe-west1.firebasedatabase.app/");

                return firebaseDB;
            }
        }
        public ChatsViewModel()
        {
            RealTimeMessages = new ObservableCollection<RealTimeMessageStream>();
            LoadConnections();


            AddConnectionCommand = new Command(OnConnectionAdd);

            ChatTapped = new Xamarin.Forms.Command<RealTimeMessageStream>(OnConnectionSelected);

        }

        void LoadConnections()
        {
            foreach (var connection in App.myAccount.Connections)
                Device.InvokeOnMainThreadAsync(() =>
                {
                    RealTimeMessages.Add(new RealTimeMessageStream(connection, new List<MessageModel>()));
                });

        }
        /* void ExecuteLoadConnectionCommand()
        {
            IsBusy = true;

            try
            {
                foreach (var accountDB in MongoAccountDB.AccountsFromDB)
                    if (App.myAccount.Id == accountDB.Id)
                    {
                        App.myAccount = accountDB;
                        foreach (var connection in App.myAccount.Connections)
                        {
                            Boolean isAlreadyConnected = false;
                            foreach(var realTimeMsg in RealTimeMessages)
                                if(realTimeMsg.Connection == connection) isAlreadyConnected = true;

                            if (isAlreadyConnected) RealTimeMessages.Add(new RealTimeMessageStream(connection, new ObservableCollection<MessageModel>()));
                        }

                    }
               
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }*/
        private void OnConnectionAdd(object obj)
        {
            NavigationManager.NextPageWithBack(new PossibleNewChats());
        }
        public void OnAppearing()
        {
            SelectedConnection = null;
        }
        public RealTimeMessageStream SelectedConnection
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                OnConnectionSelected(value);
            }
        }
        void OnConnectionSelected(RealTimeMessageStream realTimeMessageStream)
        {
            if (realTimeMessageStream == null)
                return;
            if (!chatPages.ContainsKey(realTimeMessageStream))
                chatPages.Add(realTimeMessageStream, new ChatPage(realTimeMessageStream));
            foreach (var chatPage in chatPages)
                if (chatPage.Key == realTimeMessageStream)
                    NavigationManager.NextPageWithBack(chatPage.Value);
        }
    }
}
