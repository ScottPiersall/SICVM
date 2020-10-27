using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Reflection.Emit;

namespace SIC_Simulator
{
       
    [Serializable()]
    class SIC_CPU

    {
        public int PC;
        public int A;
        public int X;
        public int L;
        public int SW;

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






    }


}
