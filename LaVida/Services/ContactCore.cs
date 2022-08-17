using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LaVida.Services
{
    public  class ContactCore
    {

        public ContactCore()
        {
         
        }

        public static async Task <ObservableCollection<Contact>> GetContactCollection()
        {
            var newContactsCollection = new ObservableCollection<Contact>();
            try
            {
                // cancellationToken parameter is optional
                var cancellationToken = default(CancellationToken);
                var contacts = await Contacts.GetAllAsync(cancellationToken);

                if (contacts == null)
                    return null;

                foreach (var contact in contacts)
                    newContactsCollection.Add(contact);

                return newContactsCollection;
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex);
            }
            return newContactsCollection;
        }
    }
}