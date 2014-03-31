using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Register
{
    public class RegisterFileMap
    {
        protected DataAdapter<byte>[] Data;

        public RegisterFileMap()
        {
            Data = new DataAdapter<byte>[256];
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X] = new DataAdapter<byte>();
            }
            Data[3].Value = 1 << 3 & 1 << 4;
            Data[0x83].Value = Data[3].Value;
            Data[0x81].Value = 255;
        }

        public void Set(byte Data, int Position)
        {
            if (IsBank1())
                Position += 0x80;
            this.Data[Position].Value = Data;

            switch (Position)
            {
                case 0x03:
                    this.Data[0x83].Value = Data;
                    break;
                case 0x83:
                    this.Data[0x03].Value = Data;
                    break;
            }
        }

        public byte Get(int Position)
        {
            if (IsBank1())
                Position += 0x80;
            return Data[Position].Value;
        }

        public bool IsBank1()
        {
            return (Data[2].Value & (1 << 5)) != 0;
        }
    }
}
