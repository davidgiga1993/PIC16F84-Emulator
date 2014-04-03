using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class AddWF : BaseFunction
    {
        public AddWF() : base(0x700, 1)
        {

        }

        public override void Execute(PIC Pic, Data.SourceLine Line)
        {
            int Command = Line.Command & ~Bitmask;
            int RegAddress = Command & 0x7E;
            bool D = (Command & 0x80) != 0;

            byte NewValue = (byte)(Pic.WRegister.Value + Pic.RegisterMap.Get(RegAddress));
            if(D)
            {                
                Pic.RegisterMap.Set(NewValue, RegAddress);
            }
            else
            {
                Pic.WRegister.Value = NewValue;
            }
        }
    }
}
