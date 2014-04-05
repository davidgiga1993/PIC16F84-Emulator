using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Functions
{
    class Call : BaseFunction
    {
        public Call()
            : base(0x4, 11, 2)
        {
        }

        public override void Execute(PIC Pic, Data.BytecodeLine Line)
        {
            int NewAddress = Line.Command & 0x7FF;
            int PC = Pic.RegisterMap.ProgrammCounter;
            Pic.Stack.Push(PC + 1);

            Pic.RegisterMap.ProgrammCounter = NewAddress;
        }
    }
}
