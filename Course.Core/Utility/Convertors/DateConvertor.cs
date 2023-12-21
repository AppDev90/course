using System;
using System.Globalization;

namespace Course.Core.Utility.Convertors
{
    public static class DateConvertor
    {
        public static string ToShamsi(this DateTime dateTime)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(dateTime) + "/" + pc.GetMonth(dateTime).ToString("00") + "/" +
                   pc.GetDayOfMonth(dateTime).ToString("00");
        }
    }
}
