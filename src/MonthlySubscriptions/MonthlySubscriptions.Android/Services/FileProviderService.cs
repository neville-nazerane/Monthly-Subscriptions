using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Java.IO;
using MonthlySubscriptions.Droid.Services;
using MonthlySubscriptions.Services;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileProviderService))]
namespace MonthlySubscriptions.Droid.Services
{
    class FileProviderService : IFileProviderService
    {

        private Activity activity;

        internal static void Init(Activity activity)
        {
            var instance = (FileProviderService)DependencyService.Get<IFileProviderService>();
            instance.activity = activity;
        }

        public string[] GetDirectories()
        {
            //if (activity is null)
            //{
            //    throw new ArgumentNullException(nameof(activity), "Make sure you have called FileProvider.Init in android's main activity");
            //}
            var files = Android.OS.Environment.ExternalStorageDirectory;
            return (from f in files.ListFiles() where f.IsDirectory select f.Name).ToArray();
        }

    }
}