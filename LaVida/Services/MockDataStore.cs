using LaVida.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LaVida.Services
{
    public class MockDataStore : IDataStore<MessageModel>
    {
        readonly List<MessageModel> Messages;

        public MockDataStore()
        {
            Messages = new List<MessageModel>();
          
        }

        public async Task<bool> AddItemAsync(MessageModel item)
        {
            Messages.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(MessageModel item)
        {
            var oldItem = Messages.Where((MessageModel arg) => arg.Message == item.Message).FirstOrDefault();
            Messages.Remove(oldItem);
            Messages.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string Message)
        {
            var oldItem = Messages.Where((MessageModel arg) => arg.Message == Message).FirstOrDefault();
            Messages.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<MessageModel> GetItemAsync(string Message)
        {
            return await Task.FromResult(Messages.FirstOrDefault(s => s.Message == Message));
        }

        public async Task<IEnumerable<MessageModel>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(Messages);
        }
    }
}