using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class Return : BaseFunction
    {
        public Return() : base(0x8, 0, 2)
        {

        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            Pic.RegisterMap.ProgrammCounter = Pic.Stack.Pop();
        }
    }
}
