using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PIC16F84_Emulator.PIC
{
    public class PIC
    {
        protected Register.RegisterFileMap RegisterMap = new Register.RegisterFileMap();
        protected byte[] Memory = new byte[114688];
    }
}
