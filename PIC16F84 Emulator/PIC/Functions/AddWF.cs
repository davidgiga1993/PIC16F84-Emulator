﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class AddWF : BaseDAddressFunction
    {
        public AddWF() : base(0x700, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            int NewValue = Pic.WRegister.Value + Value;
            Pic.RegisterMap.SetCBit(NewValue > 255);
            Pic.RegisterMap.SetDCBit(NewValue > 15);
            return (byte)NewValue;
        }
    }
}
