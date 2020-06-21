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
        private const string currentTitle = "Saved";
        private const string predictedTitle = "Predicted";
        private DateTime _date;
        private IEnumerable<GroupedSubscription> _subscriptions;
        private float _total;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public float Total { get => _total; set => SetProperty(ref _total, value); }

        public IEnumerable<GroupedSubscription> Subscriptions { get => _subscriptions; set => SetProperty(ref _subscriptions, value); }

        public ICommand GoToAddCmd { get; set; }

        public ICommand EditCmd { get; set; }

        public ManageDayViewModel()
        {
            GoToAddCmd = new Command(async () => await GoToAddAsync());
            EditCmd = new Command<string>(async title => await EditSubscription(title));
        }

        public void Appearing()
        {
            var allSubs = new List<GroupedSubscription> 
            { 
                new GroupedSubscription(currentTitle) {
                    Repository.Get(Date)?.Subscriptions?.GetValueOrDefault(Date.Day)
                }
            };

            if (Date.Month > DateTime.Now.Month)
            {
                allSubs.Add(new GroupedSubscription(predictedTitle) 
                {
                    Repository.Get(DateTime.Now)?.Subscriptions?.GetValueOrDefault(Date.Day)
                });
            }

            Subscriptions = allSubs;

            Total = Subscriptions.SelectMany(s => s).Sum(s => s.Price);

            //Subscriptions = Repository.Get(Date)?.Subscriptions.GetValueOrDefault(Date.Day);
            //Total = Subscriptions.Sum(s => s.Price);
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
