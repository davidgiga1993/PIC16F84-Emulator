using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class SubLW : BaseLiteralFunction
    {
        public SubLW()
            : base(0x3D, 8, 1)
        {

        }

        public override bool Calculate(PIC Pic, Data.BytecodeLine Line, int Literal)
        {
            int NewValue = Literal - Pic.WRegister.Value;
            Pic.RegisterMap.ZeroBit = NewValue == 0;
            Pic.RegisterMap.CarryBit = NewValue > 0;
            Pic.RegisterMap.DigitalCarryBit = NewValue > 0xF;
            Pic.WRegister.Value = (byte)NewValue;
            return true;
        }

        /// <summary>
        /// Die Match Routine wird überschrieben da Bit 8 beliebig sein darf.
        /// Dafür wird dieses Bit immer gesetzt und auch in der Maske hinzugefügt.
        /// </summary>
        /// <param name="Line"></param>
        /// <returns></returns>
        public override bool Match(Data.BytecodeLine Line)
        {
            int Command = Line.Command | (1 << 8);
            return base.Match(Command);
        }
    }
}
