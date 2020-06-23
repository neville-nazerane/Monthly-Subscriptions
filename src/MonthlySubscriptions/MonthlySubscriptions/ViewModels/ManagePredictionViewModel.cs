using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MonthlySubscriptions.ViewModels
{
    public class ManagePredictionViewModel : ViewModelBase
    {
        private Subscription _subscription;

        public Subscription Subscription { get => _subscription; set => SetProperty(ref _subscription, value); }

        public DateTime Date { get; set; }

        public void PopulateFromTitle(string title)
        {
            Subscription = Repository.Get(DateTime.Now).Subscriptions[Date.Day].Single(s => s.Title == title);
        }

    }
}
