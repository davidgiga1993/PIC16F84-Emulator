using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;
using PIC16F84_Emulator.PIC.Data.Wrapper;

namespace PIC16F84_Emulator.PIC.Register
{
    public class RegisterFileMap
    {
        public delegate void OnFSRChanged();
        public event OnFSRChanged FSRChanged;

        private static readonly int REG_STATUS_ADDRESS = 0x3;
        private static readonly int REG_C_BIT = 0;
        private static readonly int REG_DC_BIT = 1;
        private static readonly int REG_Z_BIT = 2;

        /// <summary>
        /// Stellt alle Register da
        /// </summary>
        protected DataAdapter<byte>[] Data;
        /// <summary>
        /// Programmcounter Integer
        /// </summary>
        protected DataAdapter<int> PC_I;

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
            PC_I = new DataAdapter<int>();
            for (int X = 0; X < Mapping.Length; X++)
            {
                Mapping[X] = X;
                switch (X)
                {
                    case 0x80:
                        Mapping[X] = 0x0;
                        break;
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
                switch (X)
                {
                    case 0x2: // Wird gebraucht um das PC backend (DataAdapter<int>) als byte darzustellen im Register 0x2
                        Data[X] = new IntToByteDataAdapter(PC_I);
                        break;
                    default:
                        Data[X] = new DataAdapter<byte>();
                        break;
                }
            }

            Set(0x18, 0x3);
            Set(0xFF, 0x81);
            Set(0xFF, 0x85);
            Set(0xFF, 0x86);

            Data[0x4].DataChanged += RegisterFileMap_DataChanged;
        }


        /// <summary>
        /// Gibt die Anzahl der Byte im Register an
        /// </summary>
        public int Length
        {
            get
            {
                return Data.Length;
            }
        }

        /// <summary>
        /// Zero Bit
        /// Ist true/1 wenn eine arithmetische Operation 0 als Ergebnis hatte.
        /// </summary>
        public bool ZeroBit
        {
            get
            {
                return Helper.CheckBit(REG_Z_BIT, Get(REG_STATUS_ADDRESS));
            }
            set
            {
                byte Register = Get(REG_STATUS_ADDRESS);
                if (value)
                    Register = Helper.SetBit(REG_Z_BIT, Register);
                else
                    Register = Helper.UnsetBit(REG_Z_BIT, Register);
                Set(Register, REG_STATUS_ADDRESS);
            }
        }

        /// <summary>
        /// Carry Bit
        /// Ist true / 1 wenn ein carry out vom höhsten Bit erfolg ist
        /// Bsp: 0xFF + 0x1 -> Carry Bit auf true
        /// </summary>
        public bool CarryBit
        {
            get
            {
                return Helper.CheckBit(REG_C_BIT, Get(REG_STATUS_ADDRESS));
            }
            set
            {
                byte Register = Get(REG_STATUS_ADDRESS);
                if (value)
                    Register = Helper.SetBit(REG_C_BIT, Register);
                else
                    Register = Helper.UnsetBit(REG_C_BIT, Register);
                Set(Register, REG_STATUS_ADDRESS);
            }
        }

        /// <summary>
        /// Stellt den Programmcounter da. Dieser wrapper wird benutzt um den Zugriff zu erleichtern
        /// </summary>
        /// <returns>Adresse im Programmcounter. Es wird int verwendet da der PC 13 bit groß ist</returns>
        public int ProgrammCounter
        {
            get
            {
                return PC_I.Value;
            }
            set
            {
                PC_I.Value = value;
            }
        }

        /// <summary>
        /// Gibt den Adapter des Programmcounters zurück
        /// </summary>
        public DataAdapter<int> ProgrammCounterAdapter
        {
            get
            {
                return PC_I;
            }
        }

        /// <summary>
        /// Digital Carry Bit
        /// Ist true / 1 wenn bei dem 4. Bit ein carry out erfolg ist.
        /// Bsp: 0xF + 0x1 -> DC Bit auf true
        /// </summary>
        public bool DigitalCarryBit
        {
            get
            {
                return Helper.CheckBit(REG_DC_BIT, Get(REG_STATUS_ADDRESS));
            }
            set
            {
                byte Register = Get(REG_STATUS_ADDRESS);
                if (value)
                    Register = Helper.SetBit(REG_DC_BIT, Register);
                else
                    Register = Helper.UnsetBit(REG_DC_BIT, Register);
                Set(Register, REG_STATUS_ADDRESS);
            }
        }



        /// <summary>
        /// FSR Register hat sich geändert
        /// Mapping anpassen
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Sender"></param>
        private void RegisterFileMap_DataChanged(byte Value, object Sender)
        {
            Mapping[0x0] = Value;
            Mapping[0x80] = Value;
            if (FSRChanged != null)
                FSRChanged();
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
            switch(Position)
            {
                case 0x4:
                    RegisterFileMap_DataChanged(Data, null);
                    break;
                default:                    
                    break;
            }
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
