using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Core;
using Microsoft.WindowsAzure.MobileServices;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Java.Text;

namespace AndroidApp.Screens
{
    [Activity(Label = "Beta-Chi Daily Update", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        // Mobile Service Client Reference
        private MobileServiceClient client;

        // Mobile Service Tables Used To Access Data
        private IMobileServiceTable<Day> dayTable;
        private IMobileServiceTable<ReminderItem> reminderTable;
        private IMobileServiceTable<MealItem> mealTable;
        private IMobileServiceTable<DriverItem> driverTable;

        // Adapter To Sync Reminders With The List
        // private ReminderAdapter reminderAdapter;

        // Progress Spinner For Tabler Operations
        private ProgressBar progressBar;

        // Date View
        private TextView dateTextView;

        // Sober Driver Button
        private Button soberDriverButton;

        const string applicationURL = "https://betachi.azure-mobile.net/";
        const string applicationKey = "SbsbuMkNyFvOnFVniZJbkrkjfEuUYr87";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set View From Main Layout Resource
            SetContentView(Resource.Layout.MainActivity);

            // Create Toolbar
            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetActionBar(toolbar);

            // Set Title
            ActionBar.Title = "Test Title";

            // Set Sber Driver Button
            soberDriverButton = FindViewById<Button>(Resource.Id.soberDriverButton);

            // Initialize Progress Bar
            progressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);
            progressBar.Visibility = ViewStates.Gone;

            // Create Progress Filter To Handle Busy State
            var progressHandler = new ProgressHandler();
            progressHandler.BusyStateChange += (busy) =>
            {
                if (progressBar != null)
                    progressBar.Visibility = busy ? ViewStates.Visible : ViewStates.Gone;
            };

            // Set Date Text View
            dateTextView = FindViewById<TextView>(Resource.Id.dateTextView);
            dateTextView.Text = DateTime.Today.ToShortDateString();

            // Connect To Azure Mobile Service
            try
            {
                // Initialize
                CurrentPlatform.Init();

                // Create Mobile Service Client Instance
                client = new MobileServiceClient(applicationURL, applicationKey);

                // Retrieve Tables
                dayTable = client.GetTable<Day>();
                reminderTable = client.GetTable<ReminderItem>();
                mealTable = client.GetTable<MealItem>();
                driverTable = client.GetTable<DriverItem>();




                // TODO: Set Sober Driver Button Text



            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }

            soberDriverButton.Click += (object sender, EventArgs e) =>
            {
                // On Button Click, Attempt To Dial
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call Sober Driver?");
                callDialog.SetNeutralButton("Call", delegate
                {
                    // Create Intent To Dial Phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:8168309808"));
                    StartActivity(callIntent);
                });

                // Create Negative Button And Show Dialog
                callDialog.SetNegativeButton("Cancel", delegate { });
                callDialog.Show();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.HomeMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            Toast.MakeText(this, "Menu Pressed: " + item.TitleFormatted, ToastLength.Short).Show();

            switch (item.ItemId)
            {
                //case Resource.Id
            }

            return base.OnOptionsItemSelected(item);
        }

        // TODO: Implement Retrieval & Sorting Methods

        /** Error Dialog Methods **/
        void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }

        /** Progress Handler Class **/
        class ProgressHandler : DelegatingHandler
        {
            int busyCount = 0;

            public event Action<bool> BusyStateChange;

            #region Implemented Abstract Members Of HttpMessageHandler

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, System.Threading.CancellationToken cancellationToken)
            {
                //assumes always executes on UI thread
                if (busyCount++ == 0 && BusyStateChange != null)
                    BusyStateChange(true);

                var response = await base.SendAsync(request, cancellationToken);

                // assumes always executes on UI thread
                if (--busyCount == 0 && BusyStateChange != null)
                    BusyStateChange(false);

                return response;
            }

            #endregion

        }
    }
}

