using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIC_Simulator
{
    internal class SIC_Device
    {
        private readonly int DeviceID;

        /// <summary>
        /// Device Status Word
        /// </summary>
        public int DeviceSW;
        private readonly List<byte> WriteBuffer;
        private readonly StringBuilder WriteBufferASCII;

        public string GetWriteBufferASCIIByteString => WriteBufferASCII.ToString();

        public SIC_Device(int DeviceNumber)
        {
            DeviceID = DeviceNumber;
            WriteBuffer = new List<byte>();
            DeviceSW = 0;
            WriteBufferASCII = new System.Text.StringBuilder();
        }

        public void WriteByte(byte Value)
        {
            WriteBuffer.Add(Value);

            char ch = (char)Value;
            if (!char.IsControl(ch))
            {
                WriteBufferASCII.Append(ch);
            }
            else
            {
                WriteBufferASCII.Append("<" + Value.ToString("X2") + ">");
            }
        }


        public byte ReadByte()
        {
            return WriteBuffer.Last();
        }


        /// <summary>
        /// Returns a UTF-8 String of the ASCII Bytes written to this device
        /// </summary>
        /// <returns></returns>
        public string GetASCIIStringWrites()
        {
            string Result = string.Empty;

            foreach (byte b in WriteBuffer)
            {
                char ch = (char)b;
                if (!char.IsControl(ch))
                {
                    Result += ch;

                }
                else
                {
                    Result += "<" + b.ToString("X2") + ">";
                }

            }

            return Result;
        }

        public void Clear()
        {
            WriteBuffer.Clear();
            WriteBufferASCII.Clear();
        }
    }
}
