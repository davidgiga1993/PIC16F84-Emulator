using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    abstract class BaseBitFunction : BaseFunction
    {
        public BaseBitFunction(int Bitmask, int BitmaskShift, int Cycles)
            : base(Bitmask, BitmaskShift, Cycles)
        {
        }

        public override void Execute(PIC Pic, BytecodeLine Line)
        {
            int Command = Line.Command;
            int RegAddress = Command & 0x7F;
            int BitPosition = (Command >> 7) & 0x7;

            byte Value = Pic.RegisterMap.Get(RegAddress);
            Value = Calculate(Pic, Line, Value, BitPosition);

            Pic.RegisterMap.Set(Value, RegAddress);

            Pic.RegisterMap.ProgrammCounter++;
        }

        /// <summary>
        /// Berechnet den neuen Wert eines Registers
        /// </summary>
        /// <param name="Pic">Pic Context</param>
        /// <param name="Line">Aktuelle Quellcode Zeile</param>
        /// <param name="Value">Ausgelesener Wert</param>
        /// <param name="BitPosition">Bit Position Parameter</param>
        /// <returns>Neuer Wert</returns>
        public abstract byte Calculate(PIC Pic, BytecodeLine Line, byte Value, int BitPosition);
    }
}
