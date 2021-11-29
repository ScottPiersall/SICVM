using System;

namespace SIC_Simulator.Extensions
{
    internal static class IntegerExtensions
    {
        /// <summary>
        /// Refreshes Register Displays on background thread. Calls are marshalled to UI thread
        /// </summary>
        public static string To24BITBinary(this int a)
        {
            string res = string.Empty;
            if (a >= 0)
            {
                res = Convert.ToString(a, 2);
            }
            else
            {
                // Number is Negative... We have two push two's complement in 24 bits more elegantly
            }
            return res.PadLeft(24, '0'); ;
        }
    }
}
