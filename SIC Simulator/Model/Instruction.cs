using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator.Model
{
    internal class Instruction
    {
        public string Symbol { get; }
        public string OpCode { get; }
        public string Operand { get; }
        public int LineNumber { get; }
        public int MemoryAddress { get; set; }

        public Instruction(string symbol, string opCode, string operand, int lineNumber)
        {
            Symbol = symbol;
            OpCode = opCode;
            Operand = operand;
            LineNumber = lineNumber;
        }
    }
}
