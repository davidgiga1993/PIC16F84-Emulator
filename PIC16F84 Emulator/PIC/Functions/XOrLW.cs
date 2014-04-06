using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class XOrLW : BaseLiteralFunction
    {
        public XOrLW()
            : base(0x3A, 8, 1)
        {

        }

        public override bool Calculate(PIC Pic, Data.BytecodeLine Line, int Literal)
        {
            int NewValue = Pic.WRegister.Value ^ Literal;
            Pic.RegisterMap.ZeroBit = NewValue == 0;
            Pic.WRegister.Value = (byte)NewValue;
            return true;
        }
    }
}
