﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Firebase.Database;
using Firebase.Database.Query;
using LaVida.Models;
using LaVida.Services;
using LaVida.Views;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace LaVida.ViewModels
{
    public class ChatPageViewModel : BaseViewModel
    {

        public bool LastMessageVisible { get; set; } = true;
        public int PendingMessageCount { get; set; } = 0;

        public Queue<MessageModel> DelayedMessages { get; set; } = new Queue<MessageModel>();

        public ObservableCollection<MessageModel> Messages { get; set; } = new ObservableCollection<MessageModel>();
        public string TextToSend { get; set; }
        public ICommand OnSendCommand { get; set; }
        public ICommand OnBackButton => new Command(() =>
            {
                Console.WriteLine("HELLO I AM HERE");
                NavigationManager.NextPageWithoutBack(new ChatsOverviewPage());
            });
        //  public ICommand MessageAppearingCommand { get; set; }
        //  public ICommand MessageDisappearingCommand { get; set; }
        private readonly RealTimeMessageStream MessageStream;
        private readonly int MessagesAmountOnScrollOrigin = 0;
        private int ToBeRenderedMessageFactor = 0;
        public ChatPageViewModel(RealTimeMessageStream _messageStream)
        {
            MessageStream = _messageStream;
            //  MessageAppearingCommand = new Command<MessageModel>(OnMessageAppearing);
            // MessageDisappearingCommand = new Command<MessageModel>(OnMessageDisappearing);
            OnSendCommand = new Command(() =>
            {

                if (!string.IsNullOrEmpty(TextToSend))
                {
                    ChatsViewModel.FirebaseDB.SendMessageInStream(MessageStream.Connection, new MessageModel() { Message = DateTime.Now.ToString("HH:mm") + "\n" + TextToSend, UserName = App.myAccount.Name, DateTime = DateTime.Now });
                    TextToSend = String.Empty;
                }

            });
            MessagesAmountOnScrollOrigin = ChatPage.ScrollingFactor;
            ToBeRenderedMessageFactor = ChatPage.ScrollingFactor;
            GetMessagesFromStream();

        }
        private bool hasScrolledUp = false;

        private bool GetMessagesFromStream()
        {

            Device.BeginInvokeOnMainThread(async () =>
            {



                if (ToBeRenderedMessageFactor + 9 <= ChatPage.ScrollingFactor)
                {
                    await Task.Delay(300);
                    hasScrolledUp = true;
                    ToBeRenderedMessageFactor = ChatPage.ScrollingFactor;
                }

                if (DataStore.GetItemsAsync().Result.Count() <= MessageStream.Messages.Count)
                {
                    if (!hasScrolledUp)
                        foreach (var renderedMessage in MessageStream.Messages.Skip(Math.Max(0, MessageStream.Messages.Count - ToBeRenderedMessageFactor)))
                        {
                            if (!DataStore.GetItemsAsync().Result.Contains(renderedMessage))
                            {
                                if (DataStore.GetItemsAsync().Result.Count() >= ToBeRenderedMessageFactor)
                                {
                                    await DataStore.DeleteItemAsync(Messages.ElementAt(Messages.Count - 1));
                                    Messages.RemoveAt(Messages.Count - 1);
                                }


                                // if (LastMessageVisible)
                                // {
                                Messages.Insert(0, renderedMessage);
                                await DataStore.AddItemAsync(renderedMessage);
                                // }
                                // else
                                // {
                                //      Messages.Insert(0, renderedMessage);
                                //      await DataStore.AddItemAsync(renderedMessage);
                                //     PendingMessageCount++;
                                //  }

                            }
                        }
                    else
                    {
                        foreach (var renderedMessage in MessageStream.Messages.Skip(Math.Max(0, MessageStream.Messages.Count - ToBeRenderedMessageFactor)).Reverse())
                        {
                            if (!DataStore.GetItemsAsync().Result.Contains(renderedMessage))
                            {
                                // if (LastMessageVisible)
                                // {
                                Messages.Insert(Messages.Count, renderedMessage);
                                await DataStore.AddItemAsync(renderedMessage);
                                //  }
                                // else
                                // {
                                //    Messages.Insert(Messages.Count, renderedMessage);
                                //    await DataStore.AddItemAsync(renderedMessage);
                                //     PendingMessageCount++;
                                // }
                            }
                        }
                    }
                }
                if (ChatPage.ScrollingFactor == MessagesAmountOnScrollOrigin && hasScrolledUp)
                {
                    await Task.Delay(500);
                    foreach (var renderedMessage in MessageStream.Messages.Skip(Math.Max(0, MessageStream.Messages.Count - ToBeRenderedMessageFactor)))
                    {
                        if (DataStore.GetItemsAsync().Result.Count() > MessagesAmountOnScrollOrigin)
                        {
                            await DataStore.DeleteItemAsync(Messages.ElementAt(Messages.Count - 1));
                            Messages.RemoveAt(Messages.Count - 1);
                        }
                    }

                    hasScrolledUp = false;
                    ToBeRenderedMessageFactor = MessagesAmountOnScrollOrigin;
                }
                await Task.Delay(50);
                GetMessagesFromStream();
            });
            return true;
        }
        /*  void OnMessageAppearing(MessageModel message)
          {
              var idx = Messages.IndexOf(message);
              if (idx <= 6)
              {
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      while (DelayedMessages.Count > 0)
                      {
                          Messages.Insert(Messages.Count, DelayedMessages.Dequeue());
                      }
                      LastMessageVisible = true;
                      PendingMessageCount = 0;
                  });
              }
          }

          void OnMessageDisappearing(MessageModel message)
          {
              var idx = Messages.IndexOf(message);
              if (idx >= 6)
              {
                  Device.BeginInvokeOnMainThread(() =>
                  {
                      LastMessageVisible = false;
                  });

              }
          }*/
    }
}
