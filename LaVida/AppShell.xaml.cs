﻿using LaVida.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaVida
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            Routing.RegisterRoute(nameof(ChatPage), typeof(ChatPage));
        }

    }
}