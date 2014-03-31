using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    class BindTextBoxHex : TextBox
    {
        DataAdapter<byte> Adapter;

        public BindTextBoxHex()
        {
            KeyDown+=BindTextBoxHex_KeyDown;
        }

        public void Bind(DataAdapter<byte> Adapter)
        {
            this.Adapter = Adapter;
            Adapter.DataChanged += Adapter_DataChanged;
            Adapter_DataChanged(Adapter.Value, null);
        }

        private void BindTextBoxHex_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                try
                {
                    int Temp = Convert.ToInt32(Text, 16);
                    Adapter.Value = (byte)Temp;
                }
                catch
                {
                    Adapter_DataChanged(Adapter.Value, null);
                }
            }
        }

        private void Adapter_DataChanged(byte Value, object Sender)
        {
            Text = Value.ToString("X2");
        }
    }
}
