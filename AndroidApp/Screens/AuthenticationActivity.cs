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
using Android.Provider;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Java.Lang;
using Parse;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChiActionBar")]
    public class AuthenticationActivity : Activity
    {
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
            buttonVerify.Click += (object sender, EventArgs e) =>
            {
                // Attempt To Authenticate The User By Email
                VerifyUser();
            };
        }

        public async void VerifyUser()
        {
            // Retrieve Valid Users
            var query = ParseObject.GetQuery("Members").Limit(1000);

            var users = await query.FindAsync();

            var bIsValid = false;

            // Check If Current User Is Authenticated
            foreach (ParseObject current in users)
            {
                var email = current.Get<string>("Email");

                if (email.ToLower().Trim() == editTextEmail.Text.ToLower().Trim())
                {
                    bIsValid = true;
                    Authenticate();
                }              
            }

            if (!bIsValid)
                Toast.MakeText(this, "Insufficient Permissions", ToastLength.Long).Show();
        }

        public void Authenticate()
        {
            // Create User Setting
            var prefs = Application.Context.GetSharedPreferences("BetaChi", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutBoolean("UserAuth", true);
            prefEditor.Commit();

            // Launch Main Activity
            StartActivity(typeof(MainActivity));
        }
    }
}