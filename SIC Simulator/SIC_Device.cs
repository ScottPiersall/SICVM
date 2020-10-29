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

        public SIC_Device( int DeviceNumber )
        {
            this.DeviceID = DeviceNumber;
            this.WriteBuffer = new List<byte>();
        }

        public void WriteByte( byte Value)
        {
            WriteBuffer.Add(Value);
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
