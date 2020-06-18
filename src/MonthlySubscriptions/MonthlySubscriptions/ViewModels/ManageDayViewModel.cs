using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
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

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public IEnumerable<Subscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand GoToAddCmd { get; set; }

        public ManageDayViewModel()
        {
            GoToAddCmd = new Command(async () => await GoToAddAsync());
        }

        public void Appearing()
        {
            Subscriptions = Repository.Get(Date)?.Subscriptions.GetValueOrDefault(Date.Day);
        }

        private async Task GoToAddAsync()
        {
            await Shell.Current.GoToAsync("//calendar/manageSubscription?date=" + Date.Ticks);
        }

    }
}
