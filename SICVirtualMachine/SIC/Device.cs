using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SICVirtualMachine.SIC
{
    internal class Device
    {
        public int DeviceID { get; }

        /// <summary>
        /// Device Status Word
        /// </summary>
        public int DeviceSW { get; set; }

        private readonly List<byte> WriteBuffer;
        private readonly StringBuilder WriteBufferASCII;

        public string GetWriteBufferASCIIByteString => WriteBufferASCII.ToString();

        public Device(int deviceNumber)
        {
            DeviceID = deviceNumber;
            WriteBuffer = new List<byte>();
            DeviceSW = 0;
            WriteBufferASCII = new StringBuilder();
        }

        public void WriteByte(byte value)
        {
            WriteBuffer.Add(value);

            char ch = (char)value;
            if (!char.IsControl(ch))
            {
                WriteBufferASCII.Append(ch);
            }
            else
            {
                WriteBufferASCII.Append($"<{value:X2}>");
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
