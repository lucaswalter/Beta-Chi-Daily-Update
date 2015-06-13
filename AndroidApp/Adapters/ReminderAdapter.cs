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
            get { throw new NotImplementedException(); }
        }

        public override int Count
        {
            get { throw new NotImplementedException(); }
        }

        public override long GetItemId(int position)
        {
            throw new NotImplementedException();
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            throw new NotImplementedException();
        }
    }
}