using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AndroidApp.Core
{
    public class IM
    {
        public string Id { get; set; }
        public DateTime CurrentDate { get; set; }

        public ICollection<ReminderItem> Reminders { get; set; }

        public ICollection<TeamItem> Leaderboard { get; set; }

        // TODO: Possibly Add Brotherhood Points
    }
}