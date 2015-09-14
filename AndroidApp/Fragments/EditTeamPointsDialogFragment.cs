using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using Parse;

namespace AndroidApp.Fragments
{
    public class EditTeamPointsDialogFragment : DialogFragment
    {
        // Create Layout Properties
        private NumberPicker teamPointsNumberPicker;

        // Horrible Practice
        public ParseObject Team;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Build Password Dialog
            var builder = new AlertDialog.Builder(Activity);

            // Get Layout Inflater
            var inflator = Activity.LayoutInflater;

            // Inflate Layout For Password Dialog
            var dialogView = inflator.Inflate(Resource.Layout.EditTeamPointsDialogFragment, null);

            // Initialize Properties
            teamPointsNumberPicker = dialogView.FindViewById<NumberPicker>(Resource.Id.numberPickerTeamPoints);
            teamPointsNumberPicker.MinValue = 0;
            teamPointsNumberPicker.MaxValue = 500;

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Confirm", HandlePositiveButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            Team["Points"] = teamPointsNumberPicker.Value;

            dialog.Dismiss();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}