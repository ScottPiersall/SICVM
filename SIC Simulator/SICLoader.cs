using System.Globalization;

namespace SIC_Simulator
{
    // Assigned to Brandon Woodrum

    // This class should have two constructors
    // Zero Parameter (Absolute)
    // Single Parameter (StartAddress) - > Relocate program to StartAddress

    internal static class SICLoader
    {
        public static (int start, int length) LoadObjectFileIntoCPU(string[] lines, SIC_CPU cpu)
        {
            (int start, int length) last = default;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line[0] == 'H')
                {
                    string firstAddress = line.Substring(7, 6);
                    string programSize = line.Substring(13, 6);
                    cpu.CurrentProgramEndAddress = int.Parse(firstAddress, NumberStyles.HexNumber) + int.Parse(programSize, NumberStyles.HexNumber);
                    // Read The Header Record
                    // In this context, not much to do here.
                    // from header record. 
                    // The linker module and full-implementation loader
                    // will need to look at the H records

                    last.start = int.Parse(firstAddress, NumberStyles.HexNumber);
                    last.length = int.Parse(programSize, NumberStyles.HexNumber);
                }
                if (line[0] == 'T')
                {
                    // Read T Text Record
                    int recordStartAddress = 0;
                    int recordLength = 0;

                    ReadTextRecord(line, ref recordStartAddress, ref recordLength);

                    cpu.LoadToMemory(line, recordStartAddress, recordLength);
                }

                if (line[0] == 'E')
                {
                    // Read The End Record and Set PC
                    int addressOfFirstInstruction = 0;
                    ReadEndRecord(line, ref addressOfFirstInstruction);

                    cpu.PC = addressOfFirstInstruction;
                    cpu.CurrentProgramStartAddress = addressOfFirstInstruction;
                }
            }

            return last;
        }

        private static void ReadEndRecord(string line, ref int firstExecIns)
        {
            int num = 0;
            for (int i = 1; i < 7; i++)
            {
                int ch = int.Parse($"{line[i]}", NumberStyles.HexNumber);

                num += ch;
                num = num << 4;
            }

            firstExecIns = num >> 4;
        }

        private static void ReadTextRecord(string line, ref int recordStartAdd, ref int recordLength)
        {
            int num = 0;
            for (int i = 1; i < 7; i++)
            {
                int ch = int.Parse($"{line[i]}", NumberStyles.HexNumber);

                num += ch;
                num = num << 4;
            }

            num = num >> 4;
            recordStartAdd = num;

            num = 0;
            for (int i = 7; i < 9; i++)
            {
                int ch = int.Parse($"{line[i]}", NumberStyles.HexNumber);

                num += ch;
                num = num << 4;
            }

            num = num >> 4;
            recordLength = num;
        }
    }
}
