using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.IO
{
    /// <summary>
    /// Wird für das Parsen von der LST Datei benutzt
    /// </summary>
    public class BytecodeReader
    {
        /// <summary>
        /// Liest eine Datei mit dem gegebenem Pfad ein und Parst diese
        /// </summary>
        /// <param name="Filepath">Absoluter Dateipfad</param>
        /// <returns>Geparste Zeilen</returns>
        public LSTLine[] ReadSourcecode(string Filepath)
        {
            StreamReader sr = new StreamReader(Filepath);
            string Data = sr.ReadToEnd();
            sr.Close();

            // \r entfernen um eine einheitliche Ausgangsbasis zu haben
            Data = Data.Replace('\r',' ');
            
            // Zeilen aufteilen
            string[] Lines = Data.Split('\n');
            List<LSTLine> ParsedLines = new List<LSTLine>();
            for (int X = 0; X < Lines.Length; X++)
            {
                try
                {
                    // Das Parsen einer Zeile wird in LSTLine erledigt
                    LSTLine Line = new LSTLine(Lines[X]);
                    ParsedLines.Add(Line);
                }
                catch(Exception)
                {
                }
            }
            return ParsedLines.ToArray();
        }
    }
}
