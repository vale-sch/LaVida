using Firebase.Database;
using LaVida.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace LaVida.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatsOverviewPage : ContentPage
    {
        readonly ChatsViewModel _connectionManager;

        public ChatsOverviewPage()
        {
            InitializeComponent();
            Title = "Chats";
            BindingContext = _connectionManager = new ChatsViewModel();

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _connectionManager.OnAppearing();
        }
    }
}