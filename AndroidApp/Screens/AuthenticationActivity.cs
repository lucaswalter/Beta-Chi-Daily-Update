using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Parse;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChiActionBar")]
    public class AuthenticationActivity : Activity
    {
        // Parse Table For All Users
        private ParseQuery<ParseObject> userTable;

        // Email Edit Text
        private EditText editTextEmail;

        // Verification Button
        private Button buttonVerify;

        protected override void OnCreate(Bundle bundle)
        {
            // Create And Display Layout
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.AuthenticationActivity);

            LinearLayout baseLayout = FindViewById<LinearLayout>(Resource.Id.BackgroundLinearLayout);
            baseLayout.SetBackgroundColor(Color.White);

            editTextEmail = FindViewById<EditText>(Resource.Id.editTextEmail);
            editTextEmail.Hint = "Enter Your School Email Address";

            buttonVerify = FindViewById<Button>(Resource.Id.buttonVerify);
        }
    }
}