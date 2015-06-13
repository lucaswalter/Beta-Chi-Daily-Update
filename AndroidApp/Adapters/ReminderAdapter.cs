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
using AndroidApp.Core;

namespace AndroidApp.Adapters
{
    public class ReminderAdapter : BaseAdapter<ReminderItem>
    {

        private ReminderItem[] reminders;
        private Activity context;

        public ReminderAdapter(Activity context, ReminderItem[] reminders) : base()
        {
            this.context = context;
            this.reminders = reminders;
        }

        public override ReminderItem this[int position]
        {
            get { return reminders[position]; }
        }

        public override int Count
        {
            get { return reminders.Length; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = reminders[position].Text;

            return view;
        }
    }
}