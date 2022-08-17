﻿using LaVida.Helpers;
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
        public async Task InitializeConnectionsInDB()
        {
            ContactsCollection = await ContactCore.GetContactCollection();
            foreach (var contactFromIntern in ContactsCollection)
                foreach (var phoneFromIntern in contactFromIntern.Phones.ToArray())
                    foreach (var accountFromDB in MongoAccountDB.AccountsFromDB)
                    {
                        if (WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber) == WhiteSpace.RemoveWhitespace(accountFromDB.PhoneNumber) && WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber) != WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber))
                        {
                            var connection = new Connection() { ChatID = (phoneFromIntern.PhoneNumber + accountFromDB.PhoneNumber).GetHashCode().ToString(), ChatPartner = accountFromDB.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(phoneFromIntern.PhoneNumber), IsActive = false };
                            var connectionForPartner = new Connection() { ChatID = connection.ChatID, ChatPartner = App.myAccount.Name, ChatType = ChatType.PRIVATECHAT, ChatPhoneNumber = WhiteSpace.RemoveWhitespace(App.myAccount.PhoneNumber), IsActive = false };
                            App.myAccount.Connections.Add(connection);
                            accountFromDB.Connections.Add(connectionForPartner);
                            accountFromDB.HasToRefreshConnections = true;
                            await MongoAccountDB.UpdateOneItem(accountFromDB);
                            await MongoAccountDB.UpdateOneItem(App.myAccount);

                        }
                    }
        }
    }
}