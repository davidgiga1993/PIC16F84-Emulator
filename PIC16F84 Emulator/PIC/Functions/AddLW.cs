﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class AddLW : BaseLiteralFunction
    {
        public AddLW() : base(0x3F, 8, 1)
        {
           
        }
        public override bool Calculate(PIC Pic, Data.BytecodeLine Line, int Literal)        
        {
            int NewValue = Pic.WRegister.Value + Literal;
            Pic.RegisterMap.CarryBit = NewValue > 0xFF;           
            Pic.RegisterMap.ZeroBit = NewValue == 0;

            Pic.RegisterMap.DigitalCarryBit = ((Pic.WRegister.Value & 0xF) + (Literal & 0xF)) > 0xF;
           
            Pic.WRegister.Value = (byte)NewValue;
            return true;
        }

        public override bool Match(int Command)
        {
            Command = Command | (1 << 8);
            return base.Match(Command);
        }
    }
}
