using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class IncFSZ : BaseDAddressFunction
    {
        public IncFSZ()
            : base(0xF, 8, 1, false)
        {

        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            byte NewValue = (byte)(Value + 1);
            if (NewValue == 0)
                Cycles = 2;
            else
                Cycles = 1;
            return NewValue;
        }
    }
}
