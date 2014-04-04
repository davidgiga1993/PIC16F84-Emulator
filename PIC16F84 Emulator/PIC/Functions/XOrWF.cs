using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class XOrWF : BaseDAddressFunction
    {
        public XOrWF()
            : base(0x600, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            return (byte)(Pic.WRegister.Value ^ Value);
        }
    }
}
