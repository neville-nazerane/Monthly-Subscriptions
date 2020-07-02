using AP.MobileToolkit.Fonts;
using LiteDB;
using MonthlySubscriptions.Models;
using MonthlySubscriptions.Pages;
using MonthlySubscriptions.Services;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions
{
    public partial class App : Application
    {
        public App()
        {

            //ExperimentalFeatures.Enable("SwipeView_Experimental");

            InitializeComponent();

            DbSetup();
            FontRegistry.RegisterFonts(FontAwesomeSolid.Font);

            MainPage = new AppShell();
            Routing.RegisterRoute("calendar/manageDay", typeof(ManageDayPage));
            Routing.RegisterRoute("calendar/manageSubscription", typeof(ManageSubscriptionPage));
            Routing.RegisterRoute("settings/backups", typeof(ManageBackupsPage));

        }

        void DbSetup()
        {
            BsonMapper.Global.Entity<MonthData>().Id(m => m.YearMonth, false);
            Repository.VerifyCurrentMonthPopulated();
        }

    }
}
