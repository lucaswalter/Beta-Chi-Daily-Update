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
using AndroidApp.Resources.layout;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChiActionBar")]
    public class ViewIMActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            // Initial Activity Setup
            base.OnCreate(bundle);
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.Title = String.Empty;
            SetContentView(Resource.Layout.ViewIMActivity);

            // Create IM Reminder Tab
            ActionBar.Tab tab = ActionBar.NewTab();
            tab.SetText("IM Reminders");
            tab.TabSelected += (sender, args) =>
            {
                args.FragmentTransaction.Replace(Resource.Id.fragmentContainer, new ViewIMRemindersTabFragment());
            };

            ActionBar.AddTab(tab);

            // Create IM Leaderboard Tab
            tab = ActionBar.NewTab();
            tab.SetText("IM Standings");
            tab.TabSelected += (sender, args) =>
            {
                args.FragmentTransaction.Replace(Resource.Id.fragmentContainer, new ViewIMStandingsTabFragment());
            };

            ActionBar.AddTab(tab);
        }
    }
}