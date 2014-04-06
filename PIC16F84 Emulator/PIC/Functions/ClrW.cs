using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class ClrW : BaseFunction
    {
        public ClrW()
            : base(0x2, 7, 1)
        {

        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            int Command = Line.Command;

            byte NewValue = 0;
            Pic.RegisterMap.ZeroBit = true;
            Pic.WRegister.Value = NewValue;

            Pic.RegisterMap.ProgrammCounter++;
        }
    }
}
