using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LaVida.ViewModels
{
    public static class NavigationManager
    {

        public static void NavigateToNextPage(Page page)
        {
            _ = App.Current.MainPage.Navigation.PushAsync(page);
            NavigationPage.SetHasBackButton(page, false);
        }
    }
}
