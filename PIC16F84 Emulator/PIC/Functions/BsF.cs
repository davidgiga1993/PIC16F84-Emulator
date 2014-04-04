using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    class BsF : BaseBitFunction
    {

        public BsF()
            : base(0x1400, 1)
        {

        }

        public override byte Calculate(PIC Pic, SourceLine Line, byte Value, int BitPosition)
        {
            return Helper.SetBit(BitPosition, Value);
        }
    }
}
