using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class ClrWdt : BaseFunction
    {
        public ClrWdt()
            : base(0x64, 0, 1)
        {
        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            //TODO!
        }
    }
}