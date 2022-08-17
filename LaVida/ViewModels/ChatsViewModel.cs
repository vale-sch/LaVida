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
            FirebaseDB.Connect();

            Connections = new ObservableCollection<Connection>();
            LoadConnections();
            LoadConnectionsCommand = new Command(async () => await ExecuteLoadConnectionCommand());


            AddConnectionCommand = new Command(OnConnectionAdd);

            ConnectionTapped = new Xamarin.Forms.Command<Connection>(OnConnectionSelected);

        }
 
        void LoadConnections()
        {
            foreach (var connection in App.myAccount.Connections)
                Connections.Add(connection);
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
                            if(!Connections.Contains(connection))
                                Connections.Add(connection);
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
