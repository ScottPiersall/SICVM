using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

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

    }

    class Device
    {
        public int DeviceID;






    }


}
