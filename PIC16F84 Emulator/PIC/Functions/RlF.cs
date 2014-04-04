using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class RlF : BaseDAddressFunction
    {
        public RlF() : base(0xD00, 1, false)
        {
        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            int NewValue = Value << 1;
            Pic.RegisterMap.SetCBit((NewValue & 0x100) == 1);
            return (byte)NewValue;
        }
    }
}
