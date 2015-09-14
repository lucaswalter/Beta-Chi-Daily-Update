using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Parse;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChi")]
    public class EditDataIMStandingsActivity : Activity
    {
        // Mobile Service Tables Used To Access Data
        private ParseQuery<ParseObject> teamTable;

        // Adapter To Sync Teams With The List
        private TeamAdapter teamAdapter;

        // Create Team List View
        private ListView teamListView;

        // Add Team Button
        private Button addTeamButton;

        // Save Changes Button
        private Button saveButton;

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditDataIMStandingsActivity);

            // Create Adapter To Bind The Reminder Items To The View
            teamAdapter = new TeamAdapter(this);
            teamListView = FindViewById<ListView>(Resource.Id.listViewEditIMStandings);
            teamListView.Adapter = teamAdapter;
            RegisterForContextMenu(teamListView);

            // Add Team Button
            addTeamButton = FindViewById<Button>(Resource.Id.buttonAddTeam);
            addTeamButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowAddTeamDialog();
            };

            // Save Changes Button
            saveButton = FindViewById<Button>(Resource.Id.buttonSaveTeamChanges);
            saveButton.Click += (object sender, EventArgs e) =>
            {
                for (int i = 0; i < teamAdapter.Count; i++)
                {
                    AddTeamItem(teamAdapter[i]);
                }

                RefreshTeams();
            };

            // Connect Parse Backend
            try
            {
                // Retrieve Tables
                teamTable = ParseObject.GetQuery("Team");

                // Load Data From The Mobile Service
                await RefreshTeamsFromTableAsync();

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }   
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            // If Created From Team List View
            if (v.Id == Resource.Id.listViewEditIMStandings)
            {
                var info = (AdapterView.AdapterContextMenuInfo)menuInfo;
                var menuItems = Resources.GetStringArray(Resource.Array.TeamContextMenu);

                for (int i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo)item.MenuInfo;
            var menuItems = Resources.GetStringArray(Resource.Array.TeamContextMenu);
            var menuItemIndex = item.ItemId;
            var menuItemName = menuItems[menuItemIndex];

            switch (menuItemName)
            {
                case "Points":
                    CreateAndShowEditTeamPointsDialog(teamAdapter[info.Position]);
                    UpdateTeamItem(teamAdapter[info.Position]);
                    return true;
                case "Delete":
                    RemoveTeamItem(teamAdapter[info.Position]);
                    return true;
            }

            return base.OnContextItemSelected(item);
        }

        // Retrieve Standing Data
        async Task RefreshTeamsFromTableAsync()
        {
            try
            {
                // Get Today's Reminders
                var query = teamTable;
                var list = await query.FindAsync();

                // Clear Reminder Adapter
                teamAdapter.Clear();

                // Add Reminders
                foreach (ParseObject current in list)
                    teamAdapter.Add(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        async void RefreshTeams()
        {
            await RefreshTeamsFromTableAsync();
        }

        public async void AddTeamItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Insert Team");
            }
        }

        public async void UpdateTeamItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Update Team");
            }
        }

        public async void RemoveTeamItem(ParseObject item)
        {
            try
            {
                await item.DeleteAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Remove Team");
            }

            teamAdapter.Remove(item);
        }

        /** Add Team Dialog **/
        void CreateAndShowAddTeamDialog()
        {
            var transaction = FragmentManager.BeginTransaction();
            var addTeamDialog = new AddTeamDialogFragment();

            // Horrible Practice
            addTeamDialog.TeamAdapter = teamAdapter;
            addTeamDialog.Show(transaction, "addTeamDialog");
        }

        /** Edit Team Points Dialog **/

        void CreateAndShowEditTeamPointsDialog(ParseObject team)
        {
            var transaction = FragmentManager.BeginTransaction();
            var editTeamPointsDialog = new EditTeamPointsDialogFragment();

            // Horrible Practice
            // TODO: Switch To Team Adapter
            editTeamPointsDialog.Team = team;
            editTeamPointsDialog.Show(transaction, "editTeamPointsDialog");
        }

        /** Error Dialog **/
        void CreateAndShowDialog(Exception exception, String title)
        {
            CreateAndShowDialog(exception.Message, title);
        }

        void CreateAndShowDialog(string message, string title)
        {
            AlertDialog.Builder builder = new AlertDialog.Builder(this);

            builder.SetMessage(message);
            builder.SetTitle(title);
            builder.Create().Show();
        }
    }
}