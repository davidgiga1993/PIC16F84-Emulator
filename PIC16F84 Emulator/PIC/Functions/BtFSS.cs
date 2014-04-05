using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    class BtFSS : BaseBitFunction
    {

        public BtFSS()
            : base(0x7, 10, 1)
        {

        }

        public override byte Calculate(PIC Pic, BytecodeLine Line, byte Value, int BitPosition)
        {
            if(!Helper.CheckBit(BitPosition, Value))
            {
                Cycles = 1;
            }
            else
            {
                Cycles = 2;
                Pic.RegisterMap.ProgrammCounter++; // Nächster Befehl muss übersprungen werden
            }
            return Value;
        }
    }
}
