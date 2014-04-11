using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class RetFIE : BaseFunction
    {
        public RetFIE()
            : base(0x9, 0, 2)
        {

        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            Pic.RegisterMap.ProgrammCounter = Pic.Stack.Pop();
            Pic.RegisterMap.GlobalInterruptEnable = true;
        }
    }
}
