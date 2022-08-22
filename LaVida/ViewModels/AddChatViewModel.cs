using LaVida.Models;
using LaVida.Services;
using LaVida.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace LaVida.ViewModels
{


    public class AddChatViewModel : BaseViewModel
    {
        private RealTimeMessageStream _selectedChat;
        public ObservableCollection<RealTimeMessageStream> RealTimeMessages { get; set; }
        public Command AddConnectionCommand { get; set; }
        public Xamarin.Forms.Command<RealTimeMessageStream> ChatTapped { get; set; }
        private AddChatsPage _page;
        void LoadConnections()
        {
            foreach (var connection in App.myAccount.Connections)
                if (!connection.IsActive)
                    Device.InvokeOnMainThreadAsync(() =>
                    {
                        RealTimeMessages.Add(new RealTimeMessageStream(connection, new List<MessageModel>()));
                    });

        }
        public AddChatViewModel(AddChatsPage addPage)
        {
            RealTimeMessages = new ObservableCollection<RealTimeMessageStream>();
            LoadConnections();
            _page = addPage;

            AddConnectionCommand = new Command(OnConnectionAdd);

            ChatTapped = new Xamarin.Forms.Command<RealTimeMessageStream>(OnConnectionSelected);

        }
        private void OnConnectionAdd(object obj)
        {
            NavigationManager.NextPageWithBack(new AddChatsPage());
        }
        public void OnAppearing()
        {
            SelectedConnection = null;
        }
        public RealTimeMessageStream SelectedConnection
        {
            get => _selectedChat;
            set
            {
                SetProperty(ref _selectedChat, value);
                OnConnectionSelected(value);
            }
        }
        async void OnConnectionSelected(RealTimeMessageStream realTimeMessageStream)
        {
            if (realTimeMessageStream == null || realTimeMessageStream.Connection.IsActive)
                return;

            realTimeMessageStream.Connection.IsActive = true;
            foreach (var connection in App.myAccount.Connections)
                if (connection == realTimeMessageStream.Connection)
                    connection.IsActive = true;
            await App.MongoDBDatabase.UpdateOneItem(App.myAccount);

            ChatsViewModel.chatPages.Add(realTimeMessageStream, new ChatPage(realTimeMessageStream));
            foreach (var chatPage in ChatsViewModel.chatPages)
                if (chatPage.Key == realTimeMessageStream)
                    NavigationManager.NextPageNewChat(_page, chatPage.Value);
        }
    }
}
