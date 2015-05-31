using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace BetaChi.Core.Android
{
    class DriverItem
    {
        public string Name { get; set; }
        public string Number { get; set; }

        public DateTime ShiftStart { get; set; }
        public DateTime ShiftEnd { get; set; }
    }
}