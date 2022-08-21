using LaVida.Helpers;
using LaVida.Models;
using LaVida.Services;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaVida.ViewModels
{

    internal class RegistrationViewModel : BaseViewModel
    {
        private ObservableCollection<Contact> ContactsCollection;
        public RegistrationViewModel()
        {
            ContactsCollection = new ObservableCollection<Contact>();
        }
        public async Task LoadPossibleConnectionsFromDB()
        {
            ContactsCollection = await ContactCore.GetContactCollection();
            foreach (var contactFromIntern in ContactsCollection)
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    if (WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber) == WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber))
                        App.myAccount.NamePhoneIntern = contactFromIntern.GivenName;


            await App.MongoDBDatabase.UpdateOneItem(App.myAccount);
            var accountsFromDB = await App.MongoDBDatabase.GetAllAccounts();
            foreach (var contactFromIntern in ContactsCollection)
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    foreach (var accountFromDB in accountsFromDB)
                    {
                        if (WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber) && WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber) != WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber))
                        {
                            var connection = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = contactFromIntern.GivenName, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber),AccountIDS = new List<string>() { accountFromDB.AccountID, App.myAccount.AccountID }, IsActive = false };
                            var connectionForPartner = new Connection() { ChatID = connection.ChatID, ChatPartner = App.myAccount.NamePhoneIntern, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber), AccountIDS = new List<string>() { accountFromDB.AccountID, App.myAccount.AccountID }, IsActive = false };
                            App.myAccount.Connections.Add(connection);
                            accountFromDB.Connections.Add(connectionForPartner);
                            accountFromDB.HasToRefreshConnections = true;
                            await App.MongoDBDatabase.UpdateOneItem(accountFromDB);
                            await App.MongoDBDatabase.UpdateOneItem(App.myAccount);
                        }
                    }
        }
    }
}
