using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class IorWF : BaseDAddressFunction
    {
        public IorWF()
            : base(0x400, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            return (byte)(Pic.WRegister.Value | Value);
        }
    }
}
