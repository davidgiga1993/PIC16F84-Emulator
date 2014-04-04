using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class SwapF : BaseDAddressFunction
    {
        public SwapF()
            : base(0xE00, 1, false)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            int NewValue = Value >> 4 + ((Value & 0xF) << 4);
            return (byte)NewValue;
        }
    }
}
