using SICVirtualMachine.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace SICVirtualMachine.SIC
{

    // Assigned to Kris Wieben

    internal class Assembler
    {
        private static readonly char[] InvalidSymbolCharacters = { ' ', '$', '!', '=', '+', '-', '(', ')', '@' };
        private static readonly HashSet<string> Directives = new HashSet<string> { "END", "BYTE", "WORD", "RESB", "RESW", "RESR", "EXPORTS", "START" };

        public string ObjectCode { get; private set; }
        public string SICSource { get; private set; }
        public string InstructionSource { get; private set; }

        public Dictionary<string, Instruction> SymbolTable { get; private set; } = new Dictionary<string, Instruction>();
        public List<Instruction> InstructionList { get; private set; } = new List<Instruction>();

        private enum PROCESS { INIT, END, START, ERROR }

        private PROCESS _process = PROCESS.INIT;

        public Assembler(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"Input file {filePath} does not exist.");
            }

            StreamReader file = new StreamReader(filePath);

            PassOne(file);
            PassTwo();

            InstructionSource = string.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", "Line", "Address", "Symbol", "OpCode", "Operand");

            foreach (Instruction tmp in InstructionList)
            {
                InstructionSource += string.Format("{0}\t{1}\t{2}\t{3}\t{4}\n", tmp.LineNumber, tmp.MemoryAddress.ToString("X"), tmp.Symbol, tmp.OpCode, tmp.Operand);
            }
        }

        void PassOne(StreamReader file)
        {
            int memory_address = 0,line_counter = 0;
            string line, tmpLine;

            while ((line = file.ReadLine()) != null)
            {
                tmpLine = line;
                tmpLine = tmpLine.Trim();
                line_counter++;

                if (string.IsNullOrEmpty(tmpLine) || tmpLine[0] == '#')
                { // skip comments and blank lines
                    continue;
                }

                line = Regex.Replace(line, @"\s+(?=([^']*'[^']*')*[^']*$)", "\t"); // clean line for assembler

                if (_process == PROCESS.END)
                {
                    throw new Exception($"{line}\nLine {line_counter}: REACHED END DIRECTIVE BUT STILL SCANNING CODE");
                }

                string[] lineArray = line.Split('\t');

                if (lineArray[0] is null || lineArray[1] is null)
                { 
                    throw new Exception($"{line}\nLine {line_counter}: MALFORMED SIC FILE");
                }

                string operand = (lineArray.Length == 2) ? "" : lineArray[2];
                Instruction instruction_line = new Instruction(lineArray[0], lineArray[1], operand, line_counter);

                if (instruction_line.Symbol.Length != 0)
                {
                    if (IsDirective(instruction_line.Symbol))
                    {
                        throw new Exception($"{line}\nLine {line_counter}: DIRECTIVE IN SYMBOL FIELD");
                    }

                    if (IsNotSymbol(instruction_line.Symbol))
                    {
                        throw new Exception($"{line}\nLine {line_counter}: INVALID SYMBOL");
                    }

                    if (SymbolTable.ContainsKey(instruction_line.Symbol))
                    {
                        throw new Exception($"{line}\nLine {line_counter}: DUPLICATE SYMBOL");
                    }

                    if (instruction_line.OpCode.Equals("START"))
                    {
                        _process = PROCESS.START;

                        if (int.TryParse(instruction_line.Operand, System.Globalization.NumberStyles.HexNumber, null, out memory_address)) // check if hex value
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
                            throw new Exception($"{line}\nLine {line_counter}: MALFORMED BYTE FLAG");
                        }

                        continue;
                    }
                    instruction_line.MemoryAddress = memory_address;
                    SymbolTable.Add(instruction_line.Symbol, instruction_line);
                }

                InstructionList.Add(instruction_line);

                if (_process != PROCESS.START)
                {
                    throw new Exception($"Line {line}: START DIRECTIVE NOT DEFINED");
                }


                instruction_line.MemoryAddress = memory_address;

                if (instruction_line.OpCode.Equals("END"))
                {
                    _process = PROCESS.END;
                    continue;
                }

                // START MEMORY INCREASE
                int len = 0; // var for numbers..
                if (Enum.IsDefined(typeof(OPCode), instruction_line.OpCode))
                {
                    memory_address += 3;
                }
                else if (instruction_line.OpCode.Equals("WORD"))
                {
                    if (int.TryParse(instruction_line.Operand, out len))
                    {
                        memory_address += 3;
                    }
                    else
                    {
                        throw new Exception($"{line}\nLine {instruction_line.LineNumber} CONSTANT {instruction_line.Operand} FORMAT VALIDATION FAILED:");
                    }

                    //if (len >= MAX_INT_SIZE || len <= MIN_INT_SIZE) // check max int size
                    //    throwError(WORD_SIZE_);
                }
                else if (instruction_line.OpCode.Equals("RESW"))
                {
                    if (int.TryParse(instruction_line.Operand, out len))
                    {
                        memory_address += 3 * len;
                    }
                    else
                    {
                        throw new Exception($"{line}\nLine {instruction_line.LineNumber} CONSTANT {instruction_line.Operand} FORMAT VALIDATION FAILED:");
                    }
                }
                else if (instruction_line.OpCode.Equals("BYTE"))
                {
                    len = instruction_line.Operand.Length;
                    if (instruction_line.Operand[0] == 'C')
                    { // char
                      //if (isNotByteDelimited(Operand, len))
                      //    throwError(BYTE_DELIMITER);
                        memory_address += (len - 3);
                    }
                    else if (instruction_line.Operand[0] == 'X')
                    { // hex
                      //if (isNotByteDelimited(Operand, len))
                      //    throwError(BYTE_DELIMITER);

                        //if (!isHexLiteralStrRange(Operand, 2, len))
                        //    throwError(BYTE_HEX_FORMAT);

                        memory_address += (len - 3) / 2;
                    }
                    else
                    {
                        throw new Exception($"{line}\nLine {instruction_line.LineNumber}: UNKOWN BYTE FLAG");
                    }
                }
                else if (instruction_line.OpCode.Equals("RESB"))
                {
                    if (int.TryParse(instruction_line.Operand, out len))
                    {
                        memory_address += len;
                    }
                    else
                    {
                        throw new Exception($"{line}\nLine {instruction_line.LineNumber} CONSTANT {instruction_line.Operand} FORMAT VALIDATION FAILED:");
                    }
                }
                else
                {
                    throw new Exception($"{line}\nLine {instruction_line.LineNumber}: UNKNOWN OPCODE OR DIRECTIVE {instruction_line.OpCode}");
                }

                if (memory_address > 32768)
                {
                    throw new Exception($"{line}\nLine {instruction_line.LineNumber}: MEMORY ADDRESS EXCEEDS AVILABLE RAM");
                }
            }
        }

        void PassTwo()
        {
            Instruction head = InstructionList.First();
            Instruction tail = InstructionList.Last();

            ObjectCode += string.Format("H{0,-6}{1,6:X6}{2,6:X6}\n", head.Symbol, head.MemoryAddress, tail.MemoryAddress - head.MemoryAddress);

            int memory_address = head.MemoryAddress;
            int line_counter = 0;

            bool first = true;
            bool notSkipping = true;

            SICSource = "";

            foreach (Instruction row in InstructionList)
            {
                if (first)
                { // skip the header record
                    first = !first;
                    continue;
                }

                if (line_counter == 10)
                { // save T record 
                    SaveTRecord(row.MemoryAddress);
                }

                if (Enum.TryParse(row.OpCode, out OPCode opCode))
                {
                    SetSkippedAddress(row);
                    string[] indexModeSplit = row.Operand.Split(',');

                    if (indexModeSplit[0].Length != 0 && !IsNotSymbol(indexModeSplit[0]))
                    {
                        KeyValuePair<string, Instruction> symbol = SymbolTable.FirstOrDefault(x => x.Key.Equals(indexModeSplit[0]));

                        if (symbol.Key != null)
                        {
                            int memoryAddress = symbol.Value.MemoryAddress;

                            if (indexModeSplit.Length != 1)
                            {// index mode
                                memoryAddress += 0x800000; // set X bit
                            }

                            SICSource += string.Format("{0,2:X2}{1,4:X4}", (int)opCode, memoryAddress);
                        }
                        else
                        {
                            throw new Exception($"{row.Symbol} {row.OpCode} {row.Operand}\nLine {row.LineNumber}: UNDEFINED SYMBOL {row.Operand}");
                        }
                    }
                    else
                    {
                        SICSource += string.Format("{0,2:X2}{1,4:X4}", (int)opCode, 0);
                    }
                }
                else if (row.OpCode.Equals("WORD"))
                {
                    SetSkippedAddress(row);

                    int val = int.Parse(row.Operand) & 0xFFFFFF;
                    SICSource += string.Format("{0,6:X6}", val);
                }
                else if (row.OpCode.Equals("BYTE"))
                {
                    SetSkippedAddress(row);

                    if (row.Operand[0] == 'C')
                    { // char
                        string[] tmp = row.Operand.Split('\'');
                        int counter = 0;

                        foreach (char ch in tmp[1])
                        {
                            SICSource += string.Format("{0,2:X2}", (byte)ch);
                            counter++;

                            if (SICSource.Length == 60)
                            {
                                SaveTRecord(row.MemoryAddress + counter);
                                counter = 0;
                            }
                        }

                        line_counter = (int)Math.Ceiling((double)(counter * 2) / 6);
                    }
                    else
                    { // hex
                        string[] tmp = row.Operand.Split('\'');
                        SICSource += string.Format("{0}", tmp[1]);
                    }
                }
                else if (row.OpCode.Equals("RESB") || row.OpCode.Equals("RESW"))
                {
                    if (notSkipping)
                    {
                        SaveTRecord(row.MemoryAddress);
                    }
                    else
                    {
                        memory_address = row.MemoryAddress;
                    }
                    notSkipping = false;
                    continue;
                }
                else if (row.OpCode.Equals("END"))
                {
                    if (notSkipping)
                    { // need to handle RESB and RESW directives placed at the bottom of the SIC code
                        SaveTRecord(memory_address);
                    }

                    KeyValuePair<string, Instruction> symbol = SymbolTable.FirstOrDefault(x => x.Key.Equals(row.Operand));
                    if (symbol.Key != null)
                    {
                        Instruction firstInstruction = InstructionList.FirstOrDefault(x => x.MemoryAddress >= symbol.Value.MemoryAddress && IsInstruction(x.OpCode));

                        if (firstInstruction is null)
                        {
                            firstInstruction = head;
                        }

                        ObjectCode += string.Format("E{0,6:X6}", firstInstruction.MemoryAddress); // need the first instruction
                    }
                    else if (string.IsNullOrEmpty(row.Operand))
                    {
                        ObjectCode += string.Format("E{0,6:X6}", head.MemoryAddress); // oops... this is optional.. defaulting to the start directive
                    }
                    else
                    {
                        throw new Exception($"{row.Symbol} {row.OpCode} {row.Operand}\nLine {row.LineNumber}: UNDEFINED SYMBOL {row.Operand}");
                    }
                }

                line_counter++;
            }

            void SetSkippedAddress(Instruction row)
            {
                if (!notSkipping)
                {
                    memory_address = row.MemoryAddress;
                }

                notSkipping = true;
            }

            void SaveTRecord(int current_address)
            {
                int lineLength = (int)Math.Ceiling((double)(SICSource.Length / 2)); // count odd length bytes

                ObjectCode += string.Format("T{0,6:X6}{1,2:X2}{2}\n", memory_address, lineLength, SICSource);
                SICSource = "";
                notSkipping = true;
                memory_address = current_address;
                line_counter = 0;
            }
        }

        public static bool IsInstruction(string who)
        {
            return Enum.TryParse<OPCode>(who, out _);
        }

        public static bool IsDirective(string who)
        {
            return Directives.Contains(who);
        }

        public static bool IsNotSymbol(string who)
        {
            if (string.IsNullOrEmpty(who))
            {
                return false;
            }

            if (who.Length > 6)
            {
                return true;
            }

            if (char.IsDigit(who[0]))
            {
                return true;
            }

            return InvalidSymbolCharacters.Any(x => who.Contains(x)); //contains any bad characters? TODO: Stricter Checking as C# is UTF-16...
        }
    }
}

