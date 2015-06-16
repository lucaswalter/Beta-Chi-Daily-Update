using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;

namespace AndroidApp.Fragments
{
    public class AddReminderDialogFragment : DialogFragment
    {

        // Create Layout Properties
        private EditText ReminderEditText;

        public ScribeReminderAdapter reminderAdapter;
        public DateTime date;


        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Build Password Dialog
            var builder = new AlertDialog.Builder(Activity);

            // Get Layout Inflater
            var inflator = Activity.LayoutInflater;

            // Inflate Layout For Password Dialog
            var dialogView = inflator.Inflate(Resource.Layout.AddReminderDialogFragment, null);

            // Initialize Properties
            ReminderEditText = dialogView.FindViewById<EditText>(Resource.Id.editTextReminder);

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Add Reminder", HandlePositiveButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            ReminderItem reminder = new ReminderItem();
            reminder.Date = date;
            reminder.Text = ReminderEditText.Text;
            
            reminderAdapter.Add(reminder);
            dialog.Dismiss();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}