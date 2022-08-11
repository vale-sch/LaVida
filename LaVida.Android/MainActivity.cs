using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android;
using Xamarin.Forms;
using LaVida.Helpers;

namespace LaVida.Droid
{
    [Activity(Label = "LaVida", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        readonly string[] ContactPermisison =
     {
            Manifest.Permission.ReadContacts,
            Manifest.Permission.WriteContacts
        };
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
            MessagingCenter.Subscribe<DeviceIDMessage>(this, "GetDeviceID", message =>
            {
               message.DeviceID = Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);

            });
            RequestPermissions(ContactPermisison, 0);
        }
    }
}

