using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class MovWF : BaseFunction
    {
        public MovWF()
            : base(0x80, 1)
        {

        }

        public override void Execute(PIC Pic, Data.SourceLine Line)
        {
            int Command = Line.Command & ~Bitmask;
            int RegAddress = Command & 0x7F;
            Pic.RegisterMap.Set(Pic.WRegister.Value, RegAddress);
        }
    }
}
