using MonthlySubscriptions.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace MonthlySubscriptions.ValueConverters
{
    class DayToColor : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var day = (DayContext)value;
            if (day.PredictedTotal > 0)
                return Color.LightBlue;
            else if (day.CurrentCost > 0)
                return Color.GreenYellow;
            else return Color.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
