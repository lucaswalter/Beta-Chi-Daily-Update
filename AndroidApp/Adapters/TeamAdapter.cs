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
    public class TeamAdapter : BaseAdapter<TeamItem>
    {

        private Activity context;
        private List<TeamItem> teams = new List<TeamItem>();
        
        public TeamAdapter(Activity context) : base()
        {
            this.context = context;
        }

        public void Add(TeamItem item)
        {
            teams.Add(item);
            NotifyDataSetChanged();
        }

        public void Clear()
        {
            teams.Clear();
            NotifyDataSetChanged();
        }

        public void Remove(TeamItem item)
        {
            teams.Remove(item);
            NotifyDataSetChanged();
        }

        public override TeamItem this[int position]
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
                view = context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem2, null);

            // TODO: Create Custom View To Display Name And Points
            view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = teams[position].TeamName;

            return view;
        }
    }
}