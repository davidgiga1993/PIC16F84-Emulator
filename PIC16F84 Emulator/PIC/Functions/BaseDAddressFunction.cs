using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Functions
{
    abstract class BaseDAddressFunction : BaseFunction
    {
        private bool AffectsZFlag;
        public BaseDAddressFunction(int Bitmask, int BitmaskShift, int Cycles, bool AffectsZFlag) : base(Bitmask, BitmaskShift, Cycles)
        {
            this.AffectsZFlag = AffectsZFlag;
        }

        public override void Execute(PIC Pic, BytecodeLine Line)
        {
            int Command = Line.Command;
            int RegAddress = Command & 0x7F;
            bool D = (Command & 0x80) != 0;

            byte Value = Pic.RegisterMap.Get(RegAddress);
            Value = Calculate(Pic, Line, Value);

            if (AffectsZFlag)
            {
                Pic.RegisterMap.SetZBit(Value == 0);
            }

            if (D)
            {
                Pic.RegisterMap.Set(Value, RegAddress);
            }
            else
            {
                Pic.WRegister.Value = Value;
            }

            Pic.RegisterMap.ProgrammCounter++;
        }

        /// <summary>
        /// Berechnet den neuen Wert eines Registers
        /// </summary>
        /// <param name="Pic"></param>
        /// <param name="Line"></param>
        /// <param name="Value">Ausgelesener Wert</param>
        /// <returns>Neuer Wert</returns>
        public abstract byte Calculate(PIC Pic, BytecodeLine Line, byte Value);
    }
}
