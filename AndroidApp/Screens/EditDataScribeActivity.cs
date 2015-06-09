using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace AndroidApp.Screens
{
    [Activity(Label = "Edit Scribe Data")]
    public class EditDataScribeActivity : Activity
    {
        private DateTime selectedDate;
        private Button datePickerButton;

        protected override void OnCreate(Bundle bundle)
        {
            // Create And Display Layout
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditDataScribeActivity);

            datePickerButton = FindViewById<Button>(Resource.Id.buttonDatePicker);

            datePickerButton.Click += delegate
            {
                ShowDialog(0);
            };

            selectedDate = DateTime.Today;;
            datePickerButton.Text = selectedDate.ToShortDateString();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            return new DatePickerDialog(this, HandleDateSet, selectedDate.Year, selectedDate.Month - 1, selectedDate.Day);
        }

        void HandleDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            selectedDate = e.Date;
            datePickerButton.Text = selectedDate.ToShortDateString();
        }
    }
}