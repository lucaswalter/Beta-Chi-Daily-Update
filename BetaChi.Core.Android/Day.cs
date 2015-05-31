using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// TODO: Possibly Remove Azure Mobile Services From Here And IOS Core
namespace BetaChi.Core.Android
{
    public class Day
    {
        public string Id { get; set; }
        public DateTime CurrentDate { get; set; }

        public MealItem Meals { get; set; }

        public ICollection<ReminderItem> Reminders { get; set; }
        public ICollection<DriverItem> SoberDrivers { get; set; }
    }
}
