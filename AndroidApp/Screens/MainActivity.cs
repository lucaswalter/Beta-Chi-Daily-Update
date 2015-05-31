using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace AndroidApp.Screens
{
    [Activity(Label = "Beta-Chi Daily Update", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set View From Main Layout Resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

