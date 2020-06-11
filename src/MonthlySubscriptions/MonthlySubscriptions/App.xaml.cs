using AP.MobileToolkit.Fonts;
using LiteDB;
using MonthlySubscriptions.Models;
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

        }

        void DbSetup()
        {
            
            BsonMapper.Global.Entity<MonthData>()
                                .Id(m => m.YearMonth, false);
                                
            
        }

    }
}
