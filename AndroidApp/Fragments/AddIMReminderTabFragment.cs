using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace AndroidApp.Resources.layout
{
    public class AddIMReminderTabFragment : Fragment
    {
        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create Fragment View
            var view = inflater.Inflate(Resource.Layout.AddIMReminderTabFragment, container, false);

            // TODO: Set Everything

            return view;
        }
    }
}