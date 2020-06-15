using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    static class DateTimeExtensions
    {

        public static DateTime StripMonthYear(this DateTime date)
        {
            string format = "MM-YYYY";
            string strDate = date.ToString(format);
            return DateTime.ParseExact(strDate, format, null);
        }

        public static int DaysInMonth(this DateTime date)
            => DateTime.DaysInMonth(date.Year, date.Month);

    }
}
