using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    public class BytecodeLine
    {
        public int Address;
        public int Command;
        public int SourceCodeLineIndex;

        public BytecodeLine(LSTLine Line, int SourceCodeLineIndex)
        {
            this.Address = Line.Address;
            this.Command = Line.Command;
            this.SourceCodeLineIndex = SourceCodeLineIndex;
        }
    }
}
