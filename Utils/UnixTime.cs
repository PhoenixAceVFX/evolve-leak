using System;

namespace Evolve.Utils
{
    internal class UnixTime
    {
        public static DateTime ToDateTime(double unixTime)
        {
            return new DateTime(1970, 1, 1).AddSeconds(unixTime);
        }

        public static DateTime ToDateTime(string unixTimeString)
        {
            return UnixTime.ToDateTime(double.Parse(unixTimeString));
        }

        public static double FromDateTime(DateTime dateTime)
        {
            return dateTime.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}