using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MonthlySubscriptions.Models
{
    public class DayContext
    {

        public int Day { get; set; }

        public float CurrentCost { get; set; }
        public float PredictedTotal { get; set; }

        public float Total => CurrentCost + PredictedTotal;

    }
}
