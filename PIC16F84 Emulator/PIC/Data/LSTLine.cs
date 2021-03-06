﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    /// <summary>
    /// Stellt eine geparste Zeile der lst Datei dar
    /// </summary>
    public class LSTLine
    {
        public string Source;
        public bool ContainsBytecode;

        public int Address;
        public int Command;

        /// <summary>
        /// Parst die übergebene Zeile
        /// </summary>
        /// <param name="Source">Zeile aus der lst Datei</param>
        public LSTLine(string Source)
        {

            if (Source.StartsWith(" ")) // Zeile fängt mit Leerzeichen an -> Kein Bytecode in der Datei, nur Quellcode
            {
                ContainsBytecode = false;
                this.Source = Source.Trim(); // Alle Leerzeichen am Anfang und Ende entfernen
            }
            else
            {
                // Ersten 4 Zeichen parsen
                string AddressStr = Source.Remove(4);
                Address = Convert.ToInt32(AddressStr, 16);

                // Zeichen 6 bis 9 parsen
                string DataStr = Source.Remove(0, 5).Remove(4);
                Command = Convert.ToInt32(DataStr, 16);

                ContainsBytecode = true;
                // Restliche Zeichen sind Quellcode
                this.Source = Source.Remove(0, 10).Trim();
            }
        }
    }
}
