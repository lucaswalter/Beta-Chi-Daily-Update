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

namespace AndroidApp.Screens
{
    [Activity(Label = "Edit Scribe Data", Theme = "@style/Theme.BetaChi")]
    public class EditDataScribeActivity : Activity
    {
        // Mobile Service Client Reference
        private MobileServiceClient client;

        // Mobile Service Tables Used To Access Data
        private IMobileServiceTable<ReminderItem> reminderTable;
        private IMobileServiceTable<MealItem> mealTable; 

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

        private MealItem mealItem;

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
            
            // Set Breakfast
            setBreakfastButton = FindViewById<Button>(Resource.Id.buttonSetBreakfast);
            setBreakfastButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowEditMealDialog(Constants.BREAKFAST);
            };

            // Set Lunch
            setLunchButton = FindViewById<Button>(Resource.Id.buttonSetLunch);
            setLunchButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowEditMealDialog(Constants.LUNCH);
            };

            // Set Dinner 
            setDinnerButton = FindViewById<Button>(Resource.Id.buttonSetDinner);
            setDinnerButton.Click += (object sender, EventArgs e) =>
            {
                CreateAndShowEditMealDialog(Constants.DINNER);
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
                    if (reminderAdapter[i].Id == null)
                        AddReminderItem(reminderAdapter[i]);
                }

                // Save Meal Item
                UpdateMealItem(mealItem);

            };

            // Connect To Azure Mobile Service
            try
            {
                // Initialize
                CurrentPlatform.Init();

                // Create Mobile Service Client Instance
                client = new MobileServiceClient(Constants.APPLICATION_URL, Constants.APPLICATION_KEY);

                // Retrieve Tables
                reminderTable = client.GetTable<ReminderItem>();
                mealTable = client.GetTable<MealItem>();

                // Load The Reminders From The Mobile Service
                await RefreshRemindersFromTableAsync(DateTime.Today);

                // Load Meals From The Mobil Service
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
                var list = await reminderTable.Where(x => x.Date.Day == date.Day).ToListAsync();

                reminderAdapter.Clear();

                foreach (ReminderItem current in list)
                    reminderAdapter.Add(current);

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
                // Retrieve MealItem For The Day
                var list = await mealTable.Where(x => x.Date.Day == date.Day).ToListAsync();
                var meals = list.FirstOrDefault();

                if (meals != null)
                {
                    // Set Meal Item
                    mealItem = meals;
                }
                else
                {
                    var newMealItem = new MealItem();
                    
                    newMealItem.Breakfast = Constants.NO_MEAL_SET;
                    newMealItem.Lunch = Constants.NO_MEAL_SET;
                    newMealItem.Dinner = Constants.NO_MEAL_SET;
                    newMealItem.Date = DateTime.Today;

                    newMealItem.IsFormalDinner = false;

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

        public async void AddReminderItem(ReminderItem item)
        {
            try
            {
                await reminderTable.InsertAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Insert Reminder");
            }
        }

        public async void RemoveReminderItem(ReminderItem item)
        {
            try
            {
                await reminderTable.DeleteAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Remove Reminder");
            }

            reminderAdapter.Remove(item);
        }

        // TODO: Incomplete & Needs To Tell If Data Is Different
        // TODO: Need To Incoporate Adapter Smoothly
        public async void UpdateReminderItem(ReminderItem item)
        {
            try
            {
                await reminderTable.UpdateAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Update Reminder");
            }
        }

        public async void AddMealItem(MealItem item)
        {
            try
            {
                await mealTable.InsertAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Add Meals");
            }
        }

        public async void UpdateMealItem(MealItem item)
        {
            try
            {
                await mealTable.UpdateAsync(item);
            }
            catch (Exception e)
            {
                CreateAndShowDialog(e, "Unable To Update Meals");
            }
        }

        /** Set Meals Dialog **/
        void CreateAndShowEditMealDialog(int meal)
        {
            switch (meal)
            {
                case Constants.BREAKFAST:

                case Constants.LUNCH:

                case Constants.DINNER:
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