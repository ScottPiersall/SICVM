using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIC_Simulator
{
    class SIC_Device
    {
        public int DeviceID;
        public List<byte> WriteBuffer;
        public int status;


        public SIC_Device( int DeviceNumber )
        {
            this.DeviceID = DeviceNumber;
            this.WriteBuffer = new List<byte>();
            this.status = 1;
        }

        public void WriteByte( byte Value)
        {
            WriteBuffer.Add(Value);
            this.status = 2;
        }

        public void WriteString (string str)
        {
            if (str.Length == 0) { return; }
            foreach (byte b in str)
            {
                WriteBuffer.Add(b);
            }
            this.status = 2;
        }

        public byte ReadByte()
        {
            if (WriteBuffer.Count == 0)
                return 0; //This case should not happen; should be checked by CPU
            byte Z = WriteBuffer[WriteBuffer.Count-1];
            WriteBuffer.RemoveAt(WriteBuffer.Count - 1);
            if (WriteBuffer.Count == 0) 
            {
                this.status = 1;
            }
            return Z;
        }

        public void reset()
        {
            this.WriteBuffer = new List<byte>();
            this.status = 1;
        }


        /// <summary>
        /// Returns a UTF-8 String of the ASCII Bytes written to this device
        /// </summary>
        /// <returns></returns>
        public string GetASCIIStringWrites()
        {
            String Result = String.Empty;
    
            foreach( byte b in WriteBuffer.ToArray())
            {
                char ch = (char)b;
                if  (!  Char.IsControl(ch) ) {
                    Result += ch;

                } else
                {
                    Result += "<" + b.ToString("X2") +  ">";
                }

            }



            return Result;
        }

        /// <summary>
        /// Returns a UTF-8 String of the HEX Codes of the bytes written to this device
        /// </summary>
        /// <returns></returns>
        public string GetHEXStringWrites()
        {
            String Result = String.Empty;

            foreach (byte b in WriteBuffer.ToArray())
            {
                Result += b.ToString("X2") + " ";
            }

            return Result;
        }



    }
}
