using LaVida.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace LaVida.Services
{
    public static class MongoAccountDB
    {
        public static IMongoCollection<Account> mongoCollection;
        public static ObservableCollection<Account> AccountsFromDB = new ObservableCollection<Account>();
        public static Account accountFromDB = new Account();

        private static MongoClient Client;
        private static IMongoDatabase Database;
        private static readonly string dbName = "AccountsDB";
        private static readonly string collectionName = "Accounts";

        public static void Connect()
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



        }
        public static async Task InsertOne(Account accountToInsert)
        {
            await mongoCollection.InsertOneAsync(accountToInsert);
        }
        public static async Task UpdateOneItem(Account accountToUpdate)
        {
            await mongoCollection.ReplaceOneAsync(b => b.Id == accountToUpdate.Id, accountToUpdate);
        }
        public static async Task RemoveOneItem(Account accountToRemove)
        {
            await mongoCollection.DeleteOneAsync(a => a.Id == accountToRemove.Id);
        }
        public static async Task GetAllAccounts()
        {
            var allItems = await mongoCollection
        .Find(new BsonDocument())
        .ToListAsync();
            foreach(var account in allItems)
                AccountsFromDB.Add(account);
        }
        public static async Task GetAccountById(string accountId)
        {
            var singleAccount = await mongoCollection.Find(a => a.AccountID.Equals(accountId))
                .FirstOrDefaultAsync();
            accountFromDB = singleAccount;
        }
    }
}
