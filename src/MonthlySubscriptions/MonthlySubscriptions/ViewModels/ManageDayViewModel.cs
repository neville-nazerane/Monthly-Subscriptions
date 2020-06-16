using System;
using System.Collections.Generic;
using System.Text;

namespace MonthlySubscriptions.ViewModels
{
    public class ManageDayViewModel : ViewModelBase
    {
        private DateTime _date;

        public DateTime Date { get => _date; set => SetProperty(ref _date, value); }



    }
}
