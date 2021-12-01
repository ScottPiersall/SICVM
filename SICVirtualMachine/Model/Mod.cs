using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SICVirtualMachine.Model
{

    //New Code Segment By Brandon And Nick

    //class for MOD constructor that holds info for Modification
    internal class Mod
    {
        public int Address { get; private set; }
        public int Half { get; private set; }
        public bool Flag { get; private set; }
        public Mod Next { get; set; }
        public bool Error { get; private set; }

        //if our head is which is a place holder is returned then nothing was found in search. sets its boolean to true
        public void SetError()
        {
            Error = true;
        }

        //sets all values for created mod
        public void set(int address, int half, bool flag)
        {
            Address = address;
            Half = half;
            Flag = flag;
        }

        //searches linked list for Mod record matching T-record starting address, if head is returned as place holder nothing was found
        public Mod Search(Mod head, int add)
        {
            Mod error = new Mod();
            error.SetError();

            while (head.Next != null)
            {
                if (add != head.Address)
                {
                    head = head.Next;
                }
                else
                {
                    return head;
                }
            }

            return error;
        }
    }
}
