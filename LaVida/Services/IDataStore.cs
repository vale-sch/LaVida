using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LaVida.Services
{
    public interface IDataStore<T>
    {
        Task<bool> AddItemAsync(T Message);
        Task<bool> UpdateItemAsync(T Message);
        Task<bool> DeleteItemAsync(T Message);
        Task<T> GetItemAsync(T Message);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
