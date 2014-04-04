using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class DecF : BaseDAddressFunction
    {
        public DecF()
            : base(0x300, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            return (byte)(Value - 1);
        }
    }
}
