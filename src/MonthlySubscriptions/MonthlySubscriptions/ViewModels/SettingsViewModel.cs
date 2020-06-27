using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {

        public ICommand ClearDbCmd { get; set; }

        public ICommand BackUpCmd { get; set; }

        public SettingsViewModel()
        {
            ClearDbCmd = new Command(async () => await ClearAsync());
            BackUpCmd = new Command(async () => await BackUpAsync());
        }

        private async Task ClearAsync()
        {
            if (await Shell.Current.DisplayAlert("Seriously?", "Are you sure you want to delete all data", "Yep", "No"))
            {
                Repository.ClearDatabase();
            }
        }

        private async Task BackUpAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageRead>();
            if (status == PermissionStatus.Granted)
            {
                await Shell.Current.DisplayAlert("OK!!!", "I WILL NOT", "Cancel");
                var path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                var files = Directory.GetFiles(path);
            }
            else
            {
                await Shell.Current.DisplayAlert("Failed!", "Can't backup without write permissions", "Cancel");
            }
        }

    }
}
