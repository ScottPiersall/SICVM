using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace SIC_Simulator
{

    [Serializable()]
    class SIC_CPU : ISerializable

    {
        public readonly static int NumDevices = 65;
        public readonly static int DEVICE_ID_PADDING = 16; 
        public int CurrentProgramEndAddress = 0;
        public int CurrentProgramStartAddress = 0;
        public int PC = 0;
        public int A = 0;
        public int X = 0;
        public int L = 0;
        public int SW = 0;
        public Assembler assembler = null;

        public byte[] MemoryBytes;

        public SIC_Device[] Devices;


        public bool MachineStateIsNotSaved = false;

        private StringBuilder MicroSteps;

        public String MicrocodeSteps
        {
            get { return this.MicroSteps.ToString(); }
        }

        // need to review assembly source to discern between BYTE and WORD directives
        public void getSICSource(Assembler assembler) {
          this.assembler = assembler;
        }


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
                this.ZeroizeMemory();
            }
            else
            {
                this.RandomizeMemory();
            }

            this.Devices = new SIC_Device[NumDevices];

            for (int i = 0; i < NumDevices; i++)
            {
                this.Devices[i] = new SIC_Device(i);
            }
            this.MicroSteps = new StringBuilder();
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
            rnd.NextBytes(this.MemoryBytes);
            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Sets all memory bits to ZERO in the SIC
        /// </summary>
        public void ZeroizeMemory()
        {
            byte zero;
            zero = 0;
            for (int x = 0; x < 32768; x++) { this.MemoryBytes[x] = zero; }
            MachineStateIsNotSaved = true;
        }

        /// <summary>
        /// Performs a "hard" reset of the virtual machine.
        /// 1. All registers and SW are set to zero
        /// 2. All Memory is zeroed.
        /// </summary>
        public void ResetVM()
        {
            this.PC = 0;
            this.A = 0;
            this.X = 0;
            this.L = 0;
            this.SW = 0;
            this.ZeroizeMemory();
            this.MicroSteps = new StringBuilder();
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
                char ch = (char)this.MemoryBytes[Address++];
                num2 = (int)ch;
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
            return (char)this.MemoryBytes[Address];
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
            this.MemoryBytes[address] = data;
            MachineStateIsNotSaved = true;
        }


        //New Code Segment By Brandon And Nick

        /// <summary> Loader section Brandon Woodrum and Nick Konopko
        /// Load's an Object File Representation from a File
        /// into this SIC VM's Memory
        /// NO RELOCATION IS PERFORMACE (ABSOLUTE LOADING)
        /// </summary>
        /// <param name="S"></param>
        public void LoadObjectFile(String AbsoluteFilePath)
        {
            //function that only loads from TRecords no mod
            //int c = 0;
            String l;
            System.IO.StreamReader f = new System.IO.StreamReader(AbsoluteFilePath);

            while ((l = f.ReadLine()) != null)
            {
                if (l[0] == 'H')
                {
                    ReadHeaderRecord(l);
                }
                if (l[0] == 'T')
                {
                    ReadTextRecord(l);
                }
                if (l[0] == 'E')
                {
                    continue;
                }

            }


        }
        //class for MOD constructor that holds info for Modification
        class Mod
        {
            //data fields
            int address = 0;
            int half = 0;
            bool flag = false;
            Mod next = null;
            bool error = false;


            //if our head is which is a place holder is returned then nothing was found in search. this sets its boolean to true
            public void seterr()
            {
                this.error = true;
            }
            //tells if its head or not
            public bool geterr()
            {
                return this.error;
            }

            //sets all values for created mod
            public void set(int address, int half, bool flag)
            {
                this.address = address;
                this.half = half;
                this.flag = flag;
                this.next = null;
            }
            //sets and gets for data fields
            public void setNext(Mod t)
            {
                this.next = t;
            }
            public Mod getNext()
            {
                return this.next;
            }

            public int getaddress()
            {
                return this.address;
            }
            public int gethalf()
            {
                return this.half;
            }
            public bool getbool()
            {
                return this.flag;
            }
            //searches linked list for Mod record matching T-record starting address, if head is returned as place holder nothing was found
            public Mod search(Mod head, int add)
            {
                Mod error = new Mod();
                error.seterr();
                while (head.next != null)
                {
                    if (add != head.address)
                    {
                        head = head.next;
                    }
                    else
                    {
                        return head;
                    }
                }
                return error;

            }
        }
        //relocation function for MOD records
        private void NotAbs(int x, String AbsoluteFilePath)
        {
            Mod head = new Mod();

            Mod last = head;

            int c = 0;
            String l;
            System.IO.StreamReader f = new System.IO.StreamReader(AbsoluteFilePath);
            int y = 0;
            //reads file to create MOD record objects
            while ((l = f.ReadLine()) != null)

            {
                if (l[0] == 'H')
                {
                    y = unr(l, x);
                }
                if (l[0] == 'M')
                {
                    Mod current = new Mod();
                    last.setNext(current);
                    last = current;
                    ReadModRecordR(l, x, y, current);
                }

            }
            String l2;
            System.IO.StreamReader f2 = new System.IO.StreamReader(AbsoluteFilePath);
            //reads file to generate memory, passing mod head to ReadTEXTR so that it can search for appropriate MOD
            while ((l2 = f2.ReadLine()) != null)
            {
                if (l2[0] == 'H')
                {

                    ReadHeaderRecordR(l2, x);
                }
                if (l2[0] == 'T')
                {
                    ReadTextRecordR(l2, x, head);
                }
                if (l2[0] == 'E')
                {
                    continue;
                }

            }
        }
        //parses mod record and returns correct offset minus or plus for trecord.
        private void ReadModRecordR(String T, int x, int y, Mod current)
        {  //may not need Flag anymore
            //alternate solution to plus or minus
            bool flag = false;
            if (x >= y)
            {
                flag = true;

            }
            else if (x < y)
            {
                flag = false;
            }

            ///end block

            String address = T.Substring(2, 6);
            String h = T.Substring(8, 2);
            int a = System.Int32.Parse(address);
            int l = System.Int32.Parse(h);

            //start block
            //if true then new start position was larger than old so plus difference.
            if (flag == true)
            {
                a += (x - y);
            }
            else
            {
                a -= (x - y);
            }

            //end block
            /*   String OPER = T.Substring(10, 1);
               if (OPER.Equals("+"))
                   a += y;
               else
               {
                   a -= y;
               }*/
            if (a > 8000 || a < 0)
            {
                //error for out of bounds new position.
                return;
            }
            else
            {// sets mods data fields
                current.set(a, l, flag);
            }

        }

        //ABS
        //reads header record for no mod
        private void ReadHeaderRecord(String T)
        {
            //gets starting address, but why?

            string address = T.Substring(8, 6);
        }
        //reads and loads text into memory for nomod
        private void ReadTextRecord(String T)
        {
            //String T;
            // T = s.ToString();
            string address = T.Substring(2, 6);
            string Length = T.Substring(8, 2);
            int a = System.Int32.Parse(address);
            int l = System.Int32.Parse(Length);
            LoadToMemory(T, a, l);
        }
        //NOT ABS
        //reads and loads text into memory with mod as needed.
        private void ReadTextRecordR(String T, int x, Mod head)
        {
            //String T;
            // T = s.ToString();
            string address = T.Substring(2, 6);
            string Length = T.Substring(8, 2);
            int a = System.Int32.Parse(address);
            Mod test = new Mod();
            test = head.search(head, a);
            bool MODnofound = test.geterr();
            //int u = 0;
            //if MOD record found gets new address for Textrecord then loads into memory.
            if (MODnofound != true)
            {
                //u = test.gethalf();
                a = test.getaddress();
                //a+=x;
                //a+= u;
            }

            int l = System.Int32.Parse(Length);
            LoadToMemory(T, a, l);
        }

        //read header record, but I think its obsolete. implememented function that does this ones job. to be deleted.
        private void ReadHeaderRecordR(String T, int x)
        {
            //gets starting address, but why?

            string address = T.Substring(8, 6);
            int a = System.Int32.Parse(address);
            string length = T.Substring(14, 6);
            int b = System.Int32.Parse(length);
            if (a + x + b > 8000)
            { //over memory for relocation.
                return;
            }
            else
                a += x;
        }
        //does headerrecordreaders job from above, math is handled on return and send to Mod reader.
        private int unr(String T, int x)
        {
            string address = T.Substring(8, 6);
            int a = System.Int32.Parse(address);
            int NewPCCounter = 0;
            bool flag = false;
            if (x >= a)
            {
                flag = true;

            }
            else if (x < a)
            {
                flag = false;
            }
            if (flag == true)
            {
                NewPCCounter += (a - x);
            }
            else
            {
                NewPCCounter -= (a - x);
            }
            //hopefully this updates PC counter so that Restart function works, hopefully.*************************************//important commment i might be wrong
            if (NewPCCounter > 8000 || NewPCCounter < 0)
            {
                //error for out of bounds new pc counter.
                return -1;
            }
            else
            {
                this.PC = NewPCCounter;
            }
            //returns header record start address.
            return a;
        }

        //End of Loader Block////////////////

        //here

        public void InitializePC(int PCValue)
        {
            this.PC = PCValue;
        }

        /// <summary>
        /// This method steps the CPU one time. 
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

            NextInstruction = this.FetchWord(PC);

            this.DecodeInstruction(NextInstruction, ref op, ref TA);

            this.ExecuteInstruction(op, TA);
            MachineStateIsNotSaved = true;
        }





        /// <summary>
        /// Returns a human-readable  description of the instruction and operand value located
        /// at Address. The string has two parts, delimited by a |
        /// mnemonic target address | complete description of instruction and result
        /// </summary>
        /// <param name="Address">Absolute address of 3-byte instruction</param>
        /// <returns>String with description</returns>
        public String GetInstructionDescription(int Address)
        {
            string Result = string.Empty;
            string Details = string.Empty;
            string Effect = string.Empty;
            int Word;
            Word = this.FetchWord(Address); // Fetch the Word at Address
            int TargetAddress;
            int OpCode;
            bool INDEXED = false;
            int XBit = 0;
            XBit = (Word & 0x8000);
            INDEXED = (XBit > 0);

            TargetAddress = Word & 0x7FFF;
            OpCode = Word & 0xFF0000;
            OpCode = OpCode >> 16;


            switch (OpCode)
            {
                case 0x18: //   ADD
                    Result = "ADD";
                    Details = "Add Value in Target Address to Register A";
                    Effect = "A <- (A) + (TA)";
                    break;

                case 0x40: //   AND
                    Result = "AND";
                    Details = "Perform Bitwise AND on Value in Target Address and Register A, store result in A";
                    Effect = "A <- (A) && (TA)";
                    break;

                case 0x28:  // COMP   (Compare and set Status Word SW)
                    Result = "COMP";
                    Details = "Compare Register A to Value in Target Address";
                    Effect = "Set CC";
                    break;

                case 0x24: // DIV 
                    Result = "DIV";
                    Details = "Divide Register A by Value in Target Address";
                    Effect = "A <- (A) / (TA)";
                    break;

                case 0x3C: //   J 
                    Result = "J";
                    Details = "Perform Unconditional Jump to Target Address";
                    Effect = "PC <- (TA)";
                    break;

                case 0x30: //   JEQ 
                    Result = "JEQ";
                    Details = "Perform Conditional Jump to Target Address when CC = 00";
                    Effect = "PC <- (TA) if CC = 00";
                    break;

                case 0x34: //   JGT 
                    Result = "JGT";
                    Details = "Perform Conditional Jump to Target Address when CC = 10";
                    Effect = "PC <- (TA) if CC = 10";
                    break;

                case 0x38: //   JLT 
                    Result = "JLT";
                    Details = "Perform Conditional Jump to Target Address when CC = 01";
                    Effect = "PC <- (TA) if CC = 01";
                    break;

                case 0x48: // JSUB      (Jump to subroutine starting at TA. Preserve PC by storing in L)
                    Result = "JSUB";
                    Details = "Jump to Subroutine at Target Address. Preserve PC By Storing in L";
                    Effect = "L <- PC; PC <- (TA)";
                    break;

                case 0x00: // LDA 
                    Result = "LDA";
                    Details = "Load Value in Target Address to Register A";
                    Effect = "A <- (TA)";
                    break;

                case 0x50: //  LDCH
                    Result = "LDCH";
                    Details = "Load Character (Byte) in Target Address to Register A";
                    Effect = "A <- (TA)";
                    //This effect is NOT A[rightmost byte] <- (TA) since more than the rightmost byte is affected
                    break;

                case 0x08: //  LDL 
                    Result = "LDL";
                    Details = "Load Value in Target Address to Register L";
                    Effect = "L <- (TA)";
                    break;

                case 0x04: //  LDX 
                    Result = "LDX";
                    Details = "Load Value in Target Address to Register X";
                    Effect = "X <- (TA)";
                    break;

                case 0x20:  // MUL 
                    Result = "MUL";
                    Details = "Multiple Value in Target Address by Register A Store in A";
                    Effect = "A <- (A) * (TA)";
                    break;

                case 0x44: //   OR 
                    Result = "OR";
                    Details = "Perform Bitwise OR on Value in Target Address and Register A, Store in A";
                    Effect = "A <- (A) || (TA)";
                    break;

                case 0xD8: //   RD
                    Result = "RD";
                    Details = "Read Rightmost Byte in A from Device Number in Target Address";
                    Effect = "A[rightmost byte] <- Device(TA)";
                    break;

                case 0x4C: //   RSUB
                    Result = "RSUB";
                    Details = "Return from Subroutine";
                    Effect = "PC <- (L)";
                    break;

                case 0x0C: //   STA         (Stores contents of A in Target Address)
                    Result = "STA";
                    Details = "Store Value in Register A to Target Address";
                    Effect = "(TA) <- A";
                    break;

                case 0x54: //   STCH 
                    Result = "STCH";
                    Details = "Store Rightmost Byte in Register A to Target Address";
                    Effect = "(TA) <- A[rightmost byte]";
                    break;

                case 0x14: //   STL 
                    Result = "STL";
                    Details = "Store Value in Register L to Target Address";
                    Effect = "(TA) <- L";
                    break;

                case 0x10: //   STX         (Stores contents of X in Target Address)
                    Result = "STX";
                    Details = "Store Value in Register X to Target Address";
                    Effect = "(TA) <- X";
                    break;

                case 0x1C: // SUB
                    Result = "SUB";
                    Details = "Sub Value in Target Address to Register A";
                    Effect = "A <- (A) - (TA)";
                    break;

                case 0xE0: //   TD          (Tests to see if a device is busy).
                    Result = "TD";
                    Details = "Test Device Number Specified in Target Address";
                    Effect = "Set CC";
                    break;

                case 0x2C: //   TIX 
                    Result = "TIX";
                    Details = "Increment Value in X Register. Compare to Value in Target Address";
                    Effect = "X <- X + 1; COMP X to M set CC";
                    break;

                case 0xDC: //   WD          (Write to Device)
                    Result = "WD";
                    Details = "Write Rightmost Byte in A to Device Number in Target Address";
                    Effect = "Device(TA) <- A[rightmost byte]";
                    break;

                default:
                    Result = "";
                    break;
            }
            Result += " ";
            if (INDEXED == true)
            {
                Result += "TA = TA + X ->" + TargetAddress.ToString(("X6")) + '+' + X.ToString(("X6")) + "->" + (TargetAddress + TargetAddress + X).ToString(("X6"));
                TargetAddress += this.X;   // Add contents of X register to address for indexed Mode
            }
            else
            {
                Result += "TA = " + TargetAddress.ToString("X6");
            }
            Result = Result + "|" + Details + "|" + Effect;
            return Result;
        }

        /// <summary>
        /// Executes Single Operation Code using Target Address as Operand.
        /// This Method is the "microcode" steps in the SIC CPU
        /// to execute mnemonics
        /// </summary>
        /// <param name="OpCode">Opcode for Instruction to Execute</param>
        /// <param name="TA">Calculated Target Address</param>
        public void ExecuteInstruction(int OpCode, int TA)
        {

            switch (OpCode)
            {
                case 0x18: //   ADD
                    this.MicroSteps.AppendLine("-----ADD------");
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " + " + this.FetchWord(TA).ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.A += this.FetchWord(TA);
                    //force overflow/underflow with 3 bytes
                    this.A = (this.A << 8) >> 8;
                    this.PC += 3;
                    break;

                case 0x40: //   AND
                    this.MicroSteps.AppendLine("-----AND------");
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " && " + this.FetchWord(TA).ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.A &= this.FetchWord(TA);
                    //force overflow/underflow with 3 bytes
                    this.A = (this.A << 8) >> 8;
                    this.PC += 3;
                    break;

                case 0x28:  // CMP   (Compare and set Status Word SW)
                    this.MicroSteps.AppendLine("-----CMP------");
                    int Data;
                    Data = this.FetchWord(TA);

                    if (A < Data)
                    {
                        this.SW = this.SW | 0x40;
                        this.SW = this.SW & 0xFFFF7F;
                        this.MicroSteps.AppendLine("CC <- 01");
                    }
                    else if (A == Data)
                    {
                        this.SW = this.SW & 0xFFFF3F;
                        this.MicroSteps.AppendLine("CC <- 00");
                    }
                    else
                    {
                        this.SW = this.SW | 0x80;
                        this.SW = this.SW & 0xFFFFBF;
                        this.MicroSteps.AppendLine("CC <- 10");
                    }
                    // Condition Code Values
                    // CC = 00 -> Equal
                    // CC = 01 -> Less than
                    // CC = 10 -> Greater than
                    // CC = 11 -> Not used
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x24: // DIV 
                    this.MicroSteps.AppendLine("-----DIV------");
                    // We don't want to crash the VM.
                    // IF we divide by zero, we should do something in the VM.

                    if (this.FetchWord(TA) == 0)
                    {
                        // NO exception. We should set status WORD and NOT DO THE DIV
                    }

                    else
                    {
                        this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " / " + this.FetchWord(TA).ToString("X6"));
                        this.A /= this.FetchWord(TA);
                        //force overflow/underflow with 3 bytes
                        //division overflow/underflow will only occur when A=0x800000 and (TA)=-1
                        this.A = (this.A << 8) >> 8;
                    }
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x3C: //   J 
                    this.MicroSteps.AppendLine("-----J------");
                    this.MicroSteps.AppendLine("PC <- " + TA.ToString("X6"));
                    this.PC = TA;
                    break;

                case 0x30: //   JEQ 
                    //MessageBox.Show(SW.ToString() + " & " + 0xC0 + " = " + (SW&0xC0));    # Done to compare values
                    if ((SW & 0xC0) == 0)
                    {
                        this.MicroSteps.AppendLine("PC <- " + TA.ToString("X6"));
                        PC = TA;
                    }
                    else
                    {
                        this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                        this.PC += 3;
                    }
                    break;

                case 0x34: //    JGT
                    this.MicroSteps.AppendLine("-----JGT------");
                    int TempJGT;
                    TempJGT = (SW & 0xC0) >> 6;
                    if (TempJGT == 2)
                    {
                        this.MicroSteps.AppendLine("PC <- " + TA.ToString("X6"));
                        this.PC = TA;
                    }
                    else
                    {
                        this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                        this.PC += 3;
                    }
                    break;

                case 0x38: //   JLT 
                    this.MicroSteps.AppendLine("-----JLT------");
                    int TempJLT;
                    TempJLT = (SW & 0xC0) >> 6;
                    if (TempJLT == 1)
                    {
                        this.MicroSteps.AppendLine("PC <- " + TA.ToString("X6"));
                        this.PC = TA;
                    }
                    else
                    {
                        this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                        this.PC += 3;
                    }
                    break;

                case 0x48: // JSUB      (Jump to subroutine starting at TA. Preserve PC by storing in L)
                    this.MicroSteps.AppendLine("-----JSUB------");
                    this.MicroSteps.AppendLine("L  <- " + PC.ToString("X6") + " + 3");
                    this.L = (this.PC + 3);
                    this.MicroSteps.AppendLine("PC <- " + TA.ToString("X6"));
                    this.PC = TA;
                    break;

                case 0x00: // LDA 
                    this.MicroSteps.AppendLine("-----LDA------");
                    this.A = this.FetchWord(TA);
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x50: //  LDCH
                    this.MicroSteps.AppendLine("-----LDCH------");
                    this.A = this.FetchByte(TA);
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x08: //  LDL 
                    this.MicroSteps.AppendLine("-----LDL------");
                    this.L = this.FetchWord(TA);
                    this.MicroSteps.AppendLine("L  <- " + this.L.ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x04: //  LDX 
                    this.MicroSteps.AppendLine("-----LDX------");
                    this.X = this.FetchWord(TA);
                    this.MicroSteps.AppendLine("X  <- " + this.X.ToString("X6"));
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x20:  // MUL 
                    this.MicroSteps.AppendLine("-----MUL------");
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " * " + this.FetchWord(TA).ToString("X6"));
                    this.A *= FetchWord(TA);
                    //force overflow/underflow with 3 bytes
                    this.A = (this.A << 8) >> 8;
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x44: //   OR 
                    this.MicroSteps.AppendLine("-----OR------");
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " OR " + this.FetchWord(TA).ToString("X6"));
                    this.A |= this.FetchWord(TA);
                    //force overflow/underflow with 3 bytes
                    this.A = (this.A << 8) >> 8;
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0xD8: // RD 
                    this.MicroSteps.AppendLine("-----RD------");
                    byte dataByte;
                    dataByte = (byte)this.A;
                    int DeviceNumberToRead;
                    DeviceNumberToRead = this.FetchWord(TA);

                    /* We do this check in case a device ID is stored using a BYTE constant that is greater than zero.
                       Valid IDs require at most a byte of storage. Thus, for an ID stored in a WORD, the 16 
                       most significant bits will be all zeros, i.e. the DEVICE_ID_PADDING = 16. Because pass 2 validates 
                       that a device ID complies with the specified bounds (0-64), the eight most significant bits will 
                       only have nonzero values when the device ID is stored using the BYTE directive*/
                    if((DeviceNumberToRead >> DEVICE_ID_PADDING) > 0)
                      DeviceNumberToRead >>= DEVICE_ID_PADDING;

                    /* In case device ID is stored using a BYTE constant that is equal to zero. We need this additional check
                       because we can only safely ignore the 16 least significant bits of a device ID when the 8 most significant
                       bits are a nonzero value. Since we cannot make any assumptions about how the device ID is stored in this case,
                       we explicitly check the program source to determine if the device ID was stored using BYTE or WORD */
                    else if((DeviceNumberToRead >> DEVICE_ID_PADDING) == 0) {
                      List<Instruction> InstructionList = assembler.InstructionList;
                      for(int i=0; i<InstructionList.Count; i++) {
                        Instruction instruction = InstructionList[i];
                        if(TA == instruction.MemoryAddress) {
                          if(instruction.OpCode.Equals("BYTE") || instruction.OpCode.Equals("WORD")) {
                            if(instruction.OpCode.Equals("BYTE")) {
                              DeviceNumberToRead >>= DEVICE_ID_PADDING;
                            }
                            break;
                          }
                          else
                            continue;
                        }
                      }
                    }

                    // Set Device's Status Word to BUSY
                   // this.Devices[DeviceNumberToRead].DeviceSW &= 0xFFFF3F;

                    // Write the byte to the device
                    dataByte = this.Devices[DeviceNumberToRead].ReadByte();

                    // Set Device's Status Word to AVAILABLE
                //    this.Devices[DeviceNumberToRead].DeviceSW |= 0x40;
                //    this.Devices[DeviceNumberToRead].DeviceSW &= 0xFFFF7F;
          //          int tmp;
            //        tmp = (int)dataByte;
              //      tmp &= 0xFF;
                //    this.A = this.A & 0xFFFF00;
                    if (this.Devices[DeviceNumberToRead].status == 2)
                    {
                        // Pop byte from the device (method will handle device status)
                        dataByte = this.Devices[DeviceNumberToRead].ReadByte();
                        int tmp;
                        tmp = (int)dataByte;
                        tmp &= 0xFF;
                        this.A = this.A & 0xFFFF00;
                        this.A |= tmp;

                    }
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x4C: //    RSUB
                    this.MicroSteps.AppendLine("-----RSUB------");
                    if (this.L == 0)
                    {
                        this.MicroSteps.AppendLine("PC <- (-1) PROGRAM HALTED");
                        this.PC = -1;   // Program Halted.
                    }
                    else
                    {
                        this.MicroSteps.AppendLine("PC <- " + this.L.ToString("X6"));
                        this.PC = this.L;
                    }
                    break;

                case 0x0C: //   STA         (Stores contents of A in Target Address)
                    this.MicroSteps.AppendLine("-----STA------");
                    this.MicroSteps.AppendLine("TA <- " + A.ToString("X6") + " : TA = " + TA.ToString("X6"));
                    this.StoreWord(TA, A);
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x54: //   STCH 
                    this.MicroSteps.AppendLine("-----STCH------");
                    int tempChar;
                    char dataSTCHByte;
                    tempChar = this.A & 0xFF;
                    dataSTCHByte = (char)tempChar;
                    this.StoreByte(TA, (byte)dataSTCHByte);
                    this.MicroSteps.AppendLine("TA <- " + tempChar.ToString("X6") + " : TA = " + TA.ToString("X6"));

                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x14: //   STL 
                    this.MicroSteps.AppendLine("-----STL------");
                    this.MicroSteps.AppendLine("TA <- " + L.ToString("X6") + " : TA = " + TA.ToString("X6"));
                    this.StoreWord(TA, L);
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x10: //   STX         (Stores contents of X in Target Address)
                    this.MicroSteps.AppendLine("-----STX------");
                    this.MicroSteps.AppendLine("TA <- " + X.ToString("X6") + " : TA = " + TA.ToString("X6"));
                    this.StoreWord(TA, X);
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x1C: // SUB           (Subtract Value in TA from A )
                    this.MicroSteps.AppendLine("-----SUB------");
                    this.MicroSteps.AppendLine("A  <- " + this.A.ToString("X6") + " - " + this.FetchWord(TA).ToString("X6"));
                    this.A -= this.FetchWord(TA);
                    //force overflow/underflow with 3 bytes
                    this.A = (this.A << 8) >> 8;
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0xE0: //   TD          (Tests to see if a device is busy).
                    this.MicroSteps.AppendLine("-----TD------");

                    int DeviceNumberToWriteTo;
                    DeviceNumberToWriteTo = this.FetchWord(TA);

                    //write status to CC
                    this.SW = this.SW & 0xFFFF3F | (this.Devices[DeviceNumberToWriteTo].status << 6);
                    String statusBinary = "0" + Convert.ToString(this.Devices[DeviceNumberToWriteTo].status, 2);
                    statusBinary = statusBinary.Substring(statusBinary.Length - 2);
                    this.MicroSteps.AppendLine("CC <- " + statusBinary);
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0x2C: //   TIX 
                    this.MicroSteps.AppendLine("-----TIX------");
                    int DataW;
                    int tempTIX;
                    DataW = this.FetchWord(TA);
                    this.MicroSteps.AppendLine("X <- " + X.ToString("X6") + " + 1");

                    ++this.X;
                    if (this.X >= 0x800000) //detect overflow
                    {
                        this.X -= 0x1000000; //force overflow
                    }

                    tempTIX = this.X - DataW;
                    if (tempTIX < 0)
                    {
                        this.SW = this.SW | 0x40;
                        this.SW = this.SW & 0xFFFF7F;
                    }
                    else if (tempTIX == 0)
                    {
                        this.SW = this.SW & 0xFFFF3F;
                    }
                    else
                    {
                        this.SW = this.SW | 0x80;
                        this.SW = this.SW & 0xFFFFBF;
                    }
                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

                case 0xDC: //   WD          (Write to Device)
                    this.MicroSteps.AppendLine("-----WD------");
                    /*** WD ***/

                    byte dataByteW;
                    dataByteW = (byte)this.A;
                    DeviceNumberToWriteTo = this.FetchWord(TA);
                    if (this.Devices[DeviceNumberToWriteTo].status > 0) //check if device is ready to write
                    {
                        // Write the byte to the device (will handle device status)
                        this.Devices[DeviceNumberToWriteTo].WriteByte(dataByteW);

                  /* We do this check in case a device ID is stored using a BYTE constant that is greater than zero.
                     Valid IDs require at most a byte of storage. Thus, for an ID stored in a WORD, the 16 
                     most significant bits will be all zeros, i.e. the DEVICE_ID_PADDING = 16. Because pass 2 validates 
                     that a device ID complies with the specified bounds (0-64), the eight most significant bits will 
                     only have nonzero values when the device ID is stored using the BYTE directive*/
                    if((DeviceNumberToWriteTo >> DEVICE_ID_PADDING) > 0)
                      DeviceNumberToWriteTo >>= DEVICE_ID_PADDING;

                  /* In case device ID is stored using a BYTE constant that is equal to zero. We need this additional check
                     because we can only safely ignore the 16 least significant bits of a device ID when the 8 most significant
                     bits are a nonzero value. Since we cannot make any assumptions about how the device ID is stored in this case,
                     we explicitly check the program source to determine if the device ID was stored using BYTE or WORD */
                    else if((DeviceNumberToWriteTo >> DEVICE_ID_PADDING) == 0) {
                      List<Instruction> InstructionList = assembler.InstructionList;
                      for(int i=0; i<InstructionList.Count; i++) {
                        Instruction instruction = InstructionList[i];
                        if(TA == instruction.MemoryAddress) {
                          if(instruction.OpCode.Equals("BYTE") || instruction.OpCode.Equals("WORD")) {
                            if(instruction.OpCode.Equals("BYTE")) {
                                DeviceNumberToWriteTo >>= DEVICE_ID_PADDING;
                            }
                            break;
                          }
                          else
                            continue;
                        }
                      }
                    }

                    // Set Device's Status Word to BUSY
                   // this.Devices[DeviceNumberToWriteTo].DeviceSW &= 0xFFFF3F;
                        this.MicroSteps.AppendLine("Device" + DeviceNumberToWriteTo.ToString() + " <- " + dataByteW.ToString("X6"));
                    }
                    // if the device is not ready to write, simply fail to write; no exceptions generated


                    this.MicroSteps.AppendLine("PC <- " + this.PC.ToString("X6") + " + 3");
                    this.PC += 3;
                    break;

            }




        }


        public void LoadToMemory(String line, int StartAddress, int Length)
        {
            int i = 9, num = 0, BytesRead = 0, index = StartAddress;
            while (BytesRead < Length)
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
                BytesRead++;
                StoreByte(index++, (byte)num);
            }

        }

        public void DecodeInstruction(int FullInstruction, ref int OpCode, ref int TargetAddress)
        {
            bool INDEXED = false;
            int XBit = 0;
            XBit = (FullInstruction & 0x8000);
            INDEXED = (XBit > 0);

            TargetAddress = FullInstruction & 0x7FFF;
            OpCode = FullInstruction & 0xFF0000;
            OpCode = OpCode >> 16;
            if (INDEXED == true)
            {
                TargetAddress += this.X;   // Add contents of X register to address for indexed Mode
            }
        }

        /// <summary>
        /// Set Alternate Values For A Serialized Object
        /// Stores And Formats In XML 
        /// </summary>
        /// <param name="info"> SerilizationInfo object used to customize serilization behavior</param>
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            
            info.AddValue("PC_Register",PC);
            info.AddValue("A_Register",A);
            info.AddValue("X_Register", X);
            info.AddValue("L_Register", L);
            info.AddValue("SW_Register", SW);
            info.AddValue("MemoryBytes", MemoryBytes);
            info.AddValue("MicroSteps", MicroSteps);
            for(int i = 0; i < NumDevices; i++)
            {    
                info.AddValue("DeviceString" + i,Devices[i].GetASCIIStringWrites());
            }
        }

        /// <summary>
        /// Constructor that is called during deserialization
        /// Reconstructs object from SerializationInfo info
        /// </summary>
        /// <param name="info">SerilizationInfo object used to customize serilization behavior</param>
        public SIC_CPU(SerializationInfo info, StreamingContext context)
        {
            PC = (int)info.GetValue("PC_Register", typeof(int));
            A = (int)info.GetValue("A_Register", typeof(int));
            X = (int)info.GetValue("X_Register", typeof(int));
            L = (int)info.GetValue("L_Register", typeof(int));
            SW = (int)info.GetValue("SW_Register", typeof(int));
            MemoryBytes = (byte[])info.GetValue("MemoryBytes", typeof(byte[]));
            MicroSteps = (StringBuilder)info.GetValue("MicroSteps", typeof(StringBuilder));
        }

    }


}
