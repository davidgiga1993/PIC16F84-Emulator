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

        private PIC Pic;

        private static readonly int REG_STATUS_ADDRESS = 0x3;
        private static readonly int REG_INTCON_ADDRESS = 0xB;
        public static readonly int REG_TIMER_ADDRESS = 0x1;
        public static readonly int REG_OPTIONS_ADDRESS = 0x81;

        public static readonly int REG_EEDATA_ADDRESS = 0x08;
        public static readonly int REG_EEADDR_ADDRESS = 0x09;
        public static readonly int REG_EECON1_ADDRESS = 0x88;
        public static readonly int REG_EECON2_ADDRESS = 0x89;

        public static readonly int REG_PORT_A = 0x5;
        public static readonly int REG_PORT_B = 0x6;
        public static readonly int REG_TRIS_A = 0x85;
        public static readonly int REG_TRIS_B = 0x86;

        public static readonly int REG_OPTIONS_PRESCALER_ASSIGMENT = 3;
        public static readonly int REG_OPTIONS_TIMER_MODE = 5;
        public static readonly int REG_OPTIONS_TIMER_SOURCE_EDGE = 4;

        private static readonly int REG_GIE_BIT = 7;
        private static readonly int REG_EEIE_BIT = 6;
        private static readonly int REG_T0IE_BIT = 5;
        private static readonly int REG_T0IF_BIT = 2;
        private static readonly int REG_INTE_BIT = 4;
        private static readonly int REG_INTF_BIT = 1;

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
        public RegisterFileMap(PIC Pic)
        {
            this.Pic = Pic;
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

            Data[0x6].DataChanged += PortB_DataChanged;
            Data[0x4].DataChanged += RegisterFileMap_DataChanged;

            Reset();
        }

        /// <summary>
        /// Setzt alle Register zurück
        /// </summary>
        public void Reset()
        {
            for (int X = 0; X < Data.Length; X++)
            {
                Data[X].Value = 0;
            }
            Set(0x18, 0x3);
            Set(0xFF, 0x81);
            Set(0xFF, 0x85);
            Set(0xFF, 0x86);
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
                return Helper.CheckBit(REG_DC_BIT, Get(REG_STATUS_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_STATUS_ADDRESS, true);
                if (value)
                    Register = Helper.SetBit(REG_DC_BIT, Register);
                else
                    Register = Helper.UnsetBit(REG_DC_BIT, Register);
                Set(Register, REG_STATUS_ADDRESS, true);
            }
        }
        /// <summary>
        /// Global Interrupt Enable flag
        /// </summary>
        public bool GlobalInterruptEnable
        {
            get
            {
                return Helper.CheckBit(REG_GIE_BIT, Get(REG_INTCON_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_INTCON_ADDRESS, true);
                if (value)
                    Register = Helper.SetBit(REG_GIE_BIT, Register);
                else
                    Register = Helper.UnsetBit(REG_GIE_BIT, Register);
                Set(Register, REG_INTCON_ADDRESS, true);
            }
        }
        /// <summary>
        /// TMR0 Overflow flag
        /// </summary>
        public bool TMR0Overflow
        {
            get
            {
                return Helper.CheckBit(REG_T0IF_BIT, Get(REG_INTCON_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_INTCON_ADDRESS, true);
                if(value)
                {
                    Register = Helper.SetBit(REG_T0IF_BIT, Register);
                    Pic.RunInterrupt = true; // Interrupt in nächstem Zyklus überprüfen
                }
                else
                {
                    Register = Helper.UnsetBit(REG_T0IF_BIT, Register);
                }
                Set(Register, REG_INTCON_ADDRESS, true);
            }
        }
        /// <summary>
        /// TMR0 Overflow Interrupt Enable flag
        /// </summary>
        public bool TMR0OverflowInterruptEnable
        {
            get
            {
                return Helper.CheckBit(REG_T0IE_BIT, Get(REG_INTCON_ADDRESS, true));
            }
        }
        /// <summary>
        /// RB0 External Interrupt Enable flag
        /// </summary>
        public bool RB0ExternalInterruptEnable
        {
            get
            {
                return Helper.CheckBit(REG_INTE_BIT, Get(REG_INTCON_ADDRESS, true));
            }
        }

        /// <summary>
        /// RB0/INT External Interrupt Flag bit
        /// </summary>
        public bool RB0ExternalInterrupt
        {
            get
            {
                return Helper.CheckBit(REG_INTF_BIT, Get(REG_INTCON_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_INTCON_ADDRESS, true);
                if (value)
                {
                    if (!Helper.CheckBit(REG_INTF_BIT, Register)) // Auf positive Flanke überprüfen
                        Pic.RunInterrupt = true; // Interrupt in nächstem Zyklus überprüfen
                    Register = Helper.SetBit(REG_INTF_BIT, Register);
                }
                else
                { 
                    Register = Helper.UnsetBit(REG_INTF_BIT, Register);
                }
                Set(Register, REG_INTCON_ADDRESS, true);
            }
        }
        /// <summary>
        /// EEIE Interrupt Enable bit
        /// </summary>
        public bool EEWriteCompleteInterrupt
        {
            get
            {
                return Helper.CheckBit(REG_EEIE_BIT, Get(REG_INTCON_ADDRESS, true));
            }
        }


        /// <summary>
        /// Returns false if the prescaler is assigned to Timer0.
        /// Returns true if the prescaler is assigned to the WDT
        /// </summary>
        public bool PrescalerAssignment
        {
            get
            {
                return Helper.CheckBit(REG_OPTIONS_PRESCALER_ASSIGMENT, Get(REG_OPTIONS_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_OPTIONS_ADDRESS, true);
                if (value)
                {
                    Register = Helper.SetBit(REG_OPTIONS_PRESCALER_ASSIGMENT, Register);
                }
                else
                {
                    Register = Helper.UnsetBit(REG_OPTIONS_PRESCALER_ASSIGMENT, Register);
                }
                Set(Register, REG_OPTIONS_ADDRESS, true);
            }
        }

        /// <summary>
        /// Returns true if RA4/T0CKI pin.
        /// Returns false if CLKOUT pin
        /// </summary>
        public bool TMR0ClockSource
        {
            get
            {
                return Helper.CheckBit(REG_OPTIONS_TIMER_MODE, Get(REG_OPTIONS_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_OPTIONS_ADDRESS, true);
                if (value)
                {
                    Register = Helper.SetBit(REG_OPTIONS_TIMER_MODE, Register);
                }
                else
                {
                    Register = Helper.UnsetBit(REG_OPTIONS_TIMER_MODE, Register);
                }
                Set(Register, REG_OPTIONS_ADDRESS, true);
            }
        }

        /// <summary>
        /// True wenn Edge 1 -> 0.
        /// False wenn Edge 0 -> 1
        /// </summary>
        public bool Option_Timer_Source_Edge
        {
            get
            {
                return Helper.CheckBit(REG_OPTIONS_TIMER_SOURCE_EDGE, Get(REG_OPTIONS_ADDRESS, true));
            }
            set
            {
                byte Register = Get(REG_OPTIONS_ADDRESS, true);
                if (value)
                {
                    Register = Helper.SetBit(REG_OPTIONS_TIMER_SOURCE_EDGE, Register);
                }
                else
                {
                    Register = Helper.UnsetBit(REG_OPTIONS_TIMER_SOURCE_EDGE, Register);
                }
                Set(Register, REG_OPTIONS_ADDRESS, true);
            }
        }
        

        /// <summary>
        /// Port B hat sich geändert
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Sender"></param>
        private void PortB_DataChanged(byte Value, object Sender)
        {
            if(Helper.CheckBit(0, Value)) // Bit 0 gesetzt -> RB0ExtInterrupt setzen
            {
                RB0ExternalInterrupt = true;
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
        /// <param name="Data">Daten für das Register</param>
        /// <param name="Position">Register Adresse</param>
        public void Set(byte Data, int Position)
        {
            Set(Data, Position, false);
        }

        /// <summary>
        /// Setzt ein Register an der gegebenen Adresse
        /// </summary>
        /// <param name="Data">Daten für das Register</param>
        /// <param name="Position">Register Adresse</param>
        /// <param name="IgnoreBankBit">Wenn true wird das Bit zum Bank umschalten ignoriert</param>
        public void Set(byte Data, int Position, bool IgnoreBankBit)
        {
            if (!IgnoreBankBit)
                if (IsBank1())
                    Position += 0x80;

            Position = Mapping[Position];
            switch (Position)
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
        /// <param name="Position">Adresse des Registers</param>
        /// <returns>Register Daten</returns>
        public byte Get(int Position)
        {
            return Get(Position, false);
        }
        /// <summary>
        /// Gibt ein Register zurück
        /// </summary>
        /// <param name="Position">Adresse des Registers</param>
        /// <param name="IgnoreBankBit">Wenn true wird das Bit zum Umschalten der Bank ignoriert</param>
        /// <returns></returns>
        public byte Get(int Position, bool IgnoreBankBit)
        {
            if (!IgnoreBankBit)
                if (IsBank1())
                    Position += 0x80;
            Position = Mapping[Position];
            return Data[Position].Value;
        }

        /// <summary>
        /// Gibt ein Adapter für ein Register zurück
        /// </summary>
        /// <param name="Position">Adresse des Adapters</param>
        /// <returns></returns>
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
