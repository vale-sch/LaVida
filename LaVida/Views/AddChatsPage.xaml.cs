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
    public partial class AddChatsPage : ContentPage
    {
        readonly AddChatViewModel _addChatViewModel;

        public AddChatsPage()
        {
            InitializeComponent();
            Title = "Add Chat";
            BindingContext = _addChatViewModel = new AddChatViewModel(this);

        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            _addChatViewModel.OnAppearing();
        }
    }
}