using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC.Register
{
    class Register
    {
        public byte Data;

        public void Add(Register Reg)
        {
            Add(Reg.Data);
        }

        public void Subtract(Register Reg)
        {
            Subtract(Reg.Data);
        }

        public void Add(byte Value)
        {
            Data += Value;
        }
        public void Subtract(byte Value)
        {
            Data -= Value;
        }
    }
}
