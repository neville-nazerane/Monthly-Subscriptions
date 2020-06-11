using MonthlySubscriptions.Models;
using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private DateTime _displayDate;
        private MonthData _currentData;

        public DateTime DisplayDate
        {
            get => _displayDate; 
            set
            {
                SetProperty(ref _displayDate, value);
                CurrentData = Repository.Get(value);
            }
        }

        public MonthData CurrentData { get => _currentData; set => SetProperty(ref _currentData, value); }

        public Command NextCmd => new Command(() => DisplayDate = DisplayDate.AddMonths(1));
        public Command PrevCmd => new Command(() => DisplayDate = DisplayDate.AddMonths(-1));

        public Command ResetCmd => new Command(() => DisplayDate = DateTime.Now);

        public MainViewModel()
        {
            DisplayDate = DateTime.Now;
        }

        public void AddNew(string title)
        {
            //CurrentData.Subscriptions.Add(new Subscription { Title = title });
            Repository.Save(CurrentData);
        }










    }
}
