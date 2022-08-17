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

        private Connection _selectedConnection;
        public ObservableCollection<Connection> Connections { get; set; }
        public Command LoadConnectionsCommand { get; set; }
        public Command AddConnectionCommand { get; set; }
        public Xamarin.Forms.Command<Connection> ConnectionTapped { get; set; }
        public ChatsViewModel()
        {
            FirebaseRealTimeDB.Connect();

            Task.Run(async () =>
            {
                await StartChatRoutine();
            });


        }
        async Task StartChatRoutine()
        {
            Connections = new ObservableCollection<Connection>();
            LoadConnectionsCommand = new Command(async () => await ExecuteLoadConnectionCommand());


            AddConnectionCommand = new Command(OnConnectionAdd);
            foreach (var connection in App.myAccount.Connections)
            {
                if (!DataStore.Equals(connection))
                    await DataStore.AddItemAsync(connection);
            }
            ConnectionTapped = new Xamarin.Forms.Command<Connection>(OnConnectionSelected);


        }

        async Task ExecuteLoadConnectionCommand()
        {
            IsBusy = true;

            try
            {
                Connections.Clear();
                var connections = await DataStore.GetItemsAsync(true);
                foreach (var connection in connections)
                    Connections.Add(connection);

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
            IsBusy = true;
            SelectedConnection = null;
        }
        public Connection SelectedConnection
        {
            get => _selectedConnection;
            set
            {
                SetProperty(ref _selectedConnection, value);
                OnConnectionSelected(value);
            }
        }
        void OnConnectionSelected(Connection connection)
        {
            if (connection == null)
                return;
            Console.WriteLine("HALLLO");


            NavigationManager.NextPageWithBack(new ChatPage(connection));


        }


    }
}
