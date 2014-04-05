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
            int CarryValue = Pic.RegisterMap.GetCBit() ? 1 : 0;
            int NewValue =  CarryValue << 7 + Value >> 1;
            return (byte)NewValue;
        }
    }
}
