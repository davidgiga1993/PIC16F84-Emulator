using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class ClrF : BaseFunction
    {
        public ClrF()
            : base(0x3, 7, 1)
        {

        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            int Command = Line.Command;
            int RegAddress = Command & 0x7F;

            byte NewValue = 0;
            Pic.RegisterMap.SetZBit(true);
            Pic.RegisterMap.Set(NewValue, RegAddress);

            Pic.RegisterMap.ProgrammCounter++;
        }
    }
}
