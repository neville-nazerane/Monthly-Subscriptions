using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MonthlySubscriptions.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private IEnumerable<DayContext> _days;
        private DateTime _selectedDate;
        private MonthData data;
        private DateTime? _selection;

        public DateTime? Selection { get => _selection; set => SetProperty(ref _selection, value); }

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                SetProperty(ref _selectedDate, value);
                UpdateDataFromDate();
                Selection = null;
            }
        }

        public IEnumerable<DayContext> Days { get => _days; set => SetProperty(ref _days, value); }

        public Command SelectCmd { get; set; }

        public Command NextCmd { get; set; }
        public Command PrevCmd { get; set; }

        public Command CurrentCmd { get; set; }

        public CalendarViewModel()
        {
            NextCmd = new Command(() => SelectedDate = SelectedDate.AddMonths(1));
            PrevCmd = new Command(() => SelectedDate = SelectedDate.AddMonths(-1));
            CurrentCmd = new Command(() => SelectedDate = DateTime.Now);
            SelectCmd = new Command<int>(async day => await SelectDay(day));

            SelectedDate = DateTime.Now;
            UpdateDataFromDate();
        }

        public async Task SelectDay(int day)
        {
            var selected = new DateTime(SelectedDate.Year, SelectedDate.Month, day);
            await Shell.Current.GoToAsync("//calendar/manageDay?date=" + selected.Ticks);

            //Selection = new DateTime(SelectedDate.Year, SelectedDate.Month, day);
            //data.Subscriptions[day] = new Subscription[] { 
            //    new Subscription { 
            //        Price = 39              
            //    },
            //    new Subscription {
            //        Price = 90
            //    }
            //};
            //Repository.Save(data);
            //UpdateDataFromDate();
        }

        public void UpdateDataFromDate()
        {
            int numOfDays = SelectedDate.DaysInMonth();

            var days = Enumerable.Range(1, numOfDays).Select(i => new DayContext { Day = i }).ToArray();

            data = Repository.Get(SelectedDate);

            foreach (var item in data.Subscriptions)
                days[item.Key - 1].Subscriptions = new ObservableCollection<Subscription>(item.Value);

            Days = days;
        }

    }
}
