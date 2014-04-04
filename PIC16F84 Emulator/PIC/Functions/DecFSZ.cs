﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class DecFSZ : BaseDAddressFunction
    {
        public DecFSZ()
            : base(0xB00, 1, false)
        {

        }

        public override byte Calculate(PIC Pic, Data.SourceLine Line, byte Value)
        {
            byte NewValue = (byte)(Value - 1);
            if (NewValue == 0)
                Cycles = 2;
            else
                Cycles = 1;
            return NewValue;
        }
    }
}