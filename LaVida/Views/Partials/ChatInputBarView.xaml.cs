﻿using System;
using System.Collections.Generic;
using LaVida.ViewModels;
using Xamarin.Forms;

namespace LaVida.Views.Partials
{
    public partial class ChatInputBarView : ContentView
    {
        public ChatInputBarView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                this.SetBinding(HeightRequestProperty, new Binding("Height", BindingMode.OneWay, null, null, null, chatTextInput));
            }
        }
        public void Handle_Completed(object sender, EventArgs e)
        {
            (this.Parent.Parent.BindingContext as ChatBackend).OnSendCommand.Execute(null);
            chatTextInput.Focus();
            chatTextInput.Text = String.Empty;
        }

        public void UnFocusEntry()
        {
            chatTextInput?.Unfocus();
        }

    }
}
