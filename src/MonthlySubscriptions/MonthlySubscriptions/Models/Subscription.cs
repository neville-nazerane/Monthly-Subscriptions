using System;
using System.Collections.Generic;
using System.Text;

namespace MonthlySubscriptions.Models
{
    public class Subscription
    {
        public int Day { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public float Price { get; set; }
    }
}
