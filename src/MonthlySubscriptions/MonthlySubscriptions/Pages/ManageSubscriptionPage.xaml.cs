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

    [QueryProperty("Date", "date")]
    [QueryProperty("SubTitle", "title")]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageSubscriptionPage : ContentPage, IViewModelUtil<ManageSubscriptionViewModel>
    {

        public string Date { 
            set
            {
                this.GetViewModel().Date = new DateTime(long.Parse(value));
            }
        }

        public string SubTitle { 
            set
            {
                this.GetViewModel().PopulateFromTitle(Uri.UnescapeDataString(value));
            }
        }

        public ManageSubscriptionPage()
        {
            InitializeComponent();
        }
    }
}