using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Firebase.Database;
using LaVida.Helpers;
using LaVida.Models;
using LaVida.ViewModels;
using LaVida.Views;
using MongoDB.Bson;
using MongoDB.Driver;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
namespace LaVida
{
    public partial class App : Application
    {
        public static IMongoCollection<Account> mongoCollection;
        public static ObservableCollection<Account> AccountsFromDB = new ObservableCollection<Account>();
        public static DeviceIDMessage DeviceIdentifier = new DeviceIDMessage();
        public static Account myAccount;

        private MongoClient Client;
        private IMongoDatabase Database;
        private readonly string dbName = "AccountsDB";
        private readonly string collectionName = "Accounts";
        private FirebaseClient firebaseClient;
        private ObservableCollection<Contact> ContactsCollection = new ObservableCollection<Contact>();
        public App()
        {
            InitializeComponent();
            DependencyService.Register<MockDataStore>();
            MainPage = new NavigationPage(new MainPage());
            Connect();
        }
        private void Connect()
        {
            Task.Run(async () =>
            {
                await ConnectToAccount();
            });
        }
        private async Task ConnectToAccount()
        {
            try
            {
                var connectionString = "mongodb://LaVidaAdmin:pO85OZbNjw1iNxvV@ac-jhy5v3n-shard-00-00.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-01.x5tlyr9.mongodb.net:27017,ac-jhy5v3n-shard-00-02.x5tlyr9.mongodb.net:27017/?ssl=true&replicaSet=atlas-9uw66t-shard-0&authSource=admin&retryWrites=true&w=majority";
                Client = new MongoClient(connectionString);
                Database = Client.GetDatabase(dbName);

                mongoCollection = Database.GetCollection<Account>(collectionName);

                Console.WriteLine("Accounts DB Connection established!");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            MessagingCenter.Send(DeviceIdentifier, "GetDeviceID");
            var accounts = await GettAllAccountsFromDB();
            foreach (var account in accounts)
                AccountsFromDB.Add(account);



            while (DeviceIdentifier.DeviceID == null)
            {
                await Task.Delay(1);
            }


            Boolean isInDB = false;
            foreach (var accountFromDB in AccountsFromDB)
            {
                if (DeviceIdentifier.DeviceID == accountFromDB.AccountID)
                {
                    myAccount = accountFromDB;

                    isInDB = true;
                }
            }
            Console.WriteLine("Try to connect to RealtimeDB...");

            try
            {
                firebaseClient = new FirebaseClient("https://lavida-b6aca-default-rtdb.europe-west1.firebasedatabase.app/");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine("...Connection established!");

            if (isInDB)
            {

                await LoadNewConnections();

                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new ChatsOverviewPage(firebaseClient)); });
            }
            else
                _ = Device.InvokeOnMainThreadAsync(() => { NavigationManager.NextPageWithoutBack(new RegistrationPage(firebaseClient)); });


        }

        private async Task LoadNewConnections()
        {
            ContactsCollection = await ContactCore.GetContactCollection();
            bool hasNewConnection = false;
            foreach (var contactFromIntern in ContactsCollection)
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    foreach (var accountFromDB in App.AccountsFromDB)
                    {
                        hasNewConnection = true;
                        foreach (var connection in MockDataStore.connections)
                        {

                            if (WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber) && connection.ChatPhoneNumber == WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) || WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == myAccount.PhoneNumber)
                                hasNewConnection = false;

                        }


                        if (!hasNewConnection) return;
                        Boolean hasAlreadyConnection = false;
                        foreach (var alreadyConnected in myAccount.Connections)
                            if (WhiteSpace.RemoveWhitespace(alreadyConnected.ChatPhoneNumber) == WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber)) hasAlreadyConnection = true;
                        if (!hasAlreadyConnection && WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber) && WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber) != WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber))
                        {
                            Console.WriteLine("TREFFER");

                            var connection = new Connection();
                            var connectionForPartner = new Connection();
                            //existing connection partner

                            if (accountFromDB.Connections.Count > 0)
                            {
                                foreach (var existingConnection in accountFromDB.Connections)
                                    if (WhiteSpace.RemoveWhitespace(myAccount.PhoneNumber) == WhiteSpace.RemoveWhitespace(existingConnection.ChatPhoneNumber))
                                    {
                                        connection = new Connection() { ChatID = existingConnection.ChatID, ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber), IsActive = false };
                                        Console.WriteLine("EXISTINGCONNECTION-PARTNER");
                                        App.myAccount.Connections.Add(connection);
                                        await App.mongoCollection.ReplaceOneAsync(b => b.Id == App.myAccount.Id, App.myAccount);
                                    }

                            }
                            //existing connection self

                            else if (App.myAccount.Connections.Count > 0)
                            {

                                foreach (var existingConnection in App.myAccount.Connections)
                                    if (WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == existingConnection.ChatPhoneNumber)
                                    {
                                        Console.WriteLine("EXISTINGCONNECTION-SELF");

                                        connection = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber), IsActive = false };
                                        connectionForPartner = new Connection() { ChatID = connection.ChatID, ChatPartner = myAccount.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(myAccount.PhoneNumber), IsActive = false };
                                        App.myAccount.Connections.Add(connection);
                                        accountFromDB.Connections.Add(connectionForPartner);
                                        await App.mongoCollection.ReplaceOneAsync(b => b.Id == accountFromDB.Id, accountFromDB);
                                        await App.mongoCollection.ReplaceOneAsync(b => b.Id == App.myAccount.Id, App.myAccount);
                                    }


                            }
                            //no connection at this time
                            else
                            {
                                Console.WriteLine("NO EXISTING CONNECTION");
                                connection = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber), IsActive = false };
                                connectionForPartner = new Connection() { ChatID = connection.ChatID, ChatPartner = myAccount.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(myAccount.PhoneNumber), IsActive = false };
                                App.myAccount.Connections.Add(connection);
                                accountFromDB.Connections.Add(connectionForPartner);
                                await App.mongoCollection.ReplaceOneAsync(b => b.Id == accountFromDB.Id, accountFromDB);
                                await App.mongoCollection.ReplaceOneAsync(b => b.Id == App.myAccount.Id, App.myAccount);
                            }
                        }
                    }
            foreach (var connection in myAccount.Connections)
            {
                if (!MockDataStore.connections.Contains(connection))
                    MockDataStore.connections.Add(connection);
            }
            Console.WriteLine("MOCKETDATALENGTH: " + MockDataStore.connections.Count);
        }

        public async Task<List<Account>> GettAllAccountsFromDB()
        {
            try
            {
                var allItems = await mongoCollection
           .Find(new BsonDocument())
           .ToListAsync();
                return allItems;

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);


            }
            return null;
        }
        protected override void OnStart()
        {
            // MainPage.Navigation.PushModalAsync(new ChatPage());
        }

        protected override void OnSleep()
        {

        }

        protected override void OnResume()
        {

        }
    }
}
