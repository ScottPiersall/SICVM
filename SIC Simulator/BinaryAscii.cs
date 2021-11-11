using System;
using System.Text;

namespace SIC_Simulator
{
    public class HexToBinaryAscii
    {
        string table[16] = {"0000", "0001", "0010", "0011",
                            "0100", "0101", "0110", "0111",
                            "1000", "1001", "1010", "1011",
                            "1100", "1101", "1110", "1111"}
        public HexToBinaryAscii()
        {

        }

        public string toBinary(string input) // Pass a hexadecimal formatted string to this method to return the equivalent binary string
        {
            string i = input.toUpper();
            StringBuilder sb = new StringBuilder();
            int temp;
            for(int j = 0; j < i.length(); j++)
            {
                if(Uri.IsHexDigit(i[j]) == false)
                {
                    return "NULL";
                }

                temp = (int)i[j];
                if (char.isDigit(i[j]))
                    temp -= 48;
                else
                    temp -= 65;

                sb.Append(table[temp] + " ");
            }
        }
    }
}