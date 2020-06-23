using System;
using System.Collections.Generic;
using System.Text;

namespace MonthlySubscriptions.Models
{
    public class GroupedSubscription : List<Subscription>
    {
        public string Title { get; set; }

        public GroupedSubscription()
        {

        }

        public GroupedSubscription(string title)
        {
            Title = title;
        }

        public GroupedSubscription(string title, IEnumerable<Subscription> subscriptions)
        {
            Title = title;
            Add(subscriptions);
        }

        public void Add(IEnumerable<Subscription> subscriptions)
        {
            if (subscriptions != null)
                AddRange(subscriptions);
        }

    }
}
