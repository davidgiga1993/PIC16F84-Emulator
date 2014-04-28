using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class SubWF : BaseDAddressFunction
    {
        public SubWF()
            : base(0x2, 8, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            int WComp = ~Pic.WRegister.Value;
            WComp += 0x1;

            int NewValue = Value + WComp;

            
            Pic.RegisterMap.CarryBit = NewValue >= 0;
            NewValue = (byte)NewValue;
            Pic.RegisterMap.DigitalCarryBit = (Value & 0xF) + (WComp & 0xF) > 0xF;
            return (byte)NewValue;
        }
    }
}
