using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator.Extensions
{
    static class integerExtensions
    {
        /// <summary>
        /// Refreshes Register Displays on background thread. Calls are marshalled to UI thread
        /// </summary>
        public static string To24BITBinary(this int a)
        {
            string res = string.Empty;
            if (a >= 0)
                res = Convert.ToString(a, 2);
            else
            {
                // Number is Negative... We have two push two's complement in 24 bits more elegantly
            }
            return res.PadLeft(24, '0'); ;
        }
    }
}
