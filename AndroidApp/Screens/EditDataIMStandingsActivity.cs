using System;
using System.Threading.Tasks;
using Android.App;
using Android.OS;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Microsoft.WindowsAzure.MobileServices;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChi")]
    public class EditDataIMStandingsActivity : Activity
    {

        // Mobile Service Client Reference
        private MobileServiceClient client;

        // Mobile Service Tables Used To Access Data
        private IMobileServiceTable<TeamItem> teamTable;

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
                    if (teamAdapter[i].Points == -1)
                    {
                        teamAdapter[i].Points = 0;
                        AddTeamItem(teamAdapter[i]);
                    }                     
                }
            };

            // Connect To Azure Mobile Service
            try
            {
                // Initialize
                CurrentPlatform.Init();

                // Create Mobile Service Client Instance
                client = new MobileServiceClient(Constants.APPLICATION_URL, Constants.APPLICATION_KEY);

                // Retrieve Tables
                teamTable = client.GetTable<TeamItem>();

                // Load Data From The Mobile Service
                await RefreshTeamsFromTableAsync();

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }   
        }

        // Retrieve Standing Data
        async Task RefreshTeamsFromTableAsync()
        {
            try
            {
                // Get Today's Reminders
                var list = await teamTable.OrderBy(x => x.Points).ToListAsync();

                // Clear Reminder Adapter
                teamAdapter.Clear();

                // Add Reminders
                foreach (TeamItem current in list)
                    teamAdapter.Add(current);

            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        public async void AddTeamItem(TeamItem item)
        {
            try
            {
                await teamTable.InsertAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Insert Team");
            }
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