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
    public partial class ManageDayPage : ContentPage
    {

        public string Date
        {
            set
            {
                ViewModel.Date = new DateTime(long.Parse(value));
            }
        }

        ManageDayViewModel ViewModel => BindingContext as ManageDayViewModel;

        public ManageDayPage()
        {
            InitializeComponent();
        }
    }
}