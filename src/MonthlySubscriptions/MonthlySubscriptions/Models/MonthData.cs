using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace MonthlySubscriptions.Models
{
    public class MonthData
    {

        public DateTime YearMonth { get; set; }

        public Dictionary<int, IEnumerable<Subscription>> Subscriptions { get; set; }

        public Dictionary<int, IEnumerable<Subscription>> CanceledSubscriptions { get; set; }

        public MonthData()
        {
            Subscriptions = new Dictionary<int, IEnumerable<Subscription>>();
            CanceledSubscriptions = new Dictionary<int, IEnumerable<Subscription>>();
        }

    }
}
