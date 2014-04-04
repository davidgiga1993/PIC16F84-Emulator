using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class IncF : BaseDAddressFunction
    {
        public IncF()
            : base(0xA00, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            return (byte)(Value + 1);
        }
    }
}
