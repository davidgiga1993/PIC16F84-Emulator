using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    class NOP : BaseFunction
    {
        public NOP()
            : base(0x0, 1)
        {

        }

        public override bool Match(SourceLine Line)
        {
            return (Line.Command == 0 || Line.Command == 0x20 || Line.Command == 0x40 || Line.Command == 0x60);            
        }

        public override void Execute(PIC Pic, SourceLine Line)
        {
        }
    }
}
