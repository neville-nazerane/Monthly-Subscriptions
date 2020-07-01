using MonthlySubscriptions.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MonthlySubscriptions.ViewModels
{
    public class ManageBackupsViewModel : ViewModelBase
    {
        private ObservableCollection<string> _namedBackups;
        private DateTime? _lastBackedup;

        public ObservableCollection<string> NamedBackups { get => _namedBackups; set => SetProperty(ref _namedBackups, value); }

        public DateTime? LastBackedup { get => _lastBackedup; set => SetProperty(ref _lastBackedup, value); }

        public ICommand BackupNowCmd { get; set; }
        public ICommand AddCmd { get; set; }
        public ICommand DeleteCmd { get; set; }
        public ICommand RestoreLastCmd { get; set; }
        public ICommand RestoreCmd { get; set; }

        public ManageBackupsViewModel()
        {
            BackupNowCmd = new Command(BackupNow);
            AddCmd = new Command(async () => await AddAsync());
            DeleteCmd = new Command<string>(async name => await DeleteAsync(name));
            RestoreLastCmd = new Command(async () => await RestoreLastAsync());
            RestoreCmd = new Command<string>(async name => await RestoreAsync(name));
        }

        public void OnAppearing()
        {
            LastBackedup = BackupManager.GetLastBackedUpDate();
            NamedBackups = new ObservableCollection<string>(BackupManager.GetAllNamed());
        }

        public void BackupNow()
        {
            BackupManager.BackupNow();
            LastBackedup = BackupManager.GetLastBackedUpDate();
        }

        public async Task AddAsync()
        {
            string name = await Shell.Current.DisplayPromptAsync("Name", "Enter the name of backup");
            if (name is null) return;
            try
            {
                var success = BackupManager.AddNamed(name);
                if (!success)
                {
                    await Shell.Current.DisplayAlert("Failed!", "A backup with this name already exists", "Ok");
                    return;
                }
                NamedBackups.Add(name);
            }
            catch
            {
                await Shell.Current.DisplayAlert("Failed", "Unable to save backup with this name", "Ok");
            }
        }

        public async Task DeleteAsync(string name)
        {
            if (await Shell.Current.DisplayAlert("Delete", $"Are you sure you want to delete the backup '{name}'?", "Delete", "No, Keep it"))
            {
                try
                {
                    BackupManager.DeleteNamed(name);
                }
                catch
                {
                    await Shell.Current.DisplayAlert("Failed", $"Unable to delete backup '{name}'", "Ok");
                }

                NamedBackups.Remove(name);
            }
        }

        public async Task RestoreLastAsync()
        {
            bool confirm = await Shell.Current.DisplayAlert("Restore?", 
                            "Are you sure you want to restore to the last backup point? You would lose any changes after this point",
                            "Yes, Restore", "Cancel");
            if (confirm)
            {
                if (!BackupManager.RestoreFromLast())
                {
                    await Shell.Current.DisplayAlert("Failed", "Failed to restore backup", "Ok");
                }
            }
        }

        public async Task RestoreAsync(string name)
        {
            bool confirm = await Shell.Current.DisplayAlert("Restore?",
                            $"Are you sure you want to restore to '{name}'? You would lose any changes after this point",
                            "Yes, Restore", "Cancel");
            if (confirm)
            {
                if (!BackupManager.RestoreFromLast())
                {
                    await Shell.Current.DisplayAlert("Failed", "Failed to restore backup", "Ok");
                }
            }
        }

    }
}
