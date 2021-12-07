using System;

namespace SICVirtualMachine.Model
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
