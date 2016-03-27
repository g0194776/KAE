using System;
using System.Text;

namespace KJFramework.Datas
{
    public static class SqlUtils
    {
        public static string FormatSql(object obj)
        {
            if (obj == null)
            {
                return "null";
            } if (obj.GetType() == typeof(string))
            {
                return "'" + ((string)obj).Replace("'", "''") + "'";
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                return "'" + ((DateTime)obj).ToString("yyyy-MM-dd HH:mm:ss.fff") + "'";
            }
            else
            {
                return obj.ToString();
            }
        }

        public static string MaskInvalidCharacters(string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            StringBuilder ret = null;
            for (int i = 0; i < s.Length; i++)
            {
                char ch = s[i];

                bool invalidChar = !((ch >= 0x20 && ch <= 0xD7FF) || (ch >= 0xe000 && ch <= 0xfffd) || ch == '\t' || ch == '\r' || ch == '\n');

                if (ret == null && invalidChar)
                {
                    ret = new StringBuilder();
                    ret.Append(s.Substring(0, i));
                }

                if (!invalidChar && ret != null)
                {
                    ret.Append(ch);
                }
            }

            if (ret == null)
                return s;
            else
                return ret.ToString();
        }

        public static readonly DateTime SmallDateTime_MinValue = DateTime.Parse("1900-01-01");
        public static readonly DateTime SmallDateTime_MaxValue = DateTime.Parse("2079-06-06");

        public static readonly DateTime MysqlTimeStamp_Min = DateTime.Parse("1970-01-01");
        public static readonly DateTime MysqlTimeStamp_Max = DateTime.Parse("2038-01-01");

        public static readonly DateTime DateTime_MinValue = DateTime.Parse("1753-01-01");
        public static readonly DateTime DateTime_MaxValue = DateTime.Parse("9999-12-31");

        public static readonly DateTime DayIDBase = DateTime.Parse("1996-01-01");

        public static DateTime GetDateTime(int dayID)
        {
            return DayIDBase.AddDays(dayID - 1);
        }

        public static int GetDayID(DateTime date)
        {
            return (date - DayIDBase).Days + 1;
        }
    }
}
