using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp.Adapters;
using AndroidApp.Core;
using AndroidApp.Fragments;
using Microsoft.WindowsAzure.MobileServices;
using Parse;

namespace AndroidApp.Screens
{
    [Activity(Theme = "@style/Theme.BetaChi")]
    public class EditDataScribeActivity : Activity
    {
        // Parse Queries To Retrieve Entire Table
        // TODO: Possibly Optimize Or Use Job To Cleanup Data
        private ParseQuery<ParseObject> reminderTable;
        private ParseQuery<ParseObject> mealTable;

        // Adapter To Sync Reminders With The List
        private ReminderAdapter reminderAdapter;

        // Date Selection Variables
        private DateTime selectedDate;
        private Button datePickerButton;

        // Reminder List View
        private ListView reminderListView ;

        // Reminder Button
        private Button addReminderButton;

        // Meal Button
        private Button setMealButton;

        // Meal Property
        private ParseObject mealItem;

        // Save Button
        private Button saveButton;

        /** Create Activity **/
        protected override async void OnCreate(Bundle bundle)
        {         
            // Create And Display Layout
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.EditDataScribeActivity);

            // Date Picker Button And Functionality
            datePickerButton = FindViewById<Button>(Resource.Id.buttonDatePicker);
            datePickerButton.Click += delegate { ShowDialog(0); };
            selectedDate = DateTime.Today;;
            datePickerButton.Text = selectedDate.ToShortDateString();

            // Initialize Reminder Adapter
            reminderAdapter = new ReminderAdapter(this);

            // Initialize And Bind List View
            reminderListView = FindViewById<ListView>(Resource.Id.listViewRemindersScribe);
            reminderListView.Adapter = reminderAdapter;
            RegisterForContextMenu(reminderListView);

            // Set Meals Button
            setMealButton = FindViewById<Button>(Resource.Id.buttonSetMeals);
            setMealButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowSetMealsDialog();
            };

            // Add Reminder Button
            addReminderButton = FindViewById<Button>(Resource.Id.buttonAddReminder);
            addReminderButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowAddReminderDialog();
            };

            // Add Save Button
            saveButton = FindViewById<Button>(Resource.Id.buttonSave);
            saveButton.Click += (object sender, EventArgs e) =>
            {
                // TODO: Add Progress Bar For Saving Data
                // TODO: Possible Confirmation Dialog

                // Save Reminders
                for (int i = 0; i < reminderAdapter.Count; i++)
                {
                    var reminder = reminderAdapter[i];
                    AddReminderItem(reminder);
                }

                // Save Meal Item
                UpdateMealItem(mealItem);

            };

            // Connect To Parse Backend
            try
            {
                // Retrieve Tables
                reminderTable = ParseObject.GetQuery("Reminder");
                mealTable = ParseObject.GetQuery("Meal");

                // Load The Reminders From Parse
                await RefreshRemindersFromTableAsync(DateTime.Today);

                // Load Meals From Parse
                await RefreshMealsFromTableAsync(DateTime.Today);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        public override void OnCreateContextMenu(IContextMenu menu, View v, IContextMenuContextMenuInfo menuInfo)
        {
            // If Created From Reminder List View
            if (v.Id == Resource.Id.listViewRemindersScribe)
            {
                var info = (AdapterView.AdapterContextMenuInfo) menuInfo;
                var menuItems = Resources.GetStringArray(Resource.Array.ReminderContextMenu);

                for (int i = 0; i < menuItems.Length; i++)
                    menu.Add(Menu.None, i, i, menuItems[i]);
            }
        }

        public override bool OnContextItemSelected(IMenuItem item)
        {
            var info = (AdapterView.AdapterContextMenuInfo) item.MenuInfo;
            var menuItems = Resources.GetStringArray(Resource.Array.ReminderContextMenu);
            var menuItemIndex = item.ItemId;
            var menuItemName = menuItems[menuItemIndex];
            
            switch (menuItemName)
            {
                case "Edit":
                    return true;
                case "Delete":
                    RemoveReminderItem(reminderAdapter[info.Position]);
                    return true;
            }

            return base.OnContextItemSelected(item);
        }

        /** Azure Mobile Service Methods **/
        async void OnRefreshRemindersFromTable(DateTime date)
        {
            await RefreshRemindersFromTableAsync(date);
        }
    
        async Task RefreshRemindersFromTableAsync(DateTime date)
        {
            try
            {
                var query = reminderTable;
                var list = await query.FindAsync();

                reminderAdapter.Clear();

                // Add Reminders
                foreach (var current in list)
                {
                    var currentDate = current.Get<DateTime>("Date");
                    if (currentDate == date)
                        reminderAdapter.Add(current);
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        async Task RefreshMealsFromTableAsync(DateTime date)
        {
            try
            {
                // Retrieves Today's Meals
                var query = mealTable;
                var list = await query.FindAsync();

                foreach (var current in list)
                {
                    var currentDate = current.Get<DateTime>("Date");
                    if (date == currentDate)
                        mealItem = current;
                }

                if (mealItem == null)
                {
                    var newMealItem = new ParseObject("Meal");

                    newMealItem["Breakfast"] = Constants.NO_MEAL_SET;
                    newMealItem["Lunch"] = Constants.NO_MEAL_SET;
                    newMealItem["Dinner"] = Constants.NO_MEAL_SET;
                    newMealItem["Date"] = DateTime.Today;

                    newMealItem["IsFormalDinner"] = false;

                    AddMealItem(newMealItem);

                    // Set Meal Item
                    mealItem = newMealItem;
                }
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Connection Error");
            }
        }

        public async void AddReminderItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Insert Reminder");
            }
        }

        public async void RemoveReminderItem(ParseObject item)
        {
            try
            {
                await item.DeleteAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Remove Reminder");
            }

            reminderAdapter.Remove(item);
        }

        // TODO: Incomplete & Needs To Tell If Data Is Different
        // TODO: Need To Incoporate Adapter Smoothly
        public async void UpdateReminderItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Update Reminder");
            }
        }

        public async void AddMealItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Add Meals");
            }
        }

        public async void UpdateMealItem(ParseObject item)
        {
            try
            {
                await item.SaveAsync();
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Update Meals");
            }
        }

        /** Add Reminder Dialog **/
        void CreateAndShowAddReminderDialog()
        {
            // Create Dialog And Transaction
            var transaction = FragmentManager.BeginTransaction();
            var reminderDialog = new AddReminderDialogFragment();

            // Probably Horrible Practice
            reminderDialog.date = selectedDate;
            reminderDialog.reminderTable = reminderTable;
            reminderDialog.reminderAdapter = reminderAdapter;

            reminderDialog.Show(transaction, "addReminderDialog");
        }

        /** Set Meal Dialog **/
        void CreateAndShowSetMealsDialog()
        {
            var transaction = FragmentManager.BeginTransaction();
            var setMealsDialog = new SetMealsDialogFragment();

            // Horrible Practice
            setMealsDialog.MealItem = mealItem;

            setMealsDialog.Show(transaction, "setMealsDialog");
        }

        /** Date Picking Methods **/
        protected override Dialog OnCreateDialog(int id)
        {
            return new DatePickerDialog(this, HandleDateSet, selectedDate.Year, selectedDate.Month - 1, selectedDate.Day);
        }

        // TODO: Handle Returning To Same Date
        void HandleDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            reminderAdapter.Clear();
            selectedDate = e.Date;
            datePickerButton.Text = selectedDate.ToShortDateString();
            OnRefreshRemindersFromTable(e.Date);
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