using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator
{

    // Assigned to Riley Strickland 
    // Assigned to Ellis Levine!

    class Symbol
    {
        public readonly string SymbolName;
        public readonly int Address;
        public readonly int DefinedSourceLineNumber;
        public Symbol(string def, int addr, int ln) { SymbolName = def; Address = addr; DefinedSourceLineNumber = ln; }
    }

    class Instruction
    {
        public readonly string SymbolName;
        public readonly string OpCode;
        public readonly string Operand;
        public readonly string Comment;
        public readonly int LineNumber;
        public Instruction(string sn, string op, string rand, string comm, int ln) { SymbolName = sn; OpCode = op; Operand = rand; Comment = comm; LineNumber = ln; }
    }

    class Assembler
    {
        public String ObjectCode;
        public String SICSource;

        // Expose the Symbol table
        // SO that we can show it on the virtual machine

        // Symbol Table   Address, SynbolName, DefinedSourceLineNumber




    }
}
