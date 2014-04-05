using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class AndWF : BaseDAddressFunction
    {
        public AndWF()
            : base(0x5, 8,  1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            return (byte)(Pic.WRegister.Value & Value);
        }
    }
}
