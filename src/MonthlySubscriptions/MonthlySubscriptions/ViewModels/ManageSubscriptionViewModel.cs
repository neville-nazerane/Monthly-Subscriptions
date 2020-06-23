using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class ManageSubscriptionViewModel : ViewModelBase
    {
        private DateTime _date;
        private bool _isEditing;
        private Subscription _data;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public bool IsEditing { get => _isEditing; set => SetProperty(ref _isEditing, value); }

        public Subscription Data { get => _data; set => SetProperty(ref _data, value); }

        public ICommand SaveCmd { get; set; }

        public ICommand DeleteCmd { get; set; }

        public ICommand GoToDayCmd { get; set; }

        public ManageSubscriptionViewModel()
        {
            Data = new Subscription();
            SaveCmd = new Command(async () => await SaveAsync());
            DeleteCmd = new Command(async () => await DeleteAsync());
            GoToDayCmd = new Command(async () => await GoToDayAsync());
        }

        private Task GoToDayAsync() => Shell.Current.GoToAsync("//calendar/manageDay?date=" + Date.Ticks);

        public void PopulateFromTitle(string title)
        {
            Data = Repository.Get(Date)?.Subscriptions.GetValueOrDefault(Date.Day)?.SingleOrDefault(s => s.Title == title);
            IsEditing = true;
        }

        public async Task DeleteAsync()
        {
            if (await Shell.Current.DisplayAlert("Seriously??", "Are you sure you want to delete this record", "Yes, Delete", "No, Cancel"))
            {
                await SaveAsync(toDelete: true);
            }
        }

        public async Task SaveAsync(bool toDelete = false)
        {
            var current = Repository.Get(Date);
            if (IsEditing)
            {
                try
                {
                    var subscriptions = current.Subscriptions[Date.Day].Where(s => s.Title != Data.Title).ToList();
                    if (!toDelete) subscriptions.Add(Data);
                    current.Subscriptions[Date.Day] = subscriptions;
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Error!", "Something went wrong. Unable to save", "OK");
                    await Shell.Current.GoToAsync("//Calendar");
                }
            }
            else
            {
                if (current.Subscriptions.TryGetValue(Date.Day, out var subscriptions))
                {
                    if (subscriptions.Any(s => s.Title == Data.Title))
                    {
                        await Shell.Current.DisplayAlert("Title exists", "You already have that subscription!", "OK");
                        return;
                    }

                    current.Subscriptions[Date.Day] = new List<Subscription>(subscriptions)
                    {
                        Data
                    };
                }
                else
                {
                    current.Subscriptions[Date.Day] = new Subscription[] { Data };
                }
            }
            Repository.Save(current);
            await GoToDayAsync();
        }

    }
}
