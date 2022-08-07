using LaVida.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace LaVida.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}