using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class RlF : BaseDAddressFunction
    {
        public RlF() : base(0xD, 8, 1, false)
        {
        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            int NewValue = Value << 1;
            NewValue = NewValue + (Pic.RegisterMap.CarryBit ? 1 : 0);
            Pic.RegisterMap.CarryBit = (NewValue & 0x100) != 0;
            return (byte)NewValue;
        }
    }
}
