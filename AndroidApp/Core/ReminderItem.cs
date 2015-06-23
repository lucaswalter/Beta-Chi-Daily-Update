using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AndroidApp.Core
{
    public class ReminderItem
    {
        public string Id { get; set; }

        public DateTime Date { get; set; }

        public string Text { get; set; }

        public bool ShowOnHomeScreen { get; set; }
    }
}