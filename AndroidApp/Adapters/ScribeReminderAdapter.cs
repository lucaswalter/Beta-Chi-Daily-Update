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
    public class ScribeReminderAdapter : BaseAdapter<ReminderItem>
    {

        private Activity context;
        private List<ReminderItem> reminders = new List<ReminderItem>();

        public ScribeReminderAdapter(Activity context) : base()
        {
            this.context = context;
        }

        public void Add(ReminderItem item)
        {
            reminders.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            reminders.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(ReminderItem item)
        {
            reminders.Remove(item);
            NotifyDataSetChanged();
        }

        public override ReminderItem this[int position]
        {
            get { return reminders[position]; }
        }

        public override int Count
        {
            get { return reminders.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleSelectableListItem, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = reminders[position].Text;

            return view;
        }
    }
}