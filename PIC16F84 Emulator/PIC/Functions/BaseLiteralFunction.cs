using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    /// <summary>
    /// Stellt eine abstrakte Klasse für eine Funktion mit einem Literal als Paramter da
    /// </summary>
    abstract class BaseLiteralFunction : BaseFunction
    {
        public BaseLiteralFunction(int Bitmask, int BitmaskShift, int Cycles)
            : base(Bitmask, BitmaskShift, Cycles)
        {
        }

        public override void Execute(PIC Pic, BytecodeLine Line)
        {
            int Command = Line.Command;
            int Literal = Command & 0xFF;

            if(Calculate(Pic, Line, Literal))
                Pic.RegisterMap.ProgrammCounter++;
        }

        /// <summary>
        /// Berechnet den neuen Wert eines Registers
        /// Der Wert wird / muss in dieser Funktion gesetzt werden
        /// </summary>
        /// <param name="Pic">Pic Context</param>
        /// <param name="Line">Aktuelle Quellcode Zeile</param>
        /// <param name="Literal">Literal Parameter</param>    
        /// <returns>True wenn PC um 1 erhöht werden soll</returns>
        public abstract bool Calculate(PIC Pic, BytecodeLine Line, int Literal);
    }
}
