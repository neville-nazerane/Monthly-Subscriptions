using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MonthlySubscriptions.Services
{
    public static class BackupManager
    {

        private const string lastBackupKey = "lastBackupAt";

        private const string timeFormat = "hh_mm on dd_MM_yy";
        private const string backupPrefix = "Backup at ";
        private const string namedBackupPrefix = "Backup named ";

        public static async Task<bool> EnsureInitAsync()
        {
            var status = await Permissions.RequestAsync<Permissions.StorageWrite>();
            if (status != PermissionStatus.Granted) return false;

            var fileProvider = DependencyService.Get<IFileProviderService>();
            if (!Preferences.ContainsKey(lastBackupKey))
            {
                string prefix = Path.Combine(fileProvider.GetPath(), backupPrefix);
                var dates = from f in Directory.GetFiles(fileProvider.GetPath())
                            where f.StartsWith(prefix)
                            select DateTime.ParseExact(f.Substring(prefix.Length), timeFormat, null);

                if (dates.Any())
                    Preferences.Set(lastBackupKey, dates.Max());
            }
            return true;
        }

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

        public static DateTime? GetLastBackedUpDate()
        {
            var previous = Preferences.Get(lastBackupKey, default(DateTime));
            string previousFile = BuildName(previous);
            if (File.Exists(previousFile))
                return previous;
            return null;
        }

        public static bool RestoreFromLast()
        {
            var previous = Preferences.Get(lastBackupKey, default(DateTime));
            string previousFile = BuildName(previous);
            if (File.Exists(previousFile))
            {
                Repository.Restore(previousFile);
                return true;
            }
            return false;
        }

        public static IEnumerable<string> GetAllNamed()
        {
            var fileProvider = DependencyService.Get<IFileProviderService>();
            string root = fileProvider.GetPath();
            string prefix = Path.Combine(root, namedBackupPrefix);
            var files = Directory.GetFiles(root);
            return from f in files where f.StartsWith(prefix) select f.Substring(prefix.Length);
        }

        public static bool AddNamed(string name)
        {
            var fileProvider = DependencyService.Get<IFileProviderService>();
            string fileName = Path.Combine(fileProvider.GetPath(), namedBackupPrefix + name);
            if (File.Exists(fileName))
                return false;
            Repository.BackupTo(fileName);
            return true;
        }

        public static bool RestoreNamed(string name)
        {
            var fileProvider = DependencyService.Get<IFileProviderService>();
            string fileName = Path.Combine(fileProvider.GetPath(), namedBackupPrefix + name);
            if (File.Exists(fileName))
            {
                Repository.Restore(fileName);
                return true;
            }
            return false;
        }

        internal static void DeleteNamed(string name)
        {
            var fileProvider = DependencyService.Get<IFileProviderService>();
            string fileName = Path.Combine(fileProvider.GetPath(), namedBackupPrefix + name);
            if (File.Exists(fileName)) File.Delete(fileName);
        }

        private static string BuildName(DateTime time)
        {
            var fileProvider = DependencyService.Get<IFileProviderService>(); 
            return Path.Combine(fileProvider.GetPath(), backupPrefix + time.ToString(timeFormat));
        }

    }
}
