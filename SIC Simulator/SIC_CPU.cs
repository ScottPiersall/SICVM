using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Reflection.Emit;
using System.Drawing.Imaging;

namespace SIC_Simulator
{
       
    [Serializable()]
    class SIC_CPU

    {
        public int PC = 0;
        public int A = 0;
        public int X =0;
        public int L =0;
        public int SW =0;

        public byte[] MemoryBytes;

        /// <summary>
        /// Constructs a SIC VM (CPU and Memory)
        /// </summary>
        /// <param name="ZeroizeBytes"></param>
        public SIC_CPU( bool ZeroizeBytes)
        {
            PC = A = X = L =  SW = 0;
            MemoryBytes = new byte[32768];

            if ( ZeroizeBytes == true)
            {
                this.ZeroizeMemory();

            } else
            {
                this.RandomizeMemory();
            }
        }



        /// <summary>
        /// Randomize all memory bytes in the SIC
        /// Simulates filling memory with "JUNK"
        /// Way to see if programs specifically set memory when needed
        /// and do not rely on "JUNK" values
        /// </summary>
        public void RandomizeMemory()
        {
            Random rnd = new Random( Guid.NewGuid().GetHashCode() );
            rnd.NextBytes(this.MemoryBytes);
        }

        /// <summary>
        /// Sets all memory bits to ZERO in the SIC
        /// </summary>
        public void ZeroizeMemory()
        {
            byte zero;
            zero = 0;
            for ( int x = 0; x < 32768; x++) { this.MemoryBytes[x] = zero; }
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

    }



        /// <summary>
        /// Fetches a 24-bit word from Memory and places it into an integer
        /// </summary>
        /// <param name="Address">Address of Memory Location in SIC [0..32767]</param>
        /// <returns></returns>
        public int FetchWord( int Address)
        {
            int num1 = 0;
            int num2 = 0;
            for (int i = 0; i < 3; ++i) {
                char ch = (char)this.MemoryBytes[Address++];
                num2 = (int)ch;
                num2 = num2 & 0x000000FF;
                num1 = num1 | num2;
                num1 = num1 << 8;
            }


            return num1;

        }

        /// <summary>
        /// Fetches Byte in Memory and returns character type
        /// </summary>
        /// <param name="Address"></param>
        /// <returns></returns>
        public char FetchByte( int Address)
        {
            return (char)this.MemoryBytes[Address];
        }


        public void StoreWord( int Address, int data)
        {
            byte a,b,c;

            a = (byte)((data >> 8)&0xFF);
            b = (byte)((data >> 8) &0xFF);
            c = (byte)((data >> 8) &0xFF);
            this.MemoryBytes[Address + 2] = a;
            this.MemoryBytes[Address + 1] = b;
            this.MemoryBytes[Address ] = c;
        }


        public void StoreByte( int address, byte data)
        {
            this.MemoryBytes[address] = data;
        }


        /// <summary>
        /// Load's an Object File Representation from a File
        /// into this SIC VM's Memory
        /// NO RELOCATION IS PERFORMACE (ABSOLUTE LOADING)
        /// </summary>
        /// <param name="S"></param>
        public void LoadObjectFile( String AbsoluteFilePath)
        {
            

        }


        private void ReadHeaderRecord( System.IO.Stream s)
        {

        }

        private void ReadTextRecord( System.IO.Stream s)
        {

        }


        public void InitializePC( int PCValue)
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

        }

        /// <summary>
        /// Executes Single Operation Code using Target Address as Operand.
        /// This Method is the "microcode" steps in the SIC CPU
        /// to execute mnemonics
        /// </summary>
        /// <param name="OpCode">Opcode for Instruction to Execute</param>
        /// <param name="TA">Calculated Target Address</param>
        public void ExecuteInstruction( int OpCode, int TA)
        {
            
           switch (OpCode)
            {
                case 0x18: //   ADD
                    this.A += this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x40: //   AND
                    this.A &= this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x28:  // CMP   (Compare and set Status Word SW)
                    int Data;
                    int Difference;
                    Data = this.FetchWord(TA);
                    Difference = A - Data;     // Perform subtraction and set SW
                    if (Difference < 0)
                    {
                        this.SW = this.SW | 0x40;
                        this.SW = this.SW & 0xFFFF7F;
                    }
                    else if (Difference == 0)
                    {
                        this.SW = this.SW & 0xFFFF3F;
                    }
                    else
                    {
                        this.SW = this.SW | 0x80;
                        this.SW = this.SW & 0xFFFFBF;
                    }
                    // Condition Code Values
                    // CC = 00 -> Equal
                    // CC = 01 -> Less than
                    // CC = 10 -> Greater than
                    // CC = 11 -> Not used
                    this.PC += 3;
                    break;

                case 0x24: // DIV 
                    // We don't want to crash the VM.
                    // IF we divide by zero, we should do something in the VM.

                    if (this.FetchWord(TA) == 0)
                    {
                    // NO exception. We should set status WORD and NOT DO THE DIV
                    }

                    else
                    {
                    this.A /= this.FetchWord(TA);
                    }
                    this.PC += 3;
                    break;



                case 0x3C: //   J 
                    this.PC = TA;
                    break;

                case 0x30: //   JEQ 
                    if ((SW & 0xC0) != 0)
                    {
                        PC = TA;
                    }
                    else { this.PC += 3; }
                    break;

                case 0x34: //   JGT 
                    int TempJGT;
                    TempJGT = (SW & 0xC0) >> 6;
                    if (TempJGT == 2)
                    {
                        this.PC = TA;
                    }
                    else { this.PC += 3; }
                    break;

                case 0x38: //   JLT 
                    int TempJLT;
                    TempJLT = (SW & 0xC0) >> 6;
                    if (TempJLT == 1)
                    {
                        this.PC = TA;
                    } else { this.PC += 3; }
                    break;



                case 0x48: // JSUB      (Jump to subroutine starting at TA. Preserve PC by storing in L)
                    this.L = this.PC;
                    this.PC = TA;
                    break;

                case 0x00: // LDA 
                    this.A = this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x50: //  LDCH
                    byte ByteLoad;
                    ByteLoad = (byte)FetchByte(TA);

                //TODO -> Wire in character reads from device objects
                
                    this.PC += 3;
                    break;


                case 0x08: //  LDL 
                    this.L = this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x04: //  LDX 
                    this.X = this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x20:  // MUL 
                    this.A *= FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x44: //   OR 
                    this.A |= this.FetchWord(TA);
                    this.PC += 3;
                    break;

                case 0x4C: //    RSUB
                    if (this.L == 0)
                    {
                        this.PC = -1;   // Program Halted.
                    }
                    else
                    {
                        this.PC = this.L;
                    }
                    break;

                case 0x0C: //   STA         (Stores contents of A in Target Address)
                    this.StoreWord(TA, A);
                    this.PC += 3;    
                    break;

                case 0x54: //   STCH 
                    int tempChar;
                    char dataSTCHByte;
                    tempChar = this.A & 0xFF;
                    dataSTCHByte = (char)tempChar;
                    this.StoreByte(TA, (byte)dataSTCHByte);
                    this.PC += 3;
                    break;

                case 0x14: //   STL 
                    this.StoreWord(TA, L);
                    this.PC += 3;
                    break;



                case 0x10: //   STX         (Stores contents of X in Target Address)
                    this.StoreWord(TA, X);
                    this.PC += 3;
                    break;

                case 0xE0: //   TD          (Tests to see if a device is busy).
                    this.SW = this.SW | 0x40;
                    this.SW = this.SW & 0xFFFF7F; //CC is <
                    this.PC += 3;
                    break;

                case 0x2C: //   TIX 
                    int DataW;
                    int tempTIX;
                    DataW = this.FetchWord(TA);
                    tempTIX = ++this.X - DataW;
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
                    this.PC += 3;
                    break;

                case 0xDC: //   WD          (Write to Device)
                    /*** WD ***/
                    byte dataByte;
                    dataByte = (byte)this.FetchByte(TA);

                    break;

            }




        }

        public void DecodeInstruction( int FullInstruction, ref int OpCode, ref int TargetAddress)
        {
            bool INDEXED = false;
            int XBit = 0;
            XBit = (FullInstruction & 0x8000);
            INDEXED = (XBit > 0);
            
            TargetAddress = FullInstruction & 0x7FFF;
            OpCode = FullInstruction & 0xFF0000;
            OpCode = OpCode >> 16;
            if ( INDEXED == true )
            {
                TargetAddress += this.X;   // Add contents of X register to address for indexed Mode
            }
        }



    }

    class Device
    {
        public int DeviceID;


        public Device( int DeviceNumber )
        {
            this.DeviceID = DeviceNumber;
        }



    }


}
