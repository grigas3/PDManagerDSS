using System;
using System.Collections.Generic;
using System.Text;

namespace PDManager.Core.Common.Extensions
{
    /// <summary>
    /// Datetime extensions for conversion to Unix timestamps
    /// </summary>
    public static class DateTimeExtensions
    {

        /// <summary>
        /// C# DateTime to Java
        /// </summary>
        /// <param name="dateTime">C# DateTime</param>
        /// <returns></returns>
        public static long ToUnixTimestamp(this DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalSeconds;
        }
        /// <summary>
        /// C# DateTime to Java
        /// </summary>
        /// <param name="dateTime">C# DateTime</param>
        /// <returns></returns>
        public static long ToUnixTimestampMilli(this DateTime dateTime)
        {
            return (long)(TimeZoneInfo.ConvertTimeToUtc(dateTime) -
                   new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc)).TotalMilliseconds;
        }


        /// <summary>
        /// Java Milliseconds (Unix Time) to C# DateTime
        /// </summary>
        /// <param name="javaTimeStamp">Unix Timestamp</param>
        /// <returns>C# Datetime</returns>
        public static DateTime FromUnixTimestampMilli(this double javaTimeStamp)
        {
            // Java timestamp is millisecods past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddMilliseconds(Math.Round(javaTimeStamp )).ToLocalTime();
            return dtDateTime;
        }


        /// <summary>
        /// Java Seconds (Unix Time) to C# DateTime
        /// </summary>
        /// <param name="javaTimeStamp">Unix Timestamp</param>
        /// <returns>C# Datetime</returns>
        public static DateTime FromUnixTimestamp(this double javaTimeStamp)
        {
            // Java timestamp is millisecods past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(Math.Round(javaTimeStamp)).ToLocalTime();
            return dtDateTime;
        }

    }
}
