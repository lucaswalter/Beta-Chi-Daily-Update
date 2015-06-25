using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using Microsoft.WindowsAzure.MobileServices;

namespace AndroidApp.Resources.layout
{
    public class AddIMRemindersTabFragment : Fragment
    {
        // Mobile Service Client Reference
        private MobileServiceClient client;

        // Mobile Service Tables Used To Access Data
        private IMobileServiceTable<ReminderItem> reminderTable;

        // Adapter To Sync Reminders With The List
        private IMReminderAdapter reminderAdapter;

        // Create Reminder List View
        private ListView reminderListView;

        // Buttons
        private Button DateButton;
        private Button AddReminderButton;
        private Button SaveButton;

        // Date Event
        private DateTime selectedDate = DateTime.Today;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create Fragment View
            var view = inflater.Inflate(Resource.Layout.AddIMRemindersTabFragment, container, false);

            // Initialize View Layout
            DateButton = view.FindViewById<Button>(Resource.Id.buttonIMDate);
            DateButton.Text = selectedDate.ToShortDateString();


            AddReminderButton = view.FindViewById<Button>(Resource.Id.buttonAddIMReminder);
            SaveButton = view.FindViewById<Button>(Resource.Id.buttonIMSave);

            // Create Adapter To Bind The Reminder Items To The View
            reminderAdapter = new IMReminderAdapter(Activity.Parent);
            reminderListView = view.FindViewById<ListView>(Resource.Id.listViewAddIMReminders);
            reminderListView.Adapter = reminderAdapter;

            // TODO: Handle Button Presses
            // Connect To Azure Mobile Service
            try
            {
                // Initialize
                CurrentPlatform.Init();

                // Create Mobile Service Client Instance
                client = new MobileServiceClient(Constants.APPLICATION_URL, Constants.APPLICATION_KEY);

                // Retrieve Tables
                reminderTable = client.GetTable<ReminderItem>();

                // Load Data From The Mobile Service
                RefreshReminders();

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }   

            return view;
        }

        /** Azure Mobile Service Connection Methods **/
        async void RefreshReminders()
        {
            await RefreshRemindersFromTableAsync();
        }

        async Task RefreshRemindersFromTableAsync()
        {
            try
            {
                // Get Today's Reminders
                var list = await reminderTable.Where(x => x.Date.Day == DateTime.Today.Day 
                    && x.ShowOnHomeScreen == false).ToListAsync();

                // Clear Reminder Adapter
                reminderAdapter.Clear();

                // Add Reminders
                foreach (ReminderItem current in list)
                    reminderAdapter.Add(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        /** Error Dialog **/
        void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(Activity.Parent);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}