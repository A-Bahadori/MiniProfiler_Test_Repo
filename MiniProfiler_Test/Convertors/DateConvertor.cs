using System.Globalization;
using MD.PersianDateTime;

namespace MiniProfiler_Test.Convertors;

public static class DateConvertor
{
    public static DateTime? ToGregorianDateTime(this string shamsiDate)
    {
        try
        {
            PersianDateTime persianDate = PersianDateTime.Parse(shamsiDate);
            DateTime gregorianDate = persianDate.ToDateTime();
            return gregorianDate;
        }
        catch
        {
            return null;
        }
    }

    public static string? ToShamsiDateAndTime(this DateTime? value)
    {
        if (!value.HasValue)
        {
            return null;
        }

        PersianCalendar pc = new PersianCalendar();
        return value.Value.Hour.ToString("00") + ":" +
               value.Value.Minute.ToString("00") + " " +
               pc.GetYear(value.Value) + "/" + pc.GetMonth(value.Value).ToString("00") + "/" +
               pc.GetDayOfMonth(value.Value).ToString("00");
    }

    public static string ToShamsiDateAndTime(this DateTime value)
    {
        PersianCalendar pc = new PersianCalendar();
        return value.Hour.ToString("00") + ":" +
               value.Minute.ToString("00") + " " +
               pc.GetYear(value) + "/" + pc.GetMonth(value).ToString("00") + "/" +
               pc.GetDayOfMonth(value).ToString("00");
    }
}