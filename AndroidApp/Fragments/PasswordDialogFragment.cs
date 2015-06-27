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
using AndroidApp.Screens;

namespace AndroidApp.Fragments
{
    public class PasswordDialogFragment : DialogFragment
    {
        // Create Layout Properties
        private TextView TitleTextView;
        private EditText PasswordEditText;

        // TODO: Change Method Of Authentication For Data Entry
        public int ActivityID;
        public string Password;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Build Password Dialog
            var builder = new AlertDialog.Builder(Activity);

            // Get Layout Inflater
            var inflator = Activity.LayoutInflater;

            // Inflate Layout For Password Dialog
            var dialogView = inflator.Inflate(Resource.Layout.PasswordDialogFragment, null);

            // Initialize Properties
            TitleTextView = dialogView.FindViewById<TextView>(Resource.Id.textView_Password);
            PasswordEditText = dialogView.FindViewById<EditText>(Resource.Id.editText_Password);

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Login", HandlePositiveButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            // Determine Routing Activity
            switch (ActivityID)
            {
                // TODO: Add Real Password Verification
                case Constants.EDIT_SCRIBE_DATA:
                {
                    if (PasswordEditText.Text.Equals(Password))
                    {
                        // Launch New Activity
                        Console.WriteLine("Edit Scribe Password Correct");
                        Intent editScribeActivity = new Intent(Application.Context, typeof(EditDataScribeActivity));
                        StartActivity(editScribeActivity);
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Edit Scribe Data Password Wrong", ToastLength.Short).Show();
                        dialog.Dismiss();
                    }

                    break;
                }
                case Constants.EDIT_IM_DATA:
                {
                    if (PasswordEditText.Text.Equals(Password))
                    {
                        // Launch New Activity
                        Console.WriteLine("Edit IM Password Correct");
                        Intent editIMActivity = new Intent(Application.Context, typeof(EditDataIMStandingsActivity));
                        StartActivity(editIMActivity);
                    }
                    else
                    {
                        Toast.MakeText(Application.Context, "Edit IM Data Password Wrong", ToastLength.Short).Show();
                        dialog.Dismiss();
                    }

                    break;
                }
            }
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}