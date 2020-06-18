using MonthlySubscriptions.Utils;
using MonthlySubscriptions.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions.Pages
{

    [QueryProperty("Date", "date")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageDayPage : ContentPage, IViewModelUtil<ManageDayViewModel>
    {

        public string Date
        {
            set
            {
                this.GetViewModel().Date = new DateTime(long.Parse(value));
            }
        }


        public ManageDayPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            this.GetViewModel().Appearing();
            base.OnAppearing();
        }
    }
}