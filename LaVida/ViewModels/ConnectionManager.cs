using Firebase.Database;
using LaVida.Models;
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
    public class ConnectionManager: BaseViewModel
    {
  
        private Connection _selectedConnection;
        public ObservableCollection<Connection> Connections { get; }
        public Command LoadConnectionsCommand { get; }
        public Command AddConnectionCommand { get; }
        public Xamarin.Forms.Command<Connection> ConnectionTapped { get; }
        private readonly FirebaseClient firebaseClient;
        public ConnectionManager(FirebaseClient firebaseClient)
        {
            Connections = new ObservableCollection<Connection>();
            LoadConnectionsCommand = new Command(async () => await ExecuteLoadConnectionCommand());

            ConnectionTapped = new Xamarin.Forms.Command<Connection>(OnConnectionSelected);

            AddConnectionCommand = new Command(OnConnectionAdd);

           this.firebaseClient = firebaseClient;

        }
        async Task ExecuteLoadConnectionCommand()
        {
            IsBusy = true;

            try
            {
                Connections.Clear();
                var connections = await DataStore.GetItemsAsync(true);
                foreach (var connection in connections)
                {
                    Connections.Add(connection);
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
            _ = Device.InvokeOnMainThreadAsync(() =>
            {
                NavigationManager.NextPageWithBack(new PossibleNewChats());

            });
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
            //Console.WriteLine(connection.ChatPartner);
            if (connection == null)
                return;

            _ = Device.InvokeOnMainThreadAsync(() =>
            {
                NavigationManager.NextPageWithoutBack(new ChatPage(firebaseClient, connection));

            });
        }
      

    }
}
