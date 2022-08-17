using LaVida.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LaVida.Services
{
    public  class SQLLocalDB
    {
        private readonly SQLiteAsyncConnection SQLClient;

        public SQLLocalDB(string dbPath)
        {
            SQLClient = new SQLiteAsyncConnection(dbPath);
            SQLClient.CreateTableAsync<Account>();
        }

        public  Task<List<Account>> GetMyAccount()
        {
            Console.WriteLine("WIR SIND DRIN");
            return SQLClient.Table<Account>().ToListAsync();

        }
        public  Task SaveMyAccount(Account account)
        {
            return SQLClient.InsertAsync(account);
        }

    }
}
