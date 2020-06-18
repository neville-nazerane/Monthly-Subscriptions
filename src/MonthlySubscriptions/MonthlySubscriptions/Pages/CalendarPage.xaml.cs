using MonthlySubscriptions.Utils;
using MonthlySubscriptions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalendarPage : ContentPage, IViewModelUtil<CalendarViewModel>
    {

        public CalendarPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.GetViewModel().Appearing();
        }

    }
}