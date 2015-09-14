using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using Parse;

namespace AndroidApp.Resources.layout
{
    public class ViewIMRemindersTabFragment : Fragment
    {
        // IM Reminder Table
        private ParseQuery<ParseObject> reminderTable;

        private ListView reminderListView;
        private IMReminderAdapter reminderAdapter;

        public override View OnCreateView (LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
            // Create Fragment View
            var view = inflater.Inflate(Resource.Layout.ViewIMRemindersTabFragment, container, false);

            // Create Adapter To Bind The Reminder Items To The View
            reminderAdapter = new IMReminderAdapter(this.Activity);
            reminderListView = view.FindViewById<ListView>(Resource.Id.listViewIMReminders);
            reminderListView.Adapter = reminderAdapter;

            // Retrieve Data From Parse Database
            try
            {
                // Retrieve Tables
                reminderTable = ParseObject.GetQuery("IMReminder");

                // Load Data From The Mobile Service
                RefreshReminders();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }

            return view;
        }

        /*public async override void OnResume()
        {
            await RefreshRemindersFromTableAsync();
            base.OnResume();
        }*/

        /** Retrieve IM Reminders **/
        async public void RefreshReminders()
        {
            await RefreshRemindersFromTableAsync();
        }

        async Task RefreshRemindersFromTableAsync()
        {
            try
            {
                // Get Today's Reminders
                var query = reminderTable;
                var list = await query.FindAsync();

                // Clear Reminder Adapter
                reminderAdapter.Clear();

                // Add Reminders
                foreach (var current in list)
                {
                    var date = current.Get<DateTime>("Date");
                    if (date == DateTime.Today)
                        reminderAdapter.Add(current);
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        /** Error Dialog **/
        void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this.Activity);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}