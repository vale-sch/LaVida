using LaVida.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace LaVida
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute("login", typeof(TabBar));
            Routing.RegisterRoute("main", typeof(TabBar));
            InitializeComponent();
        }

    }
}
