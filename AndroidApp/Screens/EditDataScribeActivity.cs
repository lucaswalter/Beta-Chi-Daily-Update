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
        private DateTime date;
        private Button datePickerButton;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.EditDataScribeActivity);

        }

        protected override Dialog OnCreateDialog(int id)
        {
            return new DatePickerDialog(this, HandleDateSet, date.Year, date.Month - 1, date.Day);
        }

        void HandleDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            date = e.Date;
            //var button = FindViewById<Button>(Resource.Id);
            //button.Text = date.ToShortTimeString();
        }
    }
}