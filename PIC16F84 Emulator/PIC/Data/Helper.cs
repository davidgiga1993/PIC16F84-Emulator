using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    class Helper
    {
        public static byte SetBit(int Position, byte Data)
        {
            Data |= (byte)(1 << Position);
            return Data;
        }

        public static byte UnsetBit(int Position, byte Data)
        {
            Data &= (byte)~(1 << Position);
            return Data;
        }

        public static bool CheckBit(int Position, byte Data)
        {
            return (Data & 1 << Position) != 0;
        }
    }
}
