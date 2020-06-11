using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class CalendarViewModel : ViewModelBase
    {
        private IEnumerable<DayContext> _days;
        private DateTime _selectedDate;
        private MonthData data;

        public DateTime SelectedDate { get => _selectedDate; set => SetProperty(ref _selectedDate, value); }

        public IEnumerable<DayContext> Days { get => _days; set => SetProperty(ref _days, value); }

        public Command NextCmd => new Command(() => SelectedDate = SelectedDate.AddMonths(1));
        public Command PrevCmd => new Command(() => SelectedDate = SelectedDate.AddMonths(-1));

        public Command CurrentCmd => new Command(() => SelectedDate = DateTime.Now);

        public CalendarViewModel()
        {
            SelectedDate = DateTime.Now;
            UpdateDataFromDate();
        }

        public void UpdateDataFromDate()
        {
            int numOfDays = DateTime.DaysInMonth(SelectedDate.Year, SelectedDate.Month);

            var days = Enumerable.Range(1, numOfDays).Select(i => new DayContext { Day = i }).ToArray();

            data = Repository.Get(SelectedDate);

            foreach (var item in data.Subscriptions)
                days[item.Key - 1].Subscriptions = new ObservableCollection<Subscription>(item.Value);

            Days = days;
        }

    }
}
