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
    public class MongoDBDatabase
    {
        public static IMongoCollection<Account> mongoCollection;

        private  readonly MongoClient Client;
        private  readonly IMongoDatabase Database;
        private  readonly string dbName = "AccountsDB";
        private  readonly string collectionName = "Accounts";

        public MongoDBDatabase(string dbPath)
        {
            try
            {
                var connectionString = dbPath;
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
        public  async Task InsertOne(Account accountToInsert)
        {
            await mongoCollection.InsertOneAsync(accountToInsert);
        }
        public  async Task UpdateOneItem(Account accountToUpdate)
        {
            await mongoCollection.ReplaceOneAsync(b => b.Id == accountToUpdate.Id, accountToUpdate);
        }
        public  async Task RemoveOneItem(Account accountToRemove)
        {
            await mongoCollection.DeleteOneAsync(a => a.Id == accountToRemove.Id);
        }
        public  async Task<List<Account>> GetAllAccounts()
        {
            return await mongoCollection
        .Find(new BsonDocument())
        .ToListAsync();
     
        }
        public  async Task <Account> GetAccountById(string accountId)
        {
            return await mongoCollection.Find(a => a.AccountID.Equals(accountId))
                .FirstOrDefaultAsync();
          
        }
    }
}
