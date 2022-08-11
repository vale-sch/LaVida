using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace LaVida.ViewModels
{
    public  class ContactCore
    {
        private readonly ObservableCollection<Contact> ContactsCollect = new ObservableCollection<Contact>();

        public ContactCore()
        {
            Task.Run(async () => { await GetContactCollection(); });
         
        }

        public ObservableCollection<Contact> GetContacts()
        {
            return ContactsCollect;
        }
        public  async Task GetContactCollection()
        {
            try
            {
                // cancellationToken parameter is optional
                var cancellationToken = default(CancellationToken);
                var contacts = await Contacts.GetAllAsync(cancellationToken);

                if (contacts == null)
                    return;

                foreach (var contact in contacts)
                    ContactsCollect.Add(contact);
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex);
            }
        }
    }
}