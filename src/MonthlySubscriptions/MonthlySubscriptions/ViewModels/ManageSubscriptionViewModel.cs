using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class ManageSubscriptionViewModel : ViewModelBase
    {
        private DateTime _date;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public Subscription Data { get; set; }

        public ICommand SaveCmd { get; set; }
        public ICommand GoToDayCmd { get; set; }

        public ManageSubscriptionViewModel()
        {
            Data = new Subscription();
            SaveCmd = new Command(async () => await SaveAsync());
            GoToDayCmd = new Command(async () => await GoToDayAsync());
        }

        private Task GoToDayAsync() => Shell.Current.GoToAsync("//calendar/manageDay?date" + Date.Ticks);

        public Task SaveAsync()
        {
            var current = Repository.Get(Date);
            if (current.Subscriptions.TryGetValue(Date.Day, out var subscriptions))
            {
                current.Subscriptions[Date.Day] = new List<Subscription>(subscriptions)
                {
                    Data
                };
            }
            else
            {
                current.Subscriptions[Date.Day] = new Subscription[] { Data };
            }
            Repository.Save(current);
            return GoToDayAsync();
        }

    }
}
