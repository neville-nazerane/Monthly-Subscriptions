using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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

        public CalendarViewModel()
        {
            SelectedDate = DateTime.Now;
            UpdateDataFromDate();
        }

        public void UpdateDataFromDate()
        {
            int numOfDays = DateTime.DaysInMonth(SelectedDate.Month, SelectedDate.Year);

            var days = Enumerable.Range(1, numOfDays).Select(i => new DayContext { Day = i }).ToArray();

            data = Repository.Get(SelectedDate);

            foreach (var item in data.Subscriptions)
                days[item.Key - 1].Subscriptions = new ObservableCollection<Subscription>(item.Value);

            Days = days;
        }

    }
}
