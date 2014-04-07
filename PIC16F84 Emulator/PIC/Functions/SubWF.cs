﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class SubWF : BaseDAddressFunction
    {
        public SubWF()
            : base(0x2, 8, 1, true)
        {

        }

        public override byte Calculate(PIC Pic, Data.BytecodeLine Line, byte Value)
        {
            int NewValue = Value - Pic.WRegister.Value;
            Pic.RegisterMap.CarryBit = NewValue > 0;
            Pic.RegisterMap.DigitalCarryBit = NewValue > 0xF;
            return (byte)NewValue;
        }
    }
}
