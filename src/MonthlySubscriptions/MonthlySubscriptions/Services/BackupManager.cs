using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MonthlySubscriptions.Services
{
    public static class BackupManager
    {

        private const string lastBackupKey = "lastBackupAt";

        private const string timeFormat = "hh_mm on dd_MM_yy";

        public static void BackupNow()
        {
            var time = DateTime.Now;
            Repository.BackupTo(BuildName(time));

            // removing old file
            var previous = Preferences.Get(lastBackupKey, default(DateTime));
            string previousFile = BuildName(previous);
            if (File.Exists(previousFile))
                File.Delete(previousFile);
            
            // updating last backup time
            Preferences.Set(lastBackupKey, time);
        }

        private static string BuildName(DateTime time)
        {
            var fileProvider = DependencyService.Get<IFileProviderService>();
            return Path.Combine(fileProvider.GetPath(), $"Backup at {time.ToString(timeFormat)}");
        }

    }
}
