using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace AndroidApp.Core
{
    public class MealItem
    {
        public string Id { get; set; }

        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public bool IsFormalDinner { get; set; }
    }
}