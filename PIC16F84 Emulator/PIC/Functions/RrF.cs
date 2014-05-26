using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class RrF : BaseDAddressFunction
    {
        public RrF()
            : base(0xC, 8, 1, false)
        {
        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            int CarryValue = Pic.RegisterMap.CarryBit ? 1 : 0;
            int NewCarry = Value & 0x1;
            int NewValue =  CarryValue << 8 + Value >> 1;

            Pic.RegisterMap.CarryBit = NewCarry != 0;

            return (byte)NewValue;
        }
    }
}
