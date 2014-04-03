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

        protected int[] Mapping;

        public RegisterFileMap()
        {
            Mapping = new int[256];
            Data = new DataAdapter<byte>[256];

            for (int X = 0; X < Mapping.Length; X++)
            {
                Mapping[X] = X;
                switch (X)
                {
                    case 0x83:
                        Mapping[X] = 0x03;
                        break;
                    case 0x82:
                        Mapping[X] = 0x02;
                        break;
                    case 0x84:
                        Mapping[X] = 0x4;
                        break;
                    case 0x8A:
                        Mapping[X] = 0xA;
                        break;
                    case 0x8B:
                        Mapping[X] = 0xB;
                        break;
                    case 0x87:
                    case 0x7:
                        Mapping[X] = 254;
                        break;
                    default:
                        if (X >= 0x8C && X <= 0xCF)
                        {
                            Mapping[X] = X - 0x80;
                        }
                        break;
                }
            }

            for (int X = 0; X < Data.Length; X++)
            {
                Data[X] = new DataAdapter<byte>();
            }

            Set(0x3, 1 << 3 & 1 << 4);
            Set(0x81, 255);
        }

        public void SetZBit(bool On)
        {
            byte Register = Get(0x3);
            if (On)
                Register = Helper.SetBit(2, Register);
            else
                Register = Helper.UnsetBit(2, Register);
            Set(Register, 0x3);
        }

        public void Set(byte Data, int Position)
        {
            if (IsBank1())
                Position += 0x80;

            Position = Mapping[Position];
           
            this.Data[Position].Value = Data;
        }

        public byte Get(int Position)
        {
            if (IsBank1())
                Position += 0x80;
            Position = Mapping[Position];

            return Data[Position].Value;
        }

        public bool IsBank1()
        {
            return (Data[2].Value & (1 << 5)) != 0;
        }
    }
}
