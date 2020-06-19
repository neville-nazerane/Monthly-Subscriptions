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

namespace MonthlySubscriptions.ViewModels
{
    public class ManageDayViewModel : ViewModelBase
    {
        private DateTime _date;
        private IEnumerable<Subscription> _subscriptions;
        private float _total;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public float Total { get => _total; set => SetProperty(ref _total, value); }

        public IEnumerable<Subscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand GoToAddCmd { get; set; }

        public ICommand EditCmd { get; set; }

        public ManageDayViewModel()
        {
            GoToAddCmd = new Command(async () => await GoToAddAsync());
            EditCmd = new Command<string>(async title => await EditSubscription(title));
        }

        public void Appearing()
        {
            Subscriptions = Repository.Get(Date)?.Subscriptions.GetValueOrDefault(Date.Day);
            Total = Subscriptions.Sum(s => s.Price);
        }

        private Task EditSubscription(string title)
        {
            return Shell.Current.GoToAsync($"//calendar/manageSubscription?date={Date.Ticks}&title={title}");

        }

        private Task GoToAddAsync()
        {
            return Shell.Current.GoToAsync("//calendar/manageSubscription?date=" + Date.Ticks);
        }

    }
}
