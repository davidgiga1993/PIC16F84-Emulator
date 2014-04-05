using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.Register
{
    public class RegisterFileMap
    {
        private static readonly int REG_STATUS_ADDRESS = 0x3;
        private static readonly int REG_PROGRAMCOUNTER_ADDRESS = 0x2;
        private static readonly int REG_C_BIT = 0;
        private static readonly int REG_DC_BIT = 1;
        private static readonly int REG_Z_BIT = 2;

        /// <summary>
        /// Stellt alle Register da
        /// </summary>
        protected DataAdapter<byte>[] Data;
        /// <summary>
        /// Speichert die Zuordnung von Adresse auf reelle Speicheradresse.
        /// Dadurch können verschiedene Adressen auf die gleiche Speicherstelle gemappt werden.
        /// Wird beispielsweise für das Status Register verwendet (Adresse 0x3 und 0x83)
        /// </summary>
        protected int[] Mapping;

        /// <summary>
        /// Initialisiert die Daten im Register
        /// Die Startwerte wurden dem Datenblatt entnommen        
        /// </summary>
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

            Set(1 << 3 | 1 << 4, 0x3);
            Set(0xFF, 0x81);
            Set(0xFF, 0x85);
            Set(0xFF, 0x86);
        }

        /// <summary>
        /// Setzt das Z Bit
        /// </summary>
        /// <param name="On">Z Bit an oder aus</param>
        public void SetZBit(bool On)
        {
            byte Register = Get(REG_STATUS_ADDRESS);
            if (On)
                Register = Helper.SetBit(REG_Z_BIT, Register);
            else
                Register = Helper.UnsetBit(REG_Z_BIT, Register);
            Set(Register, REG_STATUS_ADDRESS);
        }

        /// <summary>
        /// Setzt das Carry Bit
        /// </summary>
        /// <param name="On">Carry an oder aus</param>
        public void SetCBit(bool On)
        {
            byte Register = Get(REG_STATUS_ADDRESS);
            if (On)
                Register = Helper.SetBit(REG_C_BIT, Register);
            else
                Register = Helper.UnsetBit(REG_C_BIT, Register);
            Set(Register, REG_STATUS_ADDRESS);
        }

        /// <summary>
        /// Stellt den Programmcounter da. Dieser wrapper wird benutzt um den Zugriff zu erleichtern
        /// </summary>
        /// <returns>Adresse im Programmcounter. Es wird int verwendet da der PC 13 bit groß ist</returns>
        public int ProgrammCounter
        {
            get
            {
                return Get(REG_PROGRAMCOUNTER_ADDRESS);
            }
            set
            {
                Set((byte)value, REG_PROGRAMCOUNTER_ADDRESS);
            }
        }

        public DataAdapter<byte> ProgrammCounterAdapter
        {
            get
            {
                return GetAdapter(REG_PROGRAMCOUNTER_ADDRESS);
            }
        }

        /// <summary>
        /// Gibt das Carry Bit zurück
        /// </summary>
        /// <returns>True wenn Carry gesetzt</returns>
        public bool GetCBit()
        {
            return Helper.CheckBit(REG_C_BIT, Get(REG_STATUS_ADDRESS));
               
        }

        /// <summary>
        /// Setzt das Digital Carry Bit
        /// </summary>
        /// <param name="On">Digital Carry an oder aus</param>
        public void SetDCBit(bool On)
        {
            byte Register = Get(REG_STATUS_ADDRESS);
            if (On)
                Register = Helper.SetBit(REG_DC_BIT, Register);
            else
                Register = Helper.UnsetBit(REG_DC_BIT, Register);
            Set(Register, REG_STATUS_ADDRESS);
        }
        /// <summary>
        /// Setzt ein Register an der gegebenen Adresse
        /// </summary>
        /// <param name="Data">Daten fürs Register</param>
        /// <param name="Position">Register Adresse</param>
        public void Set(byte Data, int Position)
        {
            if (IsBank1())
                Position += 0x80;

            Position = Mapping[Position];
           
            this.Data[Position].Value = Data;
        }

        /// <summary>
        /// Gibt ein Register zurück
        /// </summary>
        /// <param name="Position">Register Adresse</param>
        /// <returns>Register Daten</returns>
        public byte Get(int Position)
        {
            if (IsBank1())
                Position += 0x80;
            Position = Mapping[Position];

            return Data[Position].Value;
        }

        public DataAdapter<byte> GetAdapter(int Position)
        {
            Position = Mapping[Position];
            return Data[Position];
        }

        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        /// <summary>
        /// Wird benutzt um zu prüfen ob Bank 1 aktiv ist
        /// </summary>
        /// <returns>True wenn Bank 1 aktiv ist</returns>
        public bool IsBank1()
        {
            return (Data[3].Value & (1 << 5)) != 0;
        }
    }
}
