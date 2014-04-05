using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    class BcF : BaseBitFunction
    {

        public BcF() : base(0x4, 10, 1)
        {

        }

        public override byte Calculate(PIC Pic, BytecodeLine Line, byte Value, int BitPosition)
        {
            return Helper.UnsetBit(BitPosition, Value);
        }
    }
}
