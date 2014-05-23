using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Threading;

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
        private int BlockSend = 0;
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
            if (Port != null && Port.IsOpen)
                return;
            Port = new SerialPort(Portname, 4800, Parity.None, 8, StopBits.One);

            Port.DataReceived += Port_DataReceived;
            try
            {
                Port.Open();
                _Active = true;
            }
            catch(UnauthorizedAccessException)
            {                
                return;
            }

           
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_A).DataChanged += SerialCom_DataChanged;
            Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_B).DataChanged += SerialCom_DataChanged;
        }

        void Port_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort Port = (SerialPort)sender;
            byte[] Buffer = new byte[5];
            if (Port.BytesToRead >= Buffer.Length)
            {
                Port.Read(Buffer, 0, Buffer.Length);
                BlockSend = 2;
                if (Buffer[4] == 0xD)
                {
                    Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).Value = (byte)((Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).Value & 0xE0) | (ParseByte(Buffer, 0) & 0x1F)); // Oberen 3 bits bleiben gleich!
                    Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).Value = ParseByte(Buffer, 2);
                }
            }            
        }

        private void SerialCom_DataChanged(byte Value, object Sender)
        {
            if (BlockSend != 0)
            {
                BlockSend--;
                return;
            }

            try
            {
                byte[] Data = BuildPacket();
                if (Port != null && Port.IsOpen)
                {
                    Port.Write(Data, 0, Data.Length);
                }
            }
            catch(Exception)
            {

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

        /// <summary>
        /// Convertiert 2 byte von der Seriellen Schnittstelle zu einem byte
        /// </summary>
        /// <param name="Packet"></param>
        /// <param name="Index"></param>
        /// <returns></returns>
        private byte ParseByte(byte[] Packet, int Index)
        {
            int Temp = 0;
            Temp = (Packet[Index] - 0x30) << 4;
            Temp = Temp | (Packet[Index + 1] - 0x30);
            return (byte)Temp;
        }

        private byte[] BuildPacket()
        {
            byte[] Packet = new byte[9];

            byte Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_A).Value; // mit 0x1F verunden, da nur die ersten 6 bit gebraucht werden
            Packet[0] = (byte)((Value >> 4) + 0x30); // Oberes Halbbit
            Packet[1] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_A).Value;  // mit 0x1F verunden, da nur die ersten 6 bit gebraucht werden
            Packet[2] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[3] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_TRIS_B).Value;
            Packet[4] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[5] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit

            Value = Pic.RegisterMap.GetAdapter(Register.RegisterFileMap.REG_PORT_B).Value;
            Packet[6] = (byte)((Value >> 4) + 0x30);// Oberes Halbbit
            Packet[7] = (byte)((Value & 0xF) + 0x30); // Unteres Halbbit
            Packet[8] = 0xD; // CR

            Console.WriteLine("Serial: " + Encoding.ASCII.GetString(Packet));
            return Packet;
        }
    }
}
