using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BetaChi.Core.iOS
{
    class MealItem
    {
        public string Id { get; set; }

        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public bool IsFormalDinner { get; set; }
    }
}