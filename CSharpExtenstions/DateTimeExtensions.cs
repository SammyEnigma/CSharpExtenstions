using System;
using System.Xml;
using System.Globalization;
using TimeZone = System.TimeZoneInfo;

namespace CSharpExtenstions
{
    public static class DateTimeExtensions
    {
        private static readonly DateTime MinDate = new DateTime(1900, 1, 1);
        private static readonly DateTime MaxDate = new DateTime(9999, 12, 31, 23, 59, 59, 999);
        public static long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;

        public static bool IsValid(this DateTime value)
        {
            return (value >= MinDate) && (value <= MaxDate);
        }

        public static string GetLocalOffset(this DateTime value)
        {
            TimeSpan utcOffset = TimeZoneInfo.Local.GetUtcOffset(value);
            return utcOffset.Hours.ToString("+00;-00", CultureInfo.InvariantCulture) + ":" + utcOffset.Minutes.ToString("00;00", CultureInfo.InvariantCulture);
        }


        public static DateTime? ToUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToUniversalTime() : (DateTime?)null;
        }


        public static DateTime AssumeUniversalTime(this DateTime dateTime)
        {
            return new DateTime(dateTime.Ticks, DateTimeKind.Utc);
        }


        public static DateTime? AssumeUniversalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? AssumeUniversalTime(dateTime.Value) : (DateTime?)null;
        }


        public static DateTime? ToLocalTime(this DateTime? dateTime)
        {
            return dateTime.HasValue ? dateTime.Value.ToLocalTime() : (DateTime?)null;
        }



        public static DateTime GetEvenHourDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }
            DateTime d = dateTime.Value.AddHours(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
        }


        public static DateTime GetEvenMinuteDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            d = d.AddMinutes(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }


        public static DateTime GetEvenMinuteDateBefore(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
        }


        public static DateTime GetEvenSecondDate(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }
            DateTime d = dateTime.Value;
            d = d.AddSeconds(1);
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }


        public static DateTime GetEvenSecondDateBefore(this DateTime? dateTime)
        {
            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }
            DateTime d = dateTime.Value;
            return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, d.Second);
        }



        public static DateTime GetNextGivenMinuteDate(this DateTime? dateTime, int minuteBase)
        {
            if (minuteBase < 0 || minuteBase > 59)
            {
                throw new ArgumentException("minuteBase must be >=0 and <= 59");
            }

            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }
            DateTime d = dateTime.Value;

            if (minuteBase == 0)
            {
                d = d.AddHours(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
            }

            int minute = d.Minute;
            int arItr = minute / minuteBase;
            int nextMinuteOccurance = minuteBase * (arItr + 1);

            if (nextMinuteOccurance < 60)
            {
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, nextMinuteOccurance, 0);
            }
            else
            {
                d = d.AddHours(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, 0, 0);
            }
        }


        public static DateTime GetNextGivenSecondDate(this DateTime? dateTime, int secondBase)
        {
            if (secondBase < 0 || secondBase > 59)
            {
                throw new ArgumentException("secondBase must be >=0 and <= 59");
            }

            if (!dateTime.HasValue)
            {
                dateTime = DateTime.UtcNow;
            }

            DateTime d = dateTime.Value;

            if (secondBase == 0)
            {
                d = d.AddMinutes(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
            }

            int second = d.Second;
            int arItr = second / secondBase;
            int nextSecondOccurance = secondBase * (arItr + 1);

            if (nextSecondOccurance < 60)
            {
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, nextSecondOccurance);
            }
            else
            {
                d = d.AddMinutes(1);
                return new DateTime(d.Year, d.Month, d.Day, d.Hour, d.Minute, 0);
            }
        }



        public static DateTime TranslateTime(this DateTime date, TimeZone src, TimeZone dest)
        {
            DateTime newDate = DateTime.UtcNow;
            double offset = (GetOffset(date, dest) - GetOffset(date, src));

            newDate = newDate.AddMilliseconds(-1 * offset);

            return newDate;
        }


        public static double GetOffset(this DateTime date, TimeZone tz)
        {
            if (tz.IsDaylightSavingTime(date))
            {
                // TODO
                return tz.BaseUtcOffset.TotalMilliseconds + 0;
            }

            return tz.BaseUtcOffset.TotalMilliseconds;
        }


        public static bool UseDaylightTime(this TimeZone timezone)
        {
            return timezone.SupportsDaylightSavingTime;
        }

        public static long ToJavaScriptTicks(this DateTime dateTime)
        {
            DateTimeOffset utcDateTime = dateTime.ToUniversalTime();
            long javaScriptTicks = (utcDateTime.Ticks - InitialJavaScriptDateTicks) / (long)10000;
            return javaScriptTicks;
        }


        public static XmlDateTimeSerializationMode ToSerializationMode(DateTimeKind kind)
        {
            switch (kind)
            {
                case DateTimeKind.Local:
                    return XmlDateTimeSerializationMode.Local;
                case DateTimeKind.Unspecified:
                    return XmlDateTimeSerializationMode.Unspecified;
                case DateTimeKind.Utc:
                    return XmlDateTimeSerializationMode.Utc;
                default:
                    throw new ArgumentOutOfRangeException("kind", kind, "Unexpected DateTimeKind value.");
            }
        }


        public static DateTime GetFirstDayOfMonth(this DateTime date)
        {
            DateTime dtFrom = date;
            dtFrom = dtFrom.AddDays(-(dtFrom.Day - 1));
            return dtFrom;
        }


        public static DateTime GetLastDayOfMonth(this DateTime date)
        {
            DateTime dtTo = date;
            dtTo = dtTo.AddMonths(1);
            dtTo = dtTo.AddDays(-(dtTo.Day));
            return dtTo;
        }

        public static DateTime ToEndOfTheDay(this DateTime dt)
        {
            if (dt != null)
                return new DateTime(dt.Year, dt.Month, dt.Day, 23, 59, 59);
            return dt;
        }

        public static DateTime? ToEndOfTheDay(this DateTime? dt)
        {
            return (dt.HasValue ? dt.Value.ToEndOfTheDay() : dt);
        }
        public static DateTime ToStartOfTheDay(this DateTime dt)
        {
            if (dt != null)

                return new DateTime(
                   dt.Year,
                   dt.Month,
                   dt.Day,
                   0, 0, 0, 0);
            return dt;
        }
        public static DateTime? ToStartOfTheDay(this DateTime? dt)
        {
            return (dt.HasValue ? dt.Value.ToStartOfTheDay() : dt);
        }
    }
}