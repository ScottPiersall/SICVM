using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator
{
    class SICAssemblyException : Exception
    {
        public SICAssemblyException( String Message)
                : base(Message)
        {

        }
    }


    class SICDivideByZeroException : Exception
    {
        public SICDivideByZeroException(String Message)
                : base(Message)
        {

        }
    }

}
