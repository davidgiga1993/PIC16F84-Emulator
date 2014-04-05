using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class MovWF : BaseFunction
    {
        public MovWF()
            : base(0x1, 7, 1)
        {

        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            int Command = Line.Command;
            int RegAddress = Command & 0x7F;
            Pic.RegisterMap.Set(Pic.WRegister.Value, RegAddress);

            Pic.RegisterMap.ProgrammCounter++;
        }
    }
}
