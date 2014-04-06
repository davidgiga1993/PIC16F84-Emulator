using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    /// <summary>
    /// Stellt eine Funktion des Pics dar.
    /// Alle weiteren Funktionen müssen von dieser Klasse abgeleitet werden.
    /// </summary>
    public abstract class BaseFunction
    {
        public int Bitmask;
        public int BitmaskShift;
        public int Cycles;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Bitmask">Die Bitmaske der Funktion</param>
        /// <param name="BitmaskShift">Rechtsshift des Befehls vor dem Vergleich mit der Bitmaske</param>
        /// <param name="Cycles">Wieviel Zyklen benötigt diese Funktion</param>
        public BaseFunction(int Bitmask, int BitmaskShift, int Cycles)
        {
            this.Bitmask = Bitmask;
            this.BitmaskShift = BitmaskShift;
            this.Cycles = Cycles;
        }

        /// <summary>
        /// Wird vom Pic aufgerufen um festzustellen ob der aktuelle Befehl dieser Funktion entspricht
        /// </summary>
        /// <param name="Line">Aktuelle Bytecode Zeile</param>
        /// <returns></returns>
        public virtual bool Match(BytecodeLine Line)
        {
            return Match(Line.Command);
        }

        public virtual bool Match(int Command)
        {
            return (Command >> BitmaskShift) == Bitmask;
        }

        /// <summary>
        /// Wird vom Pic aufgerufen um die Funktion auszuführen. Der Programmcounter muss innerhalb dieses Aufrufes von der Funktion selbst erhöht werden.        
        /// </summary>
        /// <param name="Pic"></param>
        /// <param name="Line">Aktuelle Bytecode Zeile</param>
        public abstract void Execute(PIC Pic, BytecodeLine Line);
    }
}
