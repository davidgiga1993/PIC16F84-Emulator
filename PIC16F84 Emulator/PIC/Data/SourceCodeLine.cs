using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    public class SourceCodeLine
    {
        public string LineNr;
        public string SourceCode;

        public SourceCodeLine(LSTLine Line)
        {
            this.SourceCode = Line.Source;
        }
    }
}
