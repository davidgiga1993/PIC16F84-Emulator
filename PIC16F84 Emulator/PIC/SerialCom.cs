using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;

namespace PIC16F84_Emulator.PIC
{
    public class SerialCom
    {
        public bool Active
        {
            get
            {
                return _Active;
            }
        }
        private PIC Pic;
        private bool _Active = false;
        private SerialPort Port;

        public SerialCom(PIC Pic)
        {
            this.Pic = Pic;
        }

        /// <summary>
        /// Gibt alle verfügbaren Ports zurück
        /// </summary>
        public string[] Ports
        {
            get
            {
                return SerialPort.GetPortNames();
            }
        }

        public void Start(string Portname)
        {
            
            Port = new SerialPort(Portname, 4800, Parity.None, 8, StopBits.One);
            if (Port.IsOpen)
                return;

            _Active = true;
            Port.Open();
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_A).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_B).DataChanged += SerialCom_DataChanged;
        }

        private void SerialCom_DataChanged(byte Value, object Sender)
        {
            byte[] Data = BuildPacket();
            if(Port != null && Port.IsOpen)
            {
                Port.Write(Data, 0, Data.Length);
            }
        }

        public void Stop()
        {
            _Active = false;
            if(Port.IsOpen)
                Port.Close();

            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).DataChanged -= SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).DataChanged -= SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_A).DataChanged -= SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_B).DataChanged -= SerialCom_DataChanged;
        }

        private void ReceiveLoop()
        {

        }

        private byte[] BuildPacket()
        {
            byte[] Packet = new byte[9];

            byte Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_A).Value;
            Packet[0] = (byte)((Value >> 4) + 0x30); // Oberes Halbbit
            Packet[1] = (byte)((Value & 0x15) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).Value;
            Packet[2] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[3] = (byte)((Value & 0x15) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_B).Value;
            Packet[4] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[5] = (byte)((Value & 0x15) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).Value;
            Packet[6] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[7] = (byte)((Value & 0x15) + 0x30); // Unteres Halbbit
            Packet[8] = 0xD; // CR

            return Packet;
        }
    }
}
