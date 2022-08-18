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
        public Command LoadConnectionsCommand { get; set; }
        public Command AddConnectionCommand { get; set; }
        public Xamarin.Forms.Command<RealTimeMessageStream> ChatTapped { get; set; }
        public ChatsViewModel()
        {
            FirebaseDB.Connect();

            RealTimeMessages = new ObservableCollection<RealTimeMessageStream>();
            LoadConnections();
            LoadConnectionsCommand = new Command(async () => await ExecuteLoadConnectionCommand());


            AddConnectionCommand = new Command(OnConnectionAdd);

            ChatTapped = new Xamarin.Forms.Command<RealTimeMessageStream>(OnConnectionSelected);

        }
 
        void LoadConnections()
        {
            foreach (var connection in App.myAccount.Connections)
                Device.InvokeOnMainThreadAsync(() =>
                {
                    RealTimeMessages.Add(new RealTimeMessageStream(connection, new ObservableCollection<MessageModel>()));
                });

        }
        async Task ExecuteLoadConnectionCommand()
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
        }
        private void OnConnectionAdd(object obj)
        {

            NavigationManager.NextPageWithBack(new PossibleNewChats());
        }
        public void OnAppearing()
        {
            IsBusy = false;
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
            NavigationManager.NextPageWithBack(new ChatPage(realTimeMessageStream));
        }
    }
}
