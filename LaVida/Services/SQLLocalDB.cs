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
            SQLClient.CreateTableAsync<SQLAccount>().Wait();
        }

        public  Task<List<SQLAccount>> GetMyAccount()
        {
            return SQLClient.Table<SQLAccount>().ToListAsync();

        }
        public  Task SaveMyAccount(SQLAccount account)
        {
            return SQLClient.InsertAsync(account);
        }

    }
}
