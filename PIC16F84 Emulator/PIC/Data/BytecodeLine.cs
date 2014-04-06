using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    /// <summary>
    /// Stelle eine Zeile Bytecode dar
    /// </summary>
    public class BytecodeLine
    {
        /// <summary>
        /// Adresse des Bytecodes
        /// </summary>
        public int Address;

        /// <summary>
        /// Befehl des Bytecodes
        /// </summary>
        public int Command;

        /// <summary>
        /// Passende Quellcodezeile
        /// </summary>
        public int SourceCodeLineIndex;

        public BytecodeLine(LSTLine Line, int SourceCodeLineIndex)
        {
            this.Address = Line.Address;
            this.Command = Line.Command;
            this.SourceCodeLineIndex = SourceCodeLineIndex;
        }
    }
}
