using AP.MobileToolkit.Fonts;
using LiteDB;
using MonthlySubscriptions.Models;
using MonthlySubscriptions.Pages;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MonthlySubscriptions
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DbSetup();
            FontRegistry.RegisterFonts(FontAwesomeSolid.Font);

            MainPage = new AppShell();
            Routing.RegisterRoute("calendar/manageDay", typeof(ManageDayPage));
            Routing.RegisterRoute("calendar/manageSubscription", typeof(ManageSubscriptionPage));

        }

        void DbSetup()
        {
            BsonMapper.Global.Entity<MonthData>().Id(m => m.YearMonth, false);
        }

    }
}
