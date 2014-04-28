using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class AddWF : BaseDAddressFunction
    {
        public AddWF() : base(0x7, 8, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            int NewValue = Pic.WRegister.Value + Value;
            
            Pic.RegisterMap.CarryBit = NewValue > 0xFF;
            Pic.RegisterMap.DigitalCarryBit = ((Pic.WRegister.Value & 0xF) + (Value & 0xF)) > 0xF;
            return (byte)NewValue;
        }
    }
}
