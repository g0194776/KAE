using System;

namespace KJFramework.Common
{
    public static class Utility
    {
        private static readonly DateTime DateTime_MinValue = new DateTime(1970, 1, 1, 0, 0, 1, DateTimeKind.Utc);
        private static readonly DateTime DateTime_MaxValue = new DateTime(2038, 1, 19, 3, 14, 7, DateTimeKind.Utc);
        internal static DateTime DbNarrow(this DateTime date)
        {
            if (date < DateTime_MinValue)
                return DateTime_MinValue;
            if (date > DateTime_MaxValue)
                return DateTime_MaxValue;
            return date;
        }
    }
}
