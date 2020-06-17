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

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }

        public ICommand GoToAddCmd { get; set; }

        public ManageDayViewModel()
        {
            GoToAddCmd = new Command(async () => await GoToAddAsync());
        }

        private async Task GoToAddAsync()
        {
            await Shell.Current.GoToAsync("//calendar/manageSubscription?date=" + Date.Ticks);
        }

    }
}
