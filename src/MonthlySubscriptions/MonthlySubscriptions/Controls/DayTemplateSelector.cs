using MonthlySubscriptions.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MonthlySubscriptions.Controls
{
    class DayTemplateSelector : DataTemplateSelector
    {

        public DataTemplate OnlyDayTemplate { get; set; }

        public DataTemplate DayWithCostTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var data = (DayContext)item;
            if (data.TotalCost == 0)
                return OnlyDayTemplate;
            else return DayWithCostTemplate;
        }
    }
}
