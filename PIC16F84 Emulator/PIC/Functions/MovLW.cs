using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class MovLW : BaseLiteralFunction
    {
        public MovLW()
            : base(0x33, 8, 1)
        {

        }

        public override bool Calculate(PIC Pic, Data.BytecodeLine Line, int Literal)
        {
            int NewValue = Pic.WRegister.Value | Literal;
            Pic.RegisterMap.ZeroBit = NewValue == 0;
            Pic.WRegister.Value = (byte)NewValue;
            return true;
        }

        /// <summary>
        /// Die Match Routine wird überschrieben da Bit 8 und 9 beliebig sein dürfen.
        /// Dafür werden diese Bits immer gesetzt und auch in der Maske hinzugefügt.
        /// </summary>
        /// <param name="Line"></param>
        /// <returns></returns>
        public override bool Match(int Command)
        {
            Command = Command | (1 << 8) | (1 << 9);
            return base.Match(Command);
        }
    }
}
