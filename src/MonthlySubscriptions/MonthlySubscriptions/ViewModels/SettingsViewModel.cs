using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        public ICommand ClearDbCmd { get; set; }

        public SettingsViewModel()
        {
            ClearDbCmd = new Command(async () => await ClearAsync());
        }

        private async Task ClearAsync()
        {
            if (await Shell.Current.DisplayAlert("Seriously?", "Are you sure you want to delete all data", "Yep", "No"))
            {
                Repository.ClearDatabase();
            }
        }

    }
}
