using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using MonthlySubscriptions.Utils;

namespace MonthlySubscriptions.ViewModels
{
    public class ManageDayViewModel : ViewModelBase
    {
        private const string currentTitle = "Saved";
        private const string predictedTitle = "Predicted";
        private const string canceledTitle = "Canceled";
        private DateTime _date;
        private IEnumerable<GroupedSubscription> _subscriptions;
        private float _total;

        private MonthData data;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public float Total { get => _total; set => SetProperty(ref _total, value); }

        public IEnumerable<GroupedSubscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand GoToAddCmd { get; set; }

        public ICommand CancelCmd { get; set; }

        public ICommand UnCancelCmd { get; set; }

        public ICommand EditCmd { get; set; }

        public ManageDayViewModel()
        {
            GoToAddCmd = new Command(async () => await GoToAddAsync());
            EditCmd = new Command<string>(async title => await EditSubscriptionAsync(title));
            CancelCmd = new Command<Subscription>(async sub => await CancelSubscriptionAsync(sub));
            UnCancelCmd = new Command<Subscription>(async sub => await UnCancelSubscriptionAsync(sub));
        }

        public void Appearing()
        {
            data = Repository.Get(Date);

            if (data is null) return;

            var allSubs = new List<GroupedSubscription>();

            if (Date.Month > DateTime.Now.Month)
            {

                var predictions = Repository.Get(DateTime.Now)?.Subscriptions?.GetValueOrDefault(Date.Day);

                if (predictions?.Any() == true)
                {
                    var cancelled = data.CanceledSubscriptions.GetValueOrDefault(Date.Day);

                    if (cancelled?.Any() == true)
                    {
                        predictions = predictions.Where(p => !cancelled.Any(c => c.Title == p.Title));
                    }

                    allSubs.Add(new GroupedSubscription(predictedTitle, predictions));

                    if (cancelled?.Any() == true)
                    {
                        allSubs.Add(new GroupedSubscription(canceledTitle, cancelled));
                    }

                    Total = predictions.Sum(s => s.Price);
                }
            }
            else
            {
                allSubs.Add(new GroupedSubscription(currentTitle) {
                    data.Subscriptions.GetValueOrDefault(Date.Day)
                });
                Total = allSubs.FirstOrDefault()?.Sum(s => s.Price) ?? 0;
            }

            Subscriptions = allSubs;
        }

        private async Task CancelSubscriptionAsync(Subscription subscription)
        {
            if (await Shell.Current.DisplayAlert("Cancel Subscription?", $"Are you seriously planning to cancel {subscription}?", "Yep", "No, keep it"))
            {
                var subs = data.CanceledSubscriptions.GetValueOrDefault(Date.Day)?.ToList();
                if (subs is null) subs = new List<Subscription> { subscription };
                else subs.Add(subscription);
                data.CanceledSubscriptions[Date.Day] = subs;
                Repository.Save(data);
                Appearing();
            }
        }

        private async Task UnCancelSubscriptionAsync(Subscription subscription)
        {
            if (await Shell.Current.DisplayAlert("Cancel Subscription?", $"Are you seriously planning to get '{subscription}' back?", "Yep, GET IT", "No WAIT!"))
            {
                data.CanceledSubscriptions[Date.Day] = data.CanceledSubscriptions.GetValueOrDefault(Date.Day).ToList().TakeOff(subscription);
                Repository.Save(data);
                Appearing();
            }
        }

        private Task EditSubscriptionAsync(string title)
        {
            Subscription sub;
            if (GetSubscription(currentTitle, title) != null)
            {
                return Shell.Current.GoToAsync($"//calendar/manageSubscription?date={Date.Ticks}&title={title}");
            }
            else if ((sub = GetSubscription(predictedTitle, title)) != null)
            {
                return CancelSubscriptionAsync(sub);
            }
            else if ((sub = GetSubscription(canceledTitle, title)) != null)
            {
                return UnCancelSubscriptionAsync(sub);
            }
            return null;
        }

        private Subscription GetSubscription(string groupTitle, string title)
            => Subscriptions.SingleOrDefault(s => s.Title == groupTitle)?.SingleOrDefault(s => s.Title == title);

        private Task GoToAddAsync()
        {
            return Shell.Current.GoToAsync("//calendar/manageSubscription?date=" + Date.Ticks);
        }

    }
}
