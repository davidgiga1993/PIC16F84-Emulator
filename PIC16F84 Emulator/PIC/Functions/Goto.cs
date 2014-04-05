using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class Goto : BaseFunction
    {
        public Goto()
            : base(0x5, 11, 2)
        {
        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            int NewAddress = Line.Command & 0x7FF;
            Pic.RegisterMap.ProgrammCounter = NewAddress;
        }
    }
}
