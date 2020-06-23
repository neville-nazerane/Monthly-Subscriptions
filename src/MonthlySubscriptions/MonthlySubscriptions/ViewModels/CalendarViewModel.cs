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
        private float _total;

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

        public float Total { get => _total; set => SetProperty(ref _total, value); }

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
        }

        public void Appearing()
        {
            UpdateDataFromDate();
        }

        public async Task SelectDay(int day)
        {
            var selected = new DateTime(SelectedDate.Year, SelectedDate.Month, day);
            await Shell.Current.GoToAsync("//calendar/manageDay?date=" + selected.Ticks);
        } 

        public void UpdateDataFromDate()
        {
            int numOfDays = SelectedDate.DaysInMonth();

            var days = Enumerable.Range(1, numOfDays).Select(i => new DayContext { Day = i }).ToArray();

            data = Repository.Get(SelectedDate);

            foreach (var item in data.Subscriptions)
                days[item.Key - 1].CurrentCost = item.Value?.Sum(s => s.Price) ?? 0;

            if (SelectedDate.Date > DateTime.Now.Date)
            {
                var prediction = Repository.Get(DateTime.Now);
                
                foreach (var item in prediction.Subscriptions)
                {
                    var subs = item.Value;
                    var cancelled = data.CanceledSubscriptions.GetValueOrDefault(item.Key);
                    
                    if (subs != null && cancelled != null)
                    {
                        subs = subs.Where(s => !cancelled.Any(c => c.Title == s.Title));
                    }

                    days[item.Key - 1].PredictedTotal = subs?.Sum(s => s.Price) ?? 0;
                }

                // handle max days
            }
            else
            {
                
            }

            Days = days;
            Total = days.Sum(d => d.Total);
        }

    }
}
