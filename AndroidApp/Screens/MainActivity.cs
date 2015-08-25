using System;
using System.Linq;
using Android.OS;
using Android.App;
using Android.Views;
using Android.Widget;
using System.Net.Http;
using System.Threading.Tasks;
using Android.Content;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Parse;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChiActionBar")]
    public class MainActivity : Activity
    {
        // Parse Queries To Retrieve Entire Table
        // TODO: Possibly Optimize Or Use Job To Cleanup Data
        private ParseQuery<ParseObject> reminderTable;
        private ParseQuery<ParseObject> mealTable;

        // Adapter To Sync Reminders With The List
        private ReminderAdapter reminderAdapter;

        // Create Reminder List View
        private ListView reminderListView;

        // Progress Spinner For Tabler Operations
        private ProgressBar progressBar;

        // Date View
        private TextView dateTextView;

        // Meal Item
        private ParseObject mealItem;

        // Meal Text Views
        private TextView breakfastTextView;
        private TextView lunchTextView;
        private TextView dinnerTextView;

        // Sober Driver Button
        private Button soberDriverButton;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set View From Main Layout Resource
            SetContentView(Resource.Layout.MainActivity);

            // Set Title
            ActionBar.Title = "Daily Update";

            // Set Date Text View
            dateTextView = FindViewById<TextView>(Resource.Id.dateTextView);
            dateTextView.Text = DateTime.Today.ToShortDateString();

            // Initialize Progress Bar
            progressBar = FindViewById<ProgressBar>(Resource.Id.loadingProgressBar);
            progressBar.Visibility = ViewStates.Gone;

            // Create Adapter To Bind The Reminder Items To The View
            reminderAdapter = new ReminderAdapter(this);
            reminderListView = FindViewById<ListView>(Resource.Id.listViewRemindersMain);
            reminderListView.Adapter = reminderAdapter;

            // Create Progress Filter To Handle Busy State
            var progressHandler = new ProgressHandler();
            progressHandler.BusyStateChange += (busy) =>
            {
                if (progressBar != null)
                    progressBar.Visibility = busy ? ViewStates.Visible : ViewStates.Gone;
            };

            // Set Meal Text Views
            breakfastTextView = FindViewById<TextView>(Resource.Id.breakfastTextView);
            lunchTextView = FindViewById<TextView>(Resource.Id.lunchTextView);
            dinnerTextView = FindViewById<TextView>(Resource.Id.dinnerTextView);

            // Set Sober Driver Button
            soberDriverButton = FindViewById<Button>(Resource.Id.soberDriverButton);

            // Sober Driver Button
            soberDriverButton.Click += (object sender, EventArgs e) =>
            {
                // On Button Click, Attempt To Dial
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Call Sober Driver?");
                callDialog.SetPositiveButton("Call", delegate
                {
                    // Create Intent To Dial Phone
                    var callIntent = new Intent(Intent.ActionCall);
                    callIntent.SetData(Android.Net.Uri.Parse("tel:8168309808")); // TODO: Change Number
                    StartActivity(callIntent);
                });

                // Create Negative Button And Show Dialog
                callDialog.SetNegativeButton("Cancel", delegate { });
                callDialog.Show();
            };

            // Retrieve Data From Parse Database
            try
            {
                // Retrieve Tables
                reminderTable = ParseObject.GetQuery("Reminder");
                mealTable = ParseObject.GetQuery("Meal");
             

                // Load Data From The Mobile Service
                await RefreshRemindersFromTableAsync();
                await RefreshMealsFromTableAsync(DateTime.Today);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }   
        }

        /** Azure Mobile Retrieval Methods **/
        async void OnRefreshItemsSelected()
        {
            await RefreshRemindersFromTableAsync();
            await RefreshMealsFromTableAsync(DateTime.Today);
        }

        async Task RefreshRemindersFromTableAsync()
        {
            try
            {
                // Get Today's Reminders
                var query = reminderTable;
                var list = await query.FindAsync();
              
                // Clear Reminder Adapter
                reminderAdapter.Clear();

                // Add Reminders
                foreach (var current in list)
                {
                    var date = current.Get<DateTime>("Date");
                    if (date == DateTime.Today)
                        reminderAdapter.Add(current);
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        async Task RefreshMealsFromTableAsync(DateTime date)
        {
            try
            {
                // Retrieves Today's Meals
                var query = mealTable;
                var list = await query.FindAsync();

                foreach (var current in list)
                {
                    var currentDate = current.Get<DateTime>("Date");

                    if (date == currentDate)
                    {
                        mealItem = current;
                        breakfastTextView.Text = mealItem.Get<string>("Breakfast");
                        lunchTextView.Text = mealItem.Get<string>("Lunch");
                        dinnerTextView.Text = mealItem.Get<string>("Dinner");
                    }                      
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        /** Menu Selection Methods **/
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.HomeMenu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            // TODO: Remove Debugging Toast
            Toast.MakeText(this, "Menu Pressed: " + item.TitleFormatted, ToastLength.Short).Show();

            switch (item.ItemId)
            {
                case Resource.Id.menu_RefreshReminders:
                    OnRefreshItemsSelected();
                    return true;
                case Resource.Id.menu_View_IM:
                    Intent intent = new Intent(this, typeof(ViewIMActivity));
                    StartActivity(intent);
                    return true;
                case Resource.Id.menu_EditScribeData:
                    Console.WriteLine("Show Scribe Password Dialog");
                    CreateAndShowPasswordDialog(Constants.EDIT_SCRIBE_DATA, "scribe");
                    return true;
                case Resource.Id.menu_EditIMData:
                    Console.WriteLine("Show IM Password Dialog");
                    CreateAndShowPasswordDialog(Constants.EDIT_IM_DATA, "im");
                    return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        /** Password Dialog **/
        void CreateAndShowPasswordDialog(int activity, string password)
        {
            // Create Dialog And Transaction
            var transaction = FragmentManager.BeginTransaction();
            var passwordDialog = new PasswordDialogFragment();

            passwordDialog.ActivityID = activity;
            passwordDialog.Password = password;
            passwordDialog.Show(transaction, "passwordDialog");
        }

        /** Error Dialog **/
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

            #region implemented abstract members of HttpMessageHandler

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

