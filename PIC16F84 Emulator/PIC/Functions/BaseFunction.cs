using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    abstract class BaseFunction
    {
        public int Bitmask;
        public int Cycles;

        public BaseFunction(int Bitmask, int Cycles)
        {
            this.Bitmask = Bitmask;
            this.Cycles = Cycles;
        }

        public virtual bool Match(SourceLine Line)
        {
            return ((Line.Command & Bitmask) != 0);
        }

        public abstract void Execute(PIC Pic, SourceLine Line);
    }
}
