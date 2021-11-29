using System;

namespace SIC_Simulator
{
    internal class SICAssemblyException : Exception
    {
        public SICAssemblyException(string Message) : base(Message)
        {

        }
    }

    internal class SICDivideByZeroException : Exception
    {
        public SICDivideByZeroException(string Message) : base(Message)
        {

        }
    }
}
