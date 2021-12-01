namespace SICVirtualMachine.Model
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
