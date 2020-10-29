using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator
{
    class SIC_Device
    {
        private int DeviceID;
        List<byte> WriteBuffer;
        
        /// <summary>
        /// Device Status Word
        /// </summary>
        public int DeviceSW;

        public SIC_Device( int DeviceNumber )
        {
            this.DeviceID = DeviceNumber;
            this.WriteBuffer = new List<byte>();
            this.DeviceSW = 0;
        }

        public void WriteByte( byte Value)
        {
            WriteBuffer.Add(Value);
        }


        public byte ReadByte()
        {
            byte Z = 0;

            return Z;

        }


        /// <summary>
        /// Returns a UTF-8 String of the ASCII Bytes written to this device
        /// </summary>
        /// <returns></returns>
        public string GetASCIIStringWrites()
        {
            String Result = String.Empty;
            return Result;
        }

        /// <summary>
        /// Returns a UTF-8 String of the HEX Codes of the bytes written to this device
        /// </summary>
        /// <returns></returns>
        public string GetHEXStringWrites()
        {
            String Result = String.Empty;
            return Result;
        }



    }
}
