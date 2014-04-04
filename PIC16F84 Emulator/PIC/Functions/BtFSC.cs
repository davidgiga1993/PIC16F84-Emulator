using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    class BtFSC : BaseBitFunction
    {

        public BtFSC()
            : base(0x1800, 1)
        {

        }

        public override byte Calculate(PIC Pic, SourceLine Line, byte Value, int BitPosition)
        {
            if(Helper.CheckBit(BitPosition, Value))
            {
                Cycles = 1;                
            }
            else
            {
                Cycles = 2;
            }
            return Value;
        }
    }
}
