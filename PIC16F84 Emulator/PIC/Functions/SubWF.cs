using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class SubWF : BaseDAddressFunction
    {
        public SubWF()
            : base(0x200, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            int NewValue = Value + (~Pic.WRegister.Value) + 1;
            Pic.RegisterMap.SetCBit(NewValue > 255);
            Pic.RegisterMap.SetDCBit(NewValue > 15);
            return (byte)NewValue;
        }
    }
}
