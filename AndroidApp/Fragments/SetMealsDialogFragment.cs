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
    public class SetMealsDialogFragment : DialogFragment
    {
        // Create Layout Properties
        private EditText breakfastEditText;
        private EditText lunchEditText;
        private EditText dinnerEditText;

        // Public Meal Item
        public ParseObject MealItem;

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Build Set Meals Dialog
            var builder = new AlertDialog.Builder(Activity);

            // Get Layout Inflater
            var inflator = Activity.LayoutInflater;

            // Inflate Layout For Password Dialog
            var dialogView = inflator.Inflate(Resource.Layout.SetMealsDialogFragment, null);

            // Initialize Properties
            breakfastEditText = dialogView.FindViewById<EditText>(Resource.Id.editTextBreakfast);
            lunchEditText = dialogView.FindViewById<EditText>(Resource.Id.editTextLunch);
            dinnerEditText = dialogView.FindViewById<EditText>(Resource.Id.editTextDinner);

            // Set Text To Current Meal Values
            breakfastEditText.Hint = MealItem.Get<string>("Breakfast");
            lunchEditText.Hint = MealItem.Get<string>("Lunch");
            dinnerEditText.Hint = MealItem.Get<string>("Dinner");

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Set Meals", HandlePositiveButtonClick);
            // builder.SetNeutralButton("Clear Meals", HandleNeutralButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            if (breakfastEditText.Text != String.Empty)
                MealItem["Breakfast"] = breakfastEditText.Text;

            if (lunchEditText.Text != String.Empty)
                MealItem["Lunch"] = lunchEditText.Text;

            if (dinnerEditText.Text != String.Empty)
                MealItem["Dinner"] = dinnerEditText.Text;

            // TODO: Implement IsFormalDinner With Checkbox
            MealItem["IsFormalDinner"] = false;

            dialog.Dismiss();
        }

        // TODO: Currently Does Not Work
        // TODO: Need To Override Custom OnClickListener To Keep Dialog Open
        private void HandleNeutralButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog) sender;

            breakfastEditText.Text = String.Empty;
            lunchEditText.Text = String.Empty;
            dinnerEditText.Text = String.Empty;

            dialog.Wait();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}