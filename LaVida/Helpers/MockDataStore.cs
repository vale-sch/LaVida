using LaVida.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaVida.Helpers
{
    public class MockDataStore : IDataStore<Connection>
    {
        public List<Connection> connections;

        public MockDataStore()
        {
          connections = new List<Connection>();
          
        }

        public async Task<bool> AddItemAsync(Connection connection)
        {
            connections.Add(connection);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Connection connection)
        {
            var oldItem = connections.Where((Connection arg) => arg.ChatID == connection.ChatID).FirstOrDefault();
            connections.Remove(oldItem);
            connections.Add(connection);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string chatId)
        {
            var oldItem = connections.Where((Connection arg) => arg.ChatID == chatId).FirstOrDefault();
            connections.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Connection> GetItemAsync(string chatId)
        {
            return await Task.FromResult(connections.FirstOrDefault(s => s.ChatID == chatId));
        }

        public async Task<IEnumerable<Connection>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(connections);
        }
    }
}