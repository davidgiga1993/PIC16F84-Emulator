using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Data
{
    /// <summary>
    /// Enthält den Stack des Pics
    /// </summary>
    public class StackData
    {
        /// <summary>
        /// Entspricht dem Stack
        /// Es wird int verwendet, da die Adressen 13 bit groß sind
        /// </summary>
        public DataAdapter<int>[] Entrys = new DataAdapter<int>[8];
        /// <summary>
        /// Aktueller Stack Index
        /// -1 da Push den Wert um 1 erhöht
        /// </summary>
        private int Pos = -1;

        public StackData()
        {
            for(int X= 0; X <Entrys.Length; X++)
            {
                Entrys[X] = new DataAdapter<int>();
            }
        }

        /// <summary>
        /// Fügt eine neue Adresse dem Stack hinzu
        /// </summary>
        /// <param name="Adress">Adresse</param>
        public void Push(int Adress)
        {
            Pos++;
            if (Pos == Entrys.Length)
                Pos = 0;
            Entrys[Pos].Value = Adress;            
        }
        /// <summary>
        /// Gibt den neusten Stack Eintrag zurück und entfernt den Eintrag
        /// </summary>
        /// <returns></returns>
        public int Pop()
        {
            int Value = Entrys[Pos].Value;
            if(Pos > 0)
                Pos--;
            return Value;
        }
    }
}
