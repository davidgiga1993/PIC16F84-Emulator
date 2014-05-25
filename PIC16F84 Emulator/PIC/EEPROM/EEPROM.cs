using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.PIC.EEPROM
{
    public class EEPROM
    {
        private PIC Pic;
        private DataAdapter<byte> EEAddr;
        private DataAdapter<byte> EEData;
        private DataAdapter<byte> EECon1;
        private DataAdapter<byte> EECon2;

        private byte[] Data = new byte[64];

        private int EECon2Steps = 0;
        private int WriteInAction = 0;
        

        public EEPROM(PIC Pic)
        {
            this.Pic = Pic;

            EEAddr = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_EEADDR_ADDRESS);
            EEData = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_EEDATA_ADDRESS);
            EECon1 = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_EECON1_ADDRESS);
            EECon2 = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_EECON2_ADDRESS);

            EECon1.DataChanged += EECon1_DataChanged;
            EECon2.DataChanged += EECon2_DataChanged;
        }

        public void Reset()
        {
            EECon2Steps = 0;
            WriteInAction = 0;
        }
        /// <summary>
        /// Asynchrones schreiben, soll jeden Zyklus ausgerufen werden
        /// </summary>
        public void Tick()
        {
            if(WriteInAction != 0)
            {
                WriteInAction++;
                if(WriteInAction == 4) // Zufällige Number um asynchrones Schreiben zu Simulieren
                {
                    Data[EEAddr.Value] = EEData.Value;
                    Write = false;
                    WriteComplete = true;
                    WriteInAction = 0;
                }               
            }
            if(Read)
            {
                Read = false;
            }
        }

        /// <summary>
        /// EECON 2 Register hat sich geändert
        /// </summary>
        /// <param name="Value"></param>
        /// <param name="Sender"></param>
        private void EECon2_DataChanged(byte Value, object Sender)
        {
            if (EECon2Steps == 0 && Value == 0x55)
            {
                EECon2Steps = 1;
            }
            if(EECon2Steps == 1 && Value == 0xAA)
            {
                EECon2Steps = 2;
                if(Write && WriteEnable)
                {
                    WriteInAction = 1;
                    EECon2Steps = 0;
                }
            }
        }

        private void EECon1_DataChanged(byte Value, object Sender)
        {
            if(Read)
            {
                int Index = EEAddr.Value;
                EEData.Value = Data[Index];
            }
            else if(Write)
            {
                if (WriteEnable && EECon2Steps == 2)
                {
                    WriteInAction = 1;
                }
                EECon2Steps = 0;
            }
        }
        /// <summary>
        /// True wenn Schreibvorgang abgeschlossen. Wird für das Interrupt benutzt
        /// </summary>
        public bool WriteComplete
        {
            get
            {
                return (EECon1.Value & 0x10) == 0x10;
            }
            set
            {
                if(value) // Nur setzen benötigt
                {
                    EECon1.Value = (byte)(EECon1.Value | 0x10);
                }
            }
        }

        /// <summary>
        /// RD bit. Nur zurücksetzbar im EEPROM Kontext
        /// </summary>
        private bool Read
        {
            get
            {
                return (EECon1.Value & 0x1) == 0x1;
            }
            set
            {
                if(!value) // Nur unset ist benötigt
                {
                    EECon1.Value = (byte)(EECon1.Value & 0xFE);
                }
            }
        }
        private bool Write
        {
            get
            {
                return (EECon1.Value & 0x2) == 0x2;
            }
            set
            {
                if (!value) // Nur unset ist benötigt
                {
                    EECon1.Value = (byte)(EECon1.Value & 0xFD);
                }
            }
        }
        private bool WriteEnable
        {
            get
            {
                return (EECon1.Value & 0x4) == 0x4;
            }
        }
        
    }
}
