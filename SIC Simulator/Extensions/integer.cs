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
            uint b = (uint)a;
            b &= 0xFFFFFF; //only read 3 bytes
            res = Convert.ToString(b, 2);
            return res.PadLeft(24, '0'); ;
        }
    }
}
