using LaVida.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace LaVida.Services
{
    public static class NavigationManager
    {

        public static void NextPageWithoutBack(Page page)
        {
            _ = App.Current.MainPage.Navigation.PushAsync(page);
            NavigationPage.SetHasBackButton(page, false);
        }
        public static void NextPageWithBack(Page page)
        {
            _ = App.Current.MainPage.Navigation.PushAsync(page);

        }
        public static void NextPageNewChat(Page oldAddPage, Page newChat)
        {
            App.Current.MainPage.Navigation.RemovePage(oldAddPage);
            _ = App.Current.MainPage.Navigation.PushAsync(newChat);


        }
    }
}
