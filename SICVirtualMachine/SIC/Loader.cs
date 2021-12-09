using System;
using System.Globalization;

namespace SICVirtualMachine.SIC
{
    // Assigned to Brandon Woodrum

    // This class should have two constructors
    // Zero Parameter (Absolute)
    // Single Parameter (StartAddress) - > Relocate program to StartAddress

    internal static class Loader
    {
        public static (int start, int length) LoadObjectFileIntoCPU(string[] lines, CPU cpu)
        {
            (int start, int length) last = default;

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                switch (line[0])
                {
                    case 'H':
                        last = LoadHeadRecord(line, cpu);
                        break;

                    case 'T':
                        LoadTextRecord(line, cpu);
                        break;

                    case 'E':
                        LoadEndRecord(line, cpu);
                        break;

                    default:
                        throw new Exception("Unknown record type.");
                }
            }

            return last;
        }

        private static (int start, int length) LoadHeadRecord(string line, CPU cpu)
        {
            int start = ParseHexValue(line, 7, 6);
            int length = ParseHexValue(line, 13, 6);

            cpu.CurrentProgramEndAddress = start + length;
            return (start, length);
        }

        private static void LoadTextRecord(string line, CPU cpu)
        {
            ReadTextRecord(line, out int recordStartAddress, out int recordLength);

            cpu.LoadToMemory(line, recordStartAddress, recordLength);
        }

        private static void LoadEndRecord(string line, CPU cpu)
        {
            ReadEndRecord(line, out int addressOfFirstInstruction);

            cpu.PC = addressOfFirstInstruction;
            cpu.CurrentProgramStartAddress = addressOfFirstInstruction;
        }

        private static void ReadEndRecord(string line, out int firstExecIns)
        {
            firstExecIns = ParseHexValue(line, 1, 6);
        }

        private static void ReadTextRecord(string line, out int recordStartAdd, out int recordLength)
        {
            recordStartAdd = ParseHexValue(line, 1, 6);
            recordLength = ParseHexValue(line, 7, 2);
        }

        private static int ParseHexValue(string s, int start, int length)
        {
            return int.Parse(s.Substring(start, length), NumberStyles.HexNumber);
        }
    }
}
