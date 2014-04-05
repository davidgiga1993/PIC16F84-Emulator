using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    public abstract class BaseFunction
    {
        public int Bitmask;
        public int BitmaskShift;
        public int Cycles;

        public BaseFunction(int Bitmask, int BitmaskShift, int Cycles)
        {
            this.Bitmask = Bitmask;
            this.BitmaskShift = BitmaskShift;
            this.Cycles = Cycles;
        }

        public virtual bool Match(BytecodeLine Line)
        {
            return Match(Line.Command);
        }

        public virtual bool Match(int Command)
        {
            return (Command >> BitmaskShift) == Bitmask;
        }

        public abstract void Execute(PIC Pic, BytecodeLine Line);
    }
}
