using LaVida.ViewModels;
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
            InitializeComponent();

            Routing.RegisterRoute("landing", typeof(TabBar));
            Routing.RegisterRoute("registration", typeof(TabBar));
            Routing.RegisterRoute("main", typeof(TabBar));
        }

    }
}
