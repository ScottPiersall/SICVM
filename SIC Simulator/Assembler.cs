using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.PerformanceData;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SIC_Simulator
{

    // Assigned to Riley Strickland 
    // Assigned to Ellis Levine!
    class Symbol
    {
        public readonly string SymbolName;
        public readonly int Address;
        public readonly int DefinedSourceLineNumber;
        public Symbol(string def, int addr, int ln) { SymbolName = def; Address = addr; DefinedSourceLineNumber = ln;}
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

    static class Parser
    {
        private static char[] InvalidSymbolCharacters = { ' ', '$', '!', '=', '+', '-', '(', ')', '@' };

        public static bool IsDirective(string who) => Assembler.DirectivesSet.Contains(who);
        public static bool IsOpCode(string who) => Assembler.InstructionsMap.ContainsKey(who);
        public static bool IsComment(string who) => who.Length > 0 && who[0] == '#';
        public static bool IsSymbol(string who)
        {
            if (who.Length > 6) return false;
            if (who[0] < 'A' || who[0] > 'Z') return false; //first letter is non-alphanumeric
            return InvalidSymbolCharacters.Any(x => who.Contains(x)); //contains any bad characters? TODO: Stricter Checking as C# is UFT-16...
        }

        public static IEnumerable<Instruction> Parse(string Source)
        {
            string[] lines = Source.Split(new[] { "\n", "\r\n" }, StringSplitOptions.None);
            for(int i = 0; i < lines.Length; ++i)
            {
                string str = lines[i];
                if (IsComment(str) || str.Trim().Length == 0)
                {
                    Debug.WriteLine($"Got a comment line: {str}");
                    continue;
                }

                string symbol = "", opcode = "", operand = "", comment = "";
                string[] pieces = str.Split(new[] { '\t' }, StringSplitOptions.None);

                /* Sometimes there's excessive tabs at the end of a line, this ensures we cap at 4 tabs. */
                switch (pieces.Length > 4 ? 4 : pieces.Length)
                {
                    case 4:
                        comment = pieces[3];
                        goto case 3;
                    case 3:
                        operand = pieces[2];
                        goto case 2;
                    case 2:
                        opcode = pieces[1];
                        goto case 1;
                    case 1:
                        symbol = pieces[0];
                        break;
                }
                yield return new Instruction(symbol, opcode, operand, comment, i);
            }
        }
    }

    class Assembler
    {
        public static readonly Dictionary<string, int> InstructionsMap = new Dictionary<string, int>
        {
            {"ADD", 0x18}, {"ADDF",0x58}, {"ADDR", 0x90}, {"AND", 0x40}, {"CLEAR", 0xB4}, {"COMP", 0x28}, {"COMPF", 0x88},
            {"COMPR", 0xA0}, {"DIV", 0x24}, {"DIVF", 0x64}, {"DIVR", 0x9C}, {"FIX", 0xC4}, {"FLOAT", 0xC0}, {"HIO", 0xC0},
            {"J", 0x3C}, {"JEQ", 0x30}, {"JGT", 0x34}, {"JLT", 0x38}, {"JSUB", 0x48}, {"LDA", 0x00}, {"LDB", 0x68}, {"LDCH", 0x50},
            {"LDF", 0x70}, {"LDL", 0x08}, {"LDS", 0x6C}, {"LDT", 0x74}, {"LDX", 0x04}, {"LPS", 0xD0}, {"MUL", 0x20}, {"MULF", 0x60},
            {"MULR", 0x98}, {"NORM",0xC8}, {"OR",0x44}, {"RD",0xD8}, {"RMO", 0xAC}, {"RSUB", 0x4C}, {"SHIFTL", 0xA4}, {"SHIFTR", 0xA8},
            {"SIO", 0xF0}, {"SSK", 0xEC}, {"STA", 0x0C}, {"STB", 0x78}, {"STCH", 0x54}, {"STF", 0x80}, {"STI", 0xD4},
            {"STL", 0x14}, {"STS", 0x7C}, {"STSW", 0xE8}, {"STT", 0x84}, {"STX", 0x10}, {"SUB", 0x1C}, {"SUBF", 0x5C},
            {"SUBR", 0x94},{"SVC", 0xB0}, {"TD", 0xE0}, {"TIO", 0xF8} ,{"TIX", 0x2C}, {"TIXR", 0xB8},{ "WD", 0xDC}
        };

        public static readonly HashSet<string> DirectivesSet = new HashSet<string> { "END", "BYTE", "WORD", "RESB", "RESW", "RESR", "EXPORTS", "START" };



        public string ObjectCode { get; private set; }
        public string SICSource { get; private set; } /* let's protect our variables from mutations */

        public Dictionary<string, Symbol> SymbolTable { get; private set; } = new Dictionary<string, Symbol>();
        public List<Instruction> Instructions { get; private set; } = new List<Instruction>();

        public Assembler(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Input file {Path.GetFileNameWithoutExtension(filePath)} does not exist.");
                return; /* todo an error here */
            }

            SICSource = File.ReadAllText(filePath);
            MessageBox.Show(SICSource);

            Instructions = Parser.Parse(SICSource).ToList();
            pass1();
        }


        private void pass1()
        {
            bool explicitStart = false, addressExceeded = false, explicitEnd = false;
            int totalInstructions = 0;



            foreach (Instruction instruction in Instructions)
            {
                if (explicitEnd)
                {
                    Debug.WriteLine("Instruction after END, this instruction will be ignored.");
                    continue;
                }

                if (instruction.SymbolName.Length != 0 && Parser.IsSymbol(instruction.SymbolName))
                {
                    SymbolTable.Add(instruction.SymbolName, new Symbol(instruction.SymbolName, 0, instruction.LineNumber));
                }
            }
        }



    }
}
