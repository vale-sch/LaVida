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
        private readonly FirebaseClient firebaseClient;
        private ObservableCollection<Contact> ContactsCollection = new ObservableCollection<Contact>();
        private Connection _selectedConneciton;
        public ObservableCollection<Connection> Connections { get; }
        public Command LoadItemsCommand { get; }
        public Command AddItemCommand { get; }
        public Xamarin.Forms.Command<Connection> ConnectionTapped { get; }

        public ConnectionManager()
        {
            Connections = new ObservableCollection<Connection>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ConnectionTapped = new Xamarin.Forms.Command<Connection>(OnItemSelected);

            AddItemCommand = new Command(OnAddItem);

            Task.Run(async () =>
            {
                await LoadNewConnections();
            });
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

        }
        async Task ExecuteLoadItemsCommand()
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
        private  void OnAddItem(object obj)
        {
           
        }
        public void OnAppearing()
        {
            IsBusy = true;
            SelectedConnection = null;
        }
        public Connection SelectedConnection
        {
            get => _selectedConneciton;
            set
            {
                SetProperty(ref _selectedConneciton, value);
                OnItemSelected(value);
            }
        }
         void OnItemSelected(Connection connection)
        {
            if (connection == null)
                return;

            _ = Device.InvokeOnMainThreadAsync(() =>
            {
                NavigationManager.NavigateToNextPage(new ChatPage(firebaseClient, _selectedConneciton));

            });
        }
        private async Task LoadNewConnections()
        {

            ContactsCollection = await ContactCore.GetContactCollection();
            foreach (var contactFromIntern in ContactsCollection)
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    foreach (var accountFromDB in App.AccountsFromDB)
                        if (WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber))
                        {
                            if (App.myAccount.PhoneNumber == phoneFromIntern.PhoneNumber) continue;

                            if (App.myAccount.Connections.Count > 0)
                            {
                                foreach (var existingConnecetion in App.myAccount.Connections)
                                    if ((phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString() == existingConnecetion.ChatID) continue;
                                _selectedConneciton = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT };
                                App.myAccount.Connections.Add(_selectedConneciton);
                                await App.mongoCollection.ReplaceOneAsync(b => b.Id == App.myAccount.Id, App.myAccount);
                            }
                            else
                            {

                                _selectedConneciton = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT };
                                App.myAccount.Connections.Add(_selectedConneciton);
                                await App.mongoCollection.ReplaceOneAsync(b => b.Id == App.myAccount.Id, App.myAccount);
                            }


                        }
        

        }

    }
}
