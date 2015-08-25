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
    public class TeamAdapter : BaseAdapter<ParseObject>
    {

        private Activity context;
        private List<ParseObject> teams = new List<ParseObject>();
        
        public TeamAdapter(Activity context) : base()
        {
            this.context = context;
        }

        public void Add(ParseObject item)
        {
            teams.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            teams.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(ParseObject item)
        {
            teams.Remove(item);
            NotifyDataSetChanged();
        }

        public override ParseObject this[int position]
        {
            get { return teams[position]; }
        }

        public override int Count
        {
            get { return teams.Count; }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            View view = convertView;

            if (view == null)
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.TwoLineListItem, null);

            // TODO: Create Custom View To Display Name And Points
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = teams[position].Get<string>("TeamName");
            view.FindViewById<TextView>(Android.Resource.Id.Text2).Text = teams[position].Get<int>("Points") + " Points";

            return view;
        }
    }
}