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


namespace AndroidApp.Fragments
{
    public class SetMealsDialogFragment : DialogFragment
    {
        // Create Layout Properties
        private EditText breakfastEditText;
        private EditText lunchEditText;
        private EditText dinnerEditText;

        // Public Meal Item
        public MealItem MealItem;

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
            breakfastEditText = View.FindViewById<EditText>(Resource.Id.editTextBreakfast);
            lunchEditText = View.FindViewById<EditText>(Resource.Id.editTextLunch);
            dinnerEditText = View.FindViewById<EditText>(Resource.Id.editTextDinner);

            // Set Text To Current Meal Values
            breakfastEditText.Text = MealItem.Breakfast;
            lunchEditText.Text = MealItem.Lunch;
            dinnerEditText.Text = MealItem.Dinner;

            // Set Positive & Negative Buttons
            builder.SetView(dialogView);
            builder.SetPositiveButton("Set Meals", HandlePositiveButtonClick);
            builder.SetNeutralButton("Clear Meals", HandleNeutralButtonClick);
            builder.SetNegativeButton("Cancel", HandleNegativeButtonClick);

            // Build And Return Dialog
            var dialog = builder.Create();
            return dialog;
        }

        private void HandlePositiveButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            MealItem.Breakfast = breakfastEditText.Text;
            MealItem.Lunch = lunchEditText.Text;
            MealItem.Dinner = dinnerEditText.Text;

            // TODO: Implement IsFormalDinner With Checkbox
            MealItem.IsFormalDinner = false;

            dialog.Dismiss();
        }

        private void HandleNeutralButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;

            MealItem.Breakfast = Constants.NO_MEAL_SET;
            MealItem.Lunch = Constants.NO_MEAL_SET;
            MealItem.Dinner = Constants.NO_MEAL_SET;

            MealItem.IsFormalDinner = false;

            dialog.Dismiss();
        }

        private void HandleNegativeButtonClick(object sender, DialogClickEventArgs e)
        {
            var dialog = (AlertDialog)sender;
            dialog.Dismiss();
        }
    }
}