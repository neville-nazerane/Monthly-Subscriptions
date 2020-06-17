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

    [QueryProperty("Date", "date")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageSubscriptionPage : ContentPage
    {

        public string Date { 
            set
            {
                ((ManageSubscriptionViewModel)BindingContext).Date = new DateTime(long.Parse(value));
            }
        }

        public ManageSubscriptionPage()
        {
            InitializeComponent();
        }
    }
}