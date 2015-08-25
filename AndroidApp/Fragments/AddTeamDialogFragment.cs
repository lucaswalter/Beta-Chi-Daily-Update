using System;
using System.Collections.Generic;
using System.Linq;
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
using Parse;


namespace AndroidApp.Fragments
{
    public class AddTeamDialogFragment : DialogFragment
    {
        // Create Layout Properties
        private EditText teamNameEditText;

        // Horrible Practice
        public TeamAdapter TeamAdapter;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Build Password Dialog
            var builder = new AlertDialog.Builder(Activity);

            // Get Layout Inflater
            var inflator = Activity.LayoutInflater;

            // Inflate Layout For Password Dialog
            var dialogView = inflator.Inflate(Resource.Layout.AddTeamDialogFragment, null);

            // Initialize Properties
            teamNameEditText = dialogView.FindViewById<EditText>(Resource.Id.editTextAddTeam);

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Add Team", HandlePositiveButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            ParseObject team = new ParseObject("Team");

            team["TeamName"] = teamNameEditText.Text;
            team["Points"] = -1;
            TeamAdapter.Add(team);

            dialog.Dismiss();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}