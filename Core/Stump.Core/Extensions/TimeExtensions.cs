using System;
using System.Text;

namespace Stump.Core.Extensions
{
    public static class TimeExtensions
    {
        public static long GetUnixTimeStampLong(this DateTime date)
        {
            return (long)( date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime() ).TotalMilliseconds;
        }

        public static int GetUnixTimeStamp(this DateTime date)
        {
            return (int)( date - new DateTime(1970, 1, 1, 0, 0, 0, 0).ToLocalTime() ).TotalSeconds;
        }

        public static string ToPrettyFormat(this TimeSpan span)
        {
            if (span == TimeSpan.Zero) return "0 minutes";

            var sb = new StringBuilder();
            if (span.Days > 0)
                sb.AppendFormat("{0} day{1} ", span.Days, span.Days > 1 ? "s" : String.Empty);
            if (span.Hours > 0)
                sb.AppendFormat("{0} hour{1} ", span.Hours, span.Hours > 1 ? "s" : String.Empty);
            if (span.Minutes > 0)
                sb.AppendFormat("{0} minute{1} ", span.Minutes, span.Minutes > 1 ? "s" : String.Empty);
            return sb.ToString();
        }
    }
}