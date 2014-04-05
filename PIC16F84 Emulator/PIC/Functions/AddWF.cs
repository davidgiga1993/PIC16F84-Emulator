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
            Pic.RegisterMap.SetCBit(NewValue > 0xFF);
            Pic.RegisterMap.SetDCBit(NewValue > 0xF);
            return (byte)NewValue;
        }
    }
}
