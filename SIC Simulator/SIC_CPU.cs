using System;
using System.Text;

namespace SIC_Simulator
{

    [Serializable()]
    internal class SIC_CPU
    {
        public static readonly int NumDevices = 65;
        public int CurrentProgramEndAddress = 0;
        public int CurrentProgramStartAddress = 0;

        public int PC = 0;
        public int A = 0;
        public int X = 0;
        public int L = 0;
        public int SW = 0;

        public byte[] MemoryBytes;

        public SIC_Device[] Devices;


        public bool MachineStateIsNotSaved = false;

        private StringBuilder MicroSteps;

        public string MicrocodeSteps => MicroSteps.ToString();

        /// <summary>
        /// Constructs a SIC VM (CPU and Memory)
        /// </summary>
        /// <param name="ZeroizeBytes"></param>
        public SIC_CPU(bool ZeroizeBytes)
        {
            PC = A = X = L = SW = 0;
            MemoryBytes = new byte[32768];

            if (ZeroizeBytes == true)
            {
                ZeroAllMemory();
            }
            else
            {
                RandomizeMemory();
            }

            Devices = new SIC_Device[NumDevices];

            for (int i = 0; i < NumDevices; i++)
            {
                Devices[i] = new SIC_Device(i);
            }
            MicroSteps = new StringBuilder();
            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Randomize all memory bytes in the SIC
        /// Simulates filling memory with "JUNK"
        /// Way to see if programs specifically set memory when needed
        /// and do not rely on "JUNK" values
        /// </summary>
        public void RandomizeMemory()
        {
            Random rnd = new Random(Guid.NewGuid().GetHashCode());
            rnd.NextBytes(MemoryBytes);
            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Sets all memory bits to ZERO in the SIC
        /// </summary>
        public void ZeroAllMemory()
        {
            byte zero = 0;

            for (int x = 0; x < 32768; x++)
            {
                MemoryBytes[x] = zero;
            }

            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Performs a "hard" reset of the virtual machine.
        /// 1. All registers and SW are set to zero
        /// 2. All Memory is zeroed.
        /// </summary>
        public void ResetVM()
        {
            PC = 0;
            A = 0;
            X = 0;
            L = 0;
            SW = 0;
            ZeroAllMemory();
            MicroSteps = new StringBuilder();
        }

        /// <summary>
        /// Fetches a 24-bit word from Memory and places it into an integer
        /// </summary>
        /// <param name="Address">Address of Memory Location in SIC [0..32767]</param>
        /// <returns></returns>
        public int FetchWord(int Address)
        {
            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < 3; ++i)
            {
                char ch = (char)MemoryBytes[Address++];
                num2 = ch;
                num2 = num2 & 0x000000FF;
                num1 = num1 | num2;
                num1 = num1 << 8;
            }
            num1 = num1 >> 8;

            return num1;

        }

        /// <summary>
        /// Fetches Byte in Memory and returns character type
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public char FetchByte(int Address)
        {
            return (char)MemoryBytes[Address];
        }

        public void StoreWord(int Address, int data)
        {
            byte b;
            for (int i = 2; i >= 0; --i)
            {
                b = (byte)(data & 0xFF);
                MemoryBytes[Address + i] = b;
                data = data >> 8;
            }
            MachineStateIsNotSaved = true;
        }

        public void StoreByte(int address, byte data)
        {
            MemoryBytes[address] = data;
            MachineStateIsNotSaved = true;
        }

        //New Code Segment By Brandon And Nick

        //class for MOD constructor that holds info for Modification
        private class Mod
        {
            public int Address { get; private set; }
            public int Half { get; private set; }
            public bool Flag { get; private set; }
            public Mod Next { get; set; }
            public bool Error { get; private set; }

            //if our head is which is a place holder is returned then nothing was found in search. sets its boolean to true
            public void SetError()
            {
                Error = true;
            }

            //sets all values for created mod
            public void set(int address, int half, bool flag)
            {
                Address = address;
                Half = half;
                Flag = flag;
            }

            //searches linked list for Mod record matching T-record starting address, if head is returned as place holder nothing was found
            public Mod Search(Mod head, int add)
            {
                Mod error = new Mod();
                error.SetError();

                while (head.Next != null)
                {
                    if (add != head.Address)
                    {
                        head = head.Next;
                    }
                    else
                    {
                        return head;
                    }
                }

                return error;
            }
        }

        /// <summary>
        /// method steps the CPU one time. 
        /// FETCH->DECODE->EXECUTE
        /// </summary>
        public void PerformStep()
        {
            // 1. Fetch the next instruction by pulling the WORD from memory
            //      pointed to by PC (Address of Next Instruction)
            //
            // 2. Decode the instruction
            //
            // 3. Execute Instruction
            //      --> Execute will advance PC by 3 bytes(1 word) or change it to
            //      -->   a target address if we have a JUMP, or RSUB

            int NextInstruction;
            int op = 0;
            int TA = 0;

            NextInstruction = FetchWord(PC);

            DecodeInstruction(NextInstruction, ref op, ref TA);

            ExecuteInstruction(op, TA);
            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Returns a human-readable  description of the instruction and operand value located
        /// at Address. The string has two parts, delimited by a |
        /// mnemonic target address | complete description of instruction and result
        /// </summary>
        /// <param name="address">Absolute address of 3-byte instruction</param>
        /// <returns>String with description</returns>
        public string GetInstructionDescription(int address)
        {

            int word = FetchWord(address); // Fetch the Word at Address
            int targetAddress = word & 0x7FFF;
            int opCode = (word & 0xFF0000) >> 16;

            int xBit = (word & 0x8000);
            bool indexed = (xBit > 0);

            string result;
            string details = string.Empty;
            string effect = string.Empty;
            switch (opCode)
            {
                case 0x18: //   ADD
                    result = "ADD";
                    details = "Add Value in Target Address to Register A";
                    effect = "A <- (A) + (TA)";
                    break;

                case 0x40: //   AND
                    result = "AND";
                    details = "Perform Bitwise AND on Value in Target Address and Register A, store result in A";
                    effect = "A <- (A) && (TA)";
                    break;

                case 0x28:  // CMP   (Compare and set Status Word SW)
                    result = "CMP";
                    break;

                case 0x24: // DIV 
                    result = "DIV";
                    details = "Divide Register A by Value in Target Address ";
                    effect = "A <- (A) / (TA)";
                    break;

                case 0x3C: //   J 
                    result = "J";
                    details = "Perform Unconditional Jump to Target Address";
                    effect = "PC <- (TA)";
                    break;

                case 0x30: //   JEQ 
                    result = "JEQ";
                    details = "Perform Conditional Jump to Target Address when CC = 00";
                    effect = "PC <- (TA) if CC = 00";
                    break;

                case 0x34: //   JGT 
                    result = "JGT";
                    details = "Perform Conditional Jump to Target Address when CC = 10";
                    effect = "PC <- (TA) if CC = 10";
                    break;

                case 0x38: //   JLT 
                    result = "JLT";
                    details = "Perform Conditional Jump to Target Address when CC = 01";
                    effect = "PC <- (TA) if CC = 01";
                    break;

                case 0x48: // JSUB      (Jump to subroutine starting at TA. Preserve PC by storing in L)
                    result = "JSUB";
                    details = "Jump to Subroutine at Target Address. Preserve PC By Storing in L";
                    effect = "L <- PC; PC <- (TA)";
                    break;

                case 0x00: // LDA 
                    result = "LDA";
                    details = "Load Value in Target Address to Register A";
                    effect = "A <- (TA)";
                    break;

                case 0x50: //  LDCH
                    result = "LDCH";
                    details = "Load Character from Device Specified in Target Address to Rightmost Byte in A";
                    effect = "A[rightmost byte] <- Device(TA)";
                    break;

                case 0x08: //  LDL 
                    result = "LDL";
                    details = "Load Value in Target Address to Register L";
                    effect = "L <- (TA)";
                    break;

                case 0x04: //  LDX 
                    result = "LDX";
                    details = "Load Value in Target Address to Register X";
                    effect = "X <- (TA)";
                    break;

                case 0x20:  // MUL 
                    result = "MUL";
                    details = "Multiple Value in Target Address by Register A Store in A";
                    effect = "A <- (A) * (TA)";
                    break;

                case 0x44: //   OR 
                    result = "OR";
                    details = "Perform Bitwise OR on Value in Target Address and Register A, store result in A";
                    effect = "A <- (A) || (TA)";
                    break;

                case 0x4C: //    RSUB
                    result = "RSUB";
                    details = "Return from Subroutine. ";
                    effect = "PC <- (L)";
                    break;

                case 0x0C: //   STA         (Stores contents of A in Target Address)
                    result = "STA";
                    details = "Store Value in Register A to Target Address";
                    effect = "(TA) <- A";
                    break;

                case 0x54: //   STCH 
                    result = "STCH";
                    break;

                case 0x14: //   STL 
                    result = "STL";
                    details = "Store Value in Register L to Target Address";
                    effect = "(TA) <- L";
                    break;

                case 0x10: //   STX         (Stores contents of X in Target Address)
                    result = "STX";
                    details = "Store Value in Register X to Target Address";
                    effect = "(TA) <- X";
                    break;

                case 0x1C: // SUB
                    result = "SUB";
                    details = "Sub Value in Target Address to Register A";
                    effect = "A <- (A) - (TA)";
                    break;

                case 0xE0: //   TD          (Tests to see if a device is busy).
                    result = "TD";
                    details = "Test Device Number Specified in Target Address";
                    effect = "Set SW";
                    break;

                case 0x2C: //   TIX 
                    result = "TIX";
                    details = "Increment value in X Register. Compare to value in Target Address";
                    effect = "X <- X + 1; COMP X to M set CC";
                    break;

                case 0xDC: //   WD          (Write to Device)
                    result = "WD";
                    details = "Write rightmost byte in A to Device Number in Target Address";
                    effect = " Device(TA) <- A[rightmost byte]";
                    break;

                default:
                    result = string.Empty;
                    break;
            }

            result += " ";

            if (indexed == true)
            {
                result += "TA = TA + X ->" + targetAddress.ToString(("X6")) + '+' + X.ToString(("X6")) + "->" + (targetAddress + targetAddress + X).ToString(("X6"));
            }
            else
            {
                result += "TA = " + targetAddress.ToString("X6");
            }

            result = result + "|" + details + "|" + effect;
            return result;
        }

        /// <summary>
        /// Executes Single Operation Code using Target Address as Operand.
        /// Method is the "microcode" steps in the SIC CPU
        /// to execute mnemonics
        /// </summary>
        /// <param name="opCode">Opcode for Instruction to Execute</param>
        /// <param name="targetAddress">Calculated Target Address</param>
        public void ExecuteInstruction(int opCode, int targetAddress)
        {

            switch ((OPCode)opCode)
            {
                case OPCode.ADD: //   ADD
                    MicroSteps.AppendLine("-----ADD------");
                    MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " + " + FetchWord(targetAddress).ToString("X6"));
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    A += FetchWord(targetAddress);
                    PC += 3;
                    break;

                case OPCode.AND: //   AND
                    MicroSteps.AppendLine("-----AND------");
                    MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " && " + FetchWord(targetAddress).ToString("X6"));
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    A &= FetchWord(targetAddress);
                    PC += 3;
                    break;

                case OPCode.COMP:  // CMP   (Compare and set Status Word SW)
                    MicroSteps.AppendLine("-----CMP------");
                    int data = FetchWord(targetAddress);

                    if (A < data)
                    {
                        SW = SW | 0x40;
                        SW = SW & 0xFFFF7F;
                        MicroSteps.AppendLine("CC <- 01");
                    }
                    else if (A == data)
                    {
                        SW = SW & 0xFFFF3F;
                        MicroSteps.AppendLine("CC <- 00");
                    }
                    else
                    {
                        SW = SW | 0x80;
                        SW = SW & 0xFFFFBF;
                        MicroSteps.AppendLine("CC <- 10");
                    }
                    // Condition Code Values
                    // CC = 00 -> Equal
                    // CC = 01 -> Less than
                    // CC = 10 -> Greater than
                    // CC = 11 -> Not used
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.DIV: // DIV 
                    MicroSteps.AppendLine("-----DIV------");
                    // We don't want to crash the VM.
                    // IF we divide by zero, we should do something in the VM.

                    if (FetchWord(targetAddress) == 0)
                    {
                        // NO exception. We should set status WORD and NOT DO THE DIV
                    }
                    else
                    {
                        MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " / " + FetchWord(targetAddress).ToString("X6"));
                        A /= FetchWord(targetAddress);
                    }

                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.J: //   J 
                    MicroSteps.AppendLine("-----J------");
                    MicroSteps.AppendLine("PC <- " + targetAddress.ToString("X6"));
                    PC = targetAddress;
                    break;

                case OPCode.JEQ: //   JEQ 
                    if ((SW & 0xC0) == 0)
                    {
                        MicroSteps.AppendLine("PC <- " + targetAddress.ToString("X6"));
                        PC = targetAddress;
                    }
                    else
                    {
                        MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                        PC += 3;
                    }
                    break;

                case OPCode.JGT: //    JGT
                    MicroSteps.AppendLine("-----JGT------");
                    int TempJGT;
                    TempJGT = (SW & 0xC0) >> 6;
                    if (TempJGT == 2)
                    {
                        MicroSteps.AppendLine("PC <- " + targetAddress.ToString("X6"));
                        PC = targetAddress;
                    }
                    else
                    {
                        MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                        PC += 3;
                    }
                    break;

                case OPCode.JLT: //   JLT 
                    MicroSteps.AppendLine("-----JLT------");
                    int TempJLT;
                    TempJLT = (SW & 0xC0) >> 6;
                    if (TempJLT == 1)
                    {
                        MicroSteps.AppendLine("PC <- " + targetAddress.ToString("X6"));
                        PC = targetAddress;
                    }
                    else
                    {
                        MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                        PC += 3;
                    }
                    break;

                case OPCode.JSUB: // JSUB      (Jump to subroutine starting at TA. Preserve PC by storing in L)
                    MicroSteps.AppendLine("-----JSUB------");
                    MicroSteps.AppendLine("L  <- " + PC.ToString("X6") + " + 3");
                    L = (PC + 3);
                    MicroSteps.AppendLine("PC <- " + targetAddress.ToString("X6"));
                    PC = targetAddress;
                    break;

                case OPCode.LDA: // LDA 
                    MicroSteps.AppendLine("-----LDA------");
                    MicroSteps.AppendLine("A  <- " + FetchWord(targetAddress).ToString("X6"));
                    A = FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.LDCH: //  LDCH
                    MicroSteps.AppendLine("-----LDCH------");
                    byte ByteLoad;
                    ByteLoad = (byte)FetchByte(targetAddress);

                    //TODO -> Wire in character reads from device objects
                    A = ByteLoad;
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.LDL: //  LDL 
                    MicroSteps.AppendLine("-----LDL------");
                    MicroSteps.AppendLine("L  <- " + FetchWord(targetAddress).ToString("X6"));
                    L = FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.LDX: //  LDX 
                    MicroSteps.AppendLine("-----LDX------");
                    MicroSteps.AppendLine("X  <- " + FetchWord(targetAddress).ToString("X6"));
                    X = FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.MUL:  // MUL 
                    MicroSteps.AppendLine("-----MUL------");
                    MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " * " + FetchWord(targetAddress).ToString("X6"));
                    A *= FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.OR: //   OR 
                    MicroSteps.AppendLine("-----OR------");
                    MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " OR " + FetchWord(targetAddress).ToString("X6"));
                    A |= FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.RD: // RD 
                    MicroSteps.AppendLine("-----RD------");
                    byte dataByte;
                    dataByte = (byte)A;
                    int DeviceNumberToRead;
                    DeviceNumberToRead = FetchWord(targetAddress);

                    // Set Device's Status Word to BUSY
                    Devices[DeviceNumberToRead].DeviceSW &= 0xFFFF3F;

                    // Write the byte to the device
                    dataByte = Devices[DeviceNumberToRead].ReadByte();

                    // Set Device's Status Word to AVAILABLE
                    Devices[DeviceNumberToRead].DeviceSW |= 0x40;
                    Devices[DeviceNumberToRead].DeviceSW &= 0xFFFF7F;
                    int tmp;
                    tmp = dataByte;
                    tmp &= 0xFF;
                    A = A & 0xFFFF00;

                    A |= tmp;
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.RSUB: //    RSUB
                    MicroSteps.AppendLine("-----RSUB------");
                    if (L == 0)
                    {
                        MicroSteps.AppendLine("PC <- (-1) PROGRAM HALTED");
                        PC = -1;   // Program Halted.
                    }
                    else
                    {
                        MicroSteps.AppendLine("PC <- " + L.ToString("X6"));
                        PC = L;
                    }
                    break;

                case OPCode.STA: //   STA         (Stores contents of A in Target Address)
                    MicroSteps.AppendLine("-----STA------");
                    MicroSteps.AppendLine("TA <- " + A.ToString("X6") + " : TA = " + targetAddress.ToString("X6"));
                    StoreWord(targetAddress, A);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.STCH: //   STCH 
                    MicroSteps.AppendLine("-----STCH------");
                    int tempChar;
                    char dataSTCHByte;
                    tempChar = A & 0xFF;
                    dataSTCHByte = (char)tempChar;
                    StoreByte(targetAddress, (byte)dataSTCHByte);
                    MicroSteps.AppendLine("TA <- " + tempChar.ToString("X6") + " : TA = " + targetAddress.ToString("X6"));

                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.STL: //   STL 
                    MicroSteps.AppendLine("-----STL------");
                    MicroSteps.AppendLine("TA <- " + L.ToString("X6") + " : TA = " + targetAddress.ToString("X6"));
                    StoreWord(targetAddress, L);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.STX: //   STX         (Stores contents of X in Target Address)
                    MicroSteps.AppendLine("-----STX------");
                    MicroSteps.AppendLine("TA <- " + X.ToString("X6") + " : TA = " + targetAddress.ToString("X6"));
                    StoreWord(targetAddress, X);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.SUB: // SUB           (Subtract Value in TA from A )
                    MicroSteps.AppendLine("-----SUB------");
                    MicroSteps.AppendLine("A  <- " + A.ToString("X6") + " - " + FetchWord(targetAddress).ToString("X6"));
                    A -= FetchWord(targetAddress);
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.TD: //   TD          (Tests to see if a device is busy).
                    MicroSteps.AppendLine("-----TD------");
                    SW = SW | 0x40;
                    SW = SW & 0xFFFF7F; //CC is <
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.TIX: //   TIX 
                    MicroSteps.AppendLine("-----TIX------");
                    int DataW;
                    int tempTIX;
                    DataW = FetchWord(targetAddress);
                    MicroSteps.AppendLine("X <- " + X.ToString("X6") + " + 1");

                    tempTIX = ++X - DataW;
                    if (tempTIX < 0)
                    {
                        SW = SW | 0x40;
                        SW = SW & 0xFFFF7F;
                    }
                    else if (tempTIX == 0)
                    {
                        SW = SW & 0xFFFF3F;
                    }
                    else
                    {
                        SW = SW | 0x80;
                        SW = SW & 0xFFFFBF;
                    }
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;

                case OPCode.WD: //   WD          (Write to Device)
                    MicroSteps.AppendLine("-----WD------");
                    /*** WD ***/

                    byte dataByteW;
                    dataByteW = (byte)A;
                    int DeviceNumberToWriteTo;
                    DeviceNumberToWriteTo = FetchWord(targetAddress);

                    // Set Device's Status Word to BUSY
                    Devices[DeviceNumberToWriteTo].DeviceSW &= 0xFFFF3F;

                    // Write the byte to the device
                    Devices[DeviceNumberToWriteTo].WriteByte(dataByteW);

                    // Set Device's Status Word to AVAILABLE
                    Devices[DeviceNumberToWriteTo].DeviceSW |= 0x40;
                    Devices[DeviceNumberToWriteTo].DeviceSW &= 0xFFFF7F;
                    MicroSteps.AppendLine("Device" + DeviceNumberToWriteTo.ToString() + " <- " + dataByteW.ToString("X6"));
                    MicroSteps.AppendLine("PC <- " + PC.ToString("X6") + " + 3");
                    PC += 3;
                    break;
            }
        }

        public void LoadToMemory(string line, int startAddress, int length)
        {
            int i = 9, num = 0, BytesRead = 0, index = startAddress;
            while (BytesRead++ < length)
            {
                char ch = line[i++];
                if (ch >= 'A')
                {
                    ch -= (char)7;
                }

                ch -= (char)48;
                num = ch << 4;

                ch = line[i++];
                if (ch >= 'A')
                {
                    ch -= (char)7;
                }

                ch -= (char)48;
                num += ch;

                StoreByte(index++, (byte)num);
            }
        }

        public void DecodeInstruction(int fullInstruction, ref int opCode, ref int targetAddress)
        {
            int XBit = (fullInstruction & 0x8000);
            bool INDEXED = (XBit > 0);

            targetAddress = fullInstruction & 0x7FFF;
            opCode = fullInstruction & 0xFF0000;
            opCode = opCode >> 16;

            if (INDEXED == true)
            {
                targetAddress += X;   // Add contents of X register to address for indexed Mode
            }
        }
    }
}
