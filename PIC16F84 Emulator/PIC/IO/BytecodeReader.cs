using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.IO
{
    /// <summary>
    /// Wird für das Parsen von der LST Datei benutzt.    /// 
    /// </summary>
    public class BytecodeReader
    {
        /// <summary>
        /// Liest eine Datei mit dem gegebenem Pfad ein und Parst diese
        /// </summary>
        /// <param name="Filepath">Absoluter Dateipfad</param>
        /// <returns>Geparste Zeilen</returns>
        public SourceLine[] ReadSourcecode(string Filepath)
        {
            StreamReader sr = new StreamReader(Filepath);
            string Data = sr.ReadToEnd();
            Data = Data.Replace('\r',' ');
            sr.Close();
            string[] Lines = Data.Split('\n');
            List<SourceLine> ParsedLines = new List<SourceLine>();
            for (int X = 0; X < Lines.Length; X++)
            {
                try
                {
                    SourceLine Line = new SourceLine(Lines[X]);
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
