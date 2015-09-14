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
using Parse;

namespace AndroidApp.Adapters
{
    public class IMReminderAdapter : BaseAdapter<ParseObject>
    {

        private Activity context;
        private List<ParseObject> reminders = new List<ParseObject>();

        public IMReminderAdapter(Activity context)
        {
            this.context = context;
        }

        public void Add(ParseObject item)
        {
            reminders.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            reminders.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(ParseObject item)
        {
            reminders.Remove(item);
            NotifyDataSetChanged();
        }

        public override ParseObject this[int position]
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
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = reminders[position].Get<string>("Text");

            return view;
        }
    }
}