using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace SIC_Simulator
{

    // Assigned to Kris Wieben
    class Instruction
    {
        public readonly string Symbol;
        public readonly string OpCode;
        public readonly string Operand;
        public readonly int LineNumber;
        public int MemoryAddress;
        public Instruction(String Symbol, String OpCode, String Operand, int LineNumber)
        {
            this.Symbol = Symbol;
            this.OpCode = OpCode;
            this.Operand = Operand;
            this.LineNumber = LineNumber;
        }
    }

    class ErrorMessage
    {
        public enum EXCEPTION
        {
            DUPLICATE_END,
            SYMBOL_IN_DIRECTIVE,
            SYMBOL_FORMAT,
            SYMBOL_DUPLICATE_DECLARATION,
            SYMBOL_NOT_DEFINED,
            MEMORY_SIZE_,
            START_NOT_DEFINED,
            BYTE_DELIMITER,
            BYTE_HEX_FORMAT,
            BYTE_FLAG,
            WORD_FORMAT,
            WORD_SIZE_,
            GENERIC
        };
        private readonly EXCEPTION Message;
        private readonly string Header = "ASSEMBLER ERROR";
        private readonly string Line;

        public ErrorMessage(string line, EXCEPTION message)
        {
            Line = line;
            Message = message;
        }

        private string GetMessage()
        {
            switch (Message)
            {
                case EXCEPTION.DUPLICATE_END:
                    return Header + "ENCOUNTERED END YET ADDITIONAL SCANNING OCCURRED.";
                case EXCEPTION.SYMBOL_IN_DIRECTIVE:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.SYMBOL_FORMAT:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.SYMBOL_DUPLICATE_DECLARATION:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.SYMBOL_NOT_DEFINED:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.MEMORY_SIZE_:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.START_NOT_DEFINED:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.BYTE_DELIMITER:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.BYTE_HEX_FORMAT:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.BYTE_FLAG:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.WORD_FORMAT:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.WORD_SIZE_:
                    return "LABEL {0} IS A DIRECTIVE";
                case EXCEPTION.GENERIC:
                    return "LABEL {0} IS A DIRECTIVE";
                default:
                    return "UNKNOWN ERROR";
            }
        }

        public override string ToString()
        {
            return String.Format(GetMessage(), Header, Line);
        }
    }

    class Assembler
    {
        
        private static readonly char[] InvalidSymbolCharacters = { ' ', '$', '!', '=', '+', '-', '(', ')', '@' };
        HashSet<string> DeviceName = new HashSet<string>(); // new
        public static bool IsInstrcution(string who) => Assembler.Instructions.ContainsKey(who);
        public static bool IsDirective(string who) => Assembler.Directives.Contains(who);
        public static bool IsNotSymbol(string who)
        {
            if (String.IsNullOrEmpty(who))
                return false;
            if (who.Length > 6)
                return true;
            if (Char.IsDigit(who[0]))
                return true;
            return InvalidSymbolCharacters.Any(x => who.Contains(x)); //contains any bad characters? TODO: Stricter Checking as C# is UFT-16...
        }

        public static readonly Dictionary<string, int> Instructions = new Dictionary<string, int>
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

        public static readonly HashSet<string> Directives = new HashSet<string> { "END", "BYTE", "WORD", "RESB", "RESW", "RESR", "EXPORTS", "START" };



        public string ObjectCode { get; private set; }
        public string SICSource { get; private set; } /* let's protect our variables from mutations */
        public string ModRecords { get; private set; }
        public string InstructionSource { get; private set; } /* let's protect our variables from mutations */

        public Dictionary<string, Instruction> SymbolTable { get; private set; } = new Dictionary<string, Instruction>();
        public List<Instruction> InstructionList { get; private set; } = new List<Instruction>();

        private enum PROCESS { INIT, END, START, ERROR }
        PROCESS _process = PROCESS.INIT;
        public Assembler(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show($"Input file {Path.GetFileNameWithoutExtension(filePath)} does not exist.");
                return; /* todo an error here */
            }

            /*
             ______________
            < BEGIN PASS I >
             --------------
            */
            StreamReader file = new StreamReader(filePath);
            int memory_address = 0, line_counter = 0;
            String output = "ASSEMBLY ERROR\n"; // error message header
            String line;
            pass_one();
            if (_process == PROCESS.ERROR)
            {
                ObjectCode = "";
                return;
            }

            /*
             _______________
            < BEGIN PASS II >
             ---------------
            */

            Instruction head = InstructionList.First();
            Instruction tail = InstructionList.Last();
            ObjectCode += String.Format("H{0,-6}{1,6:X6}{2,6:X6}\n", head.Symbol, head.MemoryAddress, tail.MemoryAddress - head.MemoryAddress);
            memory_address = line_counter = 0;
            bool first = true, NotSkipping = true;
            memory_address = head.MemoryAddress;
            SICSource = "";
            pass_two();
            if (_process == PROCESS.ERROR)
            {
                ObjectCode = "";
                return;
            }

            InstructionSource = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\n", "Line", "Address", "Symbol", "OpCode", "Operand", "CurrentValue");
            //InstructionSource = String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", "Line", "Address", "Symbol", "OpCode", "Operand");

            foreach (Instruction tmp in InstructionList)
            {
                InstructionSource += String.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", tmp.LineNumber, tmp.MemoryAddress.ToString("X"), tmp.Symbol, tmp.OpCode, tmp.Operand);
            }

            void pass_one()
            {
                while ((line = file.ReadLine()) != null)
                {
                    //If there is a comment, remove it
                    line = line.Split('#')[0];

                    line_counter++;

                    if (String.IsNullOrEmpty(line.Trim()))
                    { // skip comments and blank lines
                        continue;
                    }


                    line = Regex.Replace(line, @"\s+(?=([^']*'[^']*')*[^']*$)", "\t"); // clean line for assembler

                    if (_process == PROCESS.END)
                    {
                        output += String.Format("{0}\nLine {1}: REACHED END DIRECTIVE BUT STILL SCANNING CODE", line, line_counter);
                        MessageBox.Show(output);
                        _process = PROCESS.ERROR;
                        return;
                    }

                    String[] lineArray = line.Split('\t');

                    if (lineArray[0] is null || lineArray[1] is null)
                    { // something is wrong with the SIC file format
                        output += String.Format("{0}\nLine {1}: MALFORMED SIC FILE", line, line_counter);
                        MessageBox.Show(output);
                        _process = PROCESS.ERROR;
                        return;
                    }

                    string operand = (lineArray.Length == 2 ) ? "" : lineArray[2];
                    Instruction instruction_line = new Instruction(lineArray[0], lineArray[1], operand, line_counter);

                    if (instruction_line.Symbol.Length != 0)
                    {
                        if (IsDirective(instruction_line.Symbol))
                        {
                            output += String.Format("{0}\nLine {1}: DIRECTIVE IN SYMBOL FIELD", line, line_counter);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }

                        if (IsNotSymbol(instruction_line.Symbol))
                        {
                            output += String.Format("{0}\nLine {1}: INVALID SYMBOL", line, line_counter);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }

                        if (SymbolTable.ContainsKey(instruction_line.Symbol))
                        {
                            output += String.Format("{0}\nLine {1}: DUPLICATE SYMBOL", line, line_counter);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }

                        if (instruction_line.OpCode.Equals("START"))
                        {
                            _process = PROCESS.START;

                            if (Int32.TryParse(instruction_line.Operand, System.Globalization.NumberStyles.HexNumber, null, out memory_address)) // check if hex value
                            {
                                instruction_line.MemoryAddress = memory_address;
                                if (instruction_line.MemoryAddress >= 32768)
                                {
                                    //throwError(MEMORY_SIZE_);
                                }

                                SymbolTable.Add(instruction_line.Symbol, instruction_line);
                                InstructionList.Add(instruction_line);
                            }
                            else
                            {
                                output += String.Format("{0}\nLine {1}: MALFORMED BYTE FLAG", line, line_counter);
                                MessageBox.Show(output);
                                _process = PROCESS.ERROR;
                                return;
                            }

                            continue;
                        }
                        instruction_line.MemoryAddress = memory_address;
                        SymbolTable.Add(instruction_line.Symbol, instruction_line);
                    }

                    InstructionList.Add(instruction_line);

                    // DIDN'T FIND THE START DIRECTIVE ON THE FIRST PASS
                    if (_process != PROCESS.START)
                    {
                        output += String.Format("{0}\nLine {1}:", line, "START DIRECTIVE NOT DEFINED");
                        MessageBox.Show(output);
                        _process = PROCESS.ERROR;
                        return;
                    }


                    instruction_line.MemoryAddress = memory_address;

                    if (instruction_line.OpCode.Equals("END"))
                    {
                        _process = PROCESS.END;
                        continue;
                    }

                    // START MEMORY INCREASE
                    int len = 0; // var for numbers..
                    if (Instructions.ContainsKey(instruction_line.OpCode))
                    {
                        memory_address += 3;
                        
                        if (instruction_line.OpCode.Equals("WD") || instruction_line.OpCode.Equals("TD") || instruction_line.OpCode.Equals("RD") )
                        {
                            DeviceName.Add(instruction_line.Operand);

                        }
                    }
                    else if (instruction_line.OpCode.Equals("WORD"))
                    {
                        if (Int32.TryParse(instruction_line.Operand, out len) && (len >= -(1<<23)) && (len < (1<<23)))
                        {
                            memory_address += 3;
                        }
                        else
                        {
                            output += String.Format("{0}\nLine {1} CONSTANT {2} FORMAT VALIDATION FAILED:", line, instruction_line.LineNumber, instruction_line.Operand);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }

                        //if (len >= MAX_INT_SIZE || len <= MIN_INT_SIZE) // check max int size
                        //    throwError(WORD_SIZE_);
                    }
                    else if (instruction_line.OpCode.Equals("RESW"))
                    {
                        if (Int32.TryParse(instruction_line.Operand, out len) && len > 0)
                        {
                            memory_address += 3 * len;
                        }
                        else
                        {
                            output += String.Format("{0}\nLine {1} CONSTANT {2} FORMAT VALIDATION FAILED:", line, instruction_line.LineNumber, instruction_line.Operand);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }
                    }
                    else if (instruction_line.OpCode.Equals("BYTE"))
                    {
                        len = instruction_line.Operand.Length;
                        if (instruction_line.Operand[0] == 67)
                        { // char
                          //if (isNotByteDelimited(Operand, len))
                          //    throwError(BYTE_DELIMITER);
                            memory_address += (len - 3);
                        }
                        else if (instruction_line.Operand[0] == 88)
                        { // hex
                          //if (isNotByteDelimited(Operand, len))
                          //    throwError(BYTE_DELIMITER);

                            //if (!isHexLiteralStrRange(Operand, 2, len))
                            //    throwError(BYTE_HEX_FORMAT);

                            memory_address += (int)(len - 3) / 2;
                        }
                        else
                        {
                            output += String.Format("{0}\nLine {1}: UNKOWN BYTE FLAG", line, instruction_line.LineNumber);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }
                    }
                    else if (instruction_line.OpCode.Equals("RESB"))
                    {
                        if (Int32.TryParse(instruction_line.Operand, out len) && len > 0)
                        {
                            memory_address += len;
                        }
                        else
                        {
                            output += String.Format("{0}\nLine {1} CONSTANT {2} FORMAT VALIDATION FAILED:", line, instruction_line.LineNumber, instruction_line.Operand);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }
                    }
                    else
                    {
                        output += String.Format("{0}\nLine {1}: UNKNOWN OPCODE OR DIRECTIVE {2}", line, instruction_line.LineNumber, instruction_line.OpCode);
                        MessageBox.Show(output);
                        _process = PROCESS.ERROR;
                        return;
                    }

                    if (memory_address > 32768)
                    {
                        output += String.Format("{0}\nLine {1}: MEMORY ADDRESS EXCEEDS AVILABLE RAM", line, instruction_line.LineNumber);
                        MessageBox.Show(output);
                        _process = PROCESS.ERROR;
                        return;
                    }
                }
            }

            void pass_two()
            {
                string programName = "";
                foreach (Instruction row in InstructionList)
                {
                    if (first)
                    { // skip the header record
                        programName = row.Symbol;
                        first = !first;
                        continue;
                    }

                    if (line_counter == 10)
                    { // save T record 
                        saveTRecord(row.MemoryAddress);
                    }

                    KeyValuePair<String, int> OpCode = Instructions.FirstOrDefault(x => x.Key.Equals(row.OpCode));
                    if (OpCode.Key != null)
                    {
                        setSkippedAddress(row);
                        String[] indexModeSplit = row.Operand.Split(',');
                        if (indexModeSplit[0].Length != 0 && !IsNotSymbol(indexModeSplit[0]))
                        {
                            KeyValuePair<String, Instruction> symbol = SymbolTable.FirstOrDefault(x => x.Key.Equals(indexModeSplit[0]));
                            if (symbol.Key != null)
                            {
                                int memoryAddres = symbol.Value.MemoryAddress;
                                if (indexModeSplit.Length != 1)
                                {// index mode
                                    memoryAddres += 32768; // set X bit
                                }
                                SICSource += String.Format("{0,2:X2}{1,4:X4}", OpCode.Value, memoryAddres);
                                addMRecord(row, programName);
                            }
                            else
                            {
                                output += String.Format("{0} {1} {2}\nLine {3}: UNDEFINED SYMBOL {4}", row.Symbol, row.OpCode, row.Operand, row.LineNumber, row.Operand);
                                MessageBox.Show(output);
                                _process = PROCESS.ERROR;
                                return;
                            }
                        }
                        else
                        {
                            SICSource += String.Format("{0,2:X2}{1,4:X4}", OpCode.Value, 0);
                        }
                    }
                    else if (row.OpCode.Equals("WORD"))
                    {
                        setSkippedAddress(row);
                        int val = Int32.Parse(row.Operand) & 0xFFFFFF;
                        SICSource += String.Format("{0,6:X6}", val);
                        
                        foreach(var x in DeviceName){
                            if(row.Symbol.Contains(x))
                                DeviceDetector(val,row);
                        }
                    }
                    else if (row.OpCode.Equals("BYTE"))
                    {
                        setSkippedAddress(row);
                        if (row.Operand[0] == 67)
                        { // char
                            String[] tmp = row.Operand.Split('\'');
                            int counter = 0;
                            foreach (char ch in tmp[1])
                            {
                                SICSource += String.Format("{0,2:X2}", (byte)ch);
                                counter++;
                                if (SICSource.Length == 60)
                                {
                                    saveTRecord(row.MemoryAddress + counter);
                                    counter = 0;
                                }
                            }
                            line_counter = (int)Math.Ceiling((double)(counter * 2) / 6);
                        }
                        else
                        { // hex
                            String[] tmp = row.Operand.Split('\'');
                            SICSource += String.Format("{0}", tmp[1]);
                            foreach(var x in DeviceName){
                                if(row.Symbol.Contains(x))
                                   DeviceDetector(Convert.ToInt32(tmp[1], 16),row);
                            }
                        }
                    }
                    else if (row.OpCode.Equals("RESB") || row.OpCode.Equals("RESW"))
                    {
                        if (NotSkipping)
                        {
                            saveTRecord(row.MemoryAddress);
                        }
                        else
                        {
                            memory_address = row.MemoryAddress;
                        }
                        NotSkipping = false;
                        continue;
                    }
                    else if (row.OpCode.Equals("END"))
                    {
                        ModRecords = ModRecords.Remove(ModRecords.Length - 1);
                        //Shouldn't be needed anymore newly defined string in assembler and form1
                        //SICSource += "\n" + ModRecords;
                        if (NotSkipping)
                        { // need to handle RESB and RESW directives placed at the bottom of the SIC code
                            saveTRecord(memory_address);
                        }

                        KeyValuePair<String, Instruction> symbol = SymbolTable.FirstOrDefault(x => x.Key.Equals(row.Operand));
                        if (symbol.Key != null)
                        {
                            Instruction firstInstruction = InstructionList.Where(x => x.MemoryAddress >= symbol.Value.MemoryAddress).FirstOrDefault(x => IsInstrcution(x.OpCode)); // lazy mode enabled
                            if (firstInstruction is null)
                            {
                                firstInstruction = head;
                            }
                            ObjectCode += String.Format("E{0,6:X6}", firstInstruction.MemoryAddress); // need the first instruction
                        }
                        else if (String.IsNullOrEmpty(row.Operand))
                        {
                            ObjectCode += String.Format("E{0,6:X6}", head.MemoryAddress); // oops... this is optional.. defaulting to the start directive
                        }
                        else
                        {
                            output += String.Format("{0} {1} {2}\nLine {3}: UNDEFINED SYMBOL {4}", row.Symbol, row.OpCode, row.Operand, row.LineNumber, row.Operand);
                            MessageBox.Show(output);
                            _process = PROCESS.ERROR;
                            return;
                        }
                    }

                    line_counter++;
                }
            }

            void setSkippedAddress(Instruction row)
            {
                if (!NotSkipping)
                {
                    memory_address = row.MemoryAddress;
                }
                NotSkipping = true;
            }

            void saveTRecord(int current_address)
            {
                int lineLength = (int)Math.Ceiling((double)(SICSource.Length / 2)); // count odd length bytes
                ObjectCode += String.Format("T{0,6:X6}{1,2:X2}{2}\n", memory_address, lineLength, SICSource);
                SICSource = "";
                NotSkipping = true;
                memory_address = current_address;
                line_counter = 0;
            }
            
            /// <summary>
            /// Checks Whether Set Device ID Is Within Range During
            /// </summary>
            /// <param name="val">value of Device ID</param>
            /// <param name="row">instruction object</param> 
            void DeviceDetector(int val, Instruction row)
            {
                if(val > 64 || val < 0)
                {
                   output += String.Format("{0} {1} {2}\nLine {3}: Device ID {4} Does Not Exist", row.Symbol, row.OpCode, row.Operand, row.LineNumber, val);
                   MessageBox.Show(output);
                   _process = PROCESS.ERROR;
                   return;
                }
            }

            void addMRecord(Instruction row, string programName)
            {
                int address = row.MemoryAddress;
                if (row.OpCode.Contains(','))
                {
                    address += 0x8000;
                }
                int modifiedaddress = address + 1;
                ModRecords += String.Format("M{0}04+{1}\n", modifiedaddress.ToString("X6"), programName);

            }

            //MessageBox.Show(ObjectCode);
        }
    }
}

