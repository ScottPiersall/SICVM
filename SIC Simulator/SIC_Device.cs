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

    }
}
