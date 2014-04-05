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
        public delegate void OnBindDoubleclick(DataAdapter<byte> Adapter, string ID);
        public event OnBindDoubleclick BindDoubleclick;

        public bool EnableChangeColor
        {
            get
            {
                return _EnableChangeColor;
            }
            set
            {
                if (value)
                {
                    ColorResetTimer = new Timer();
                    ColorResetTimer.Interval = 1000;
                    ColorResetTimer.Tick += T_Tick;
                }
                else
                {
                    if (ColorResetTimer != null)
                        ColorResetTimer.Stop();
                    ColorResetTimer = null;
                }
                _EnableChangeColor = value;
            }
        }

        public string ID;

        private bool _EnableChangeColor;

        private DataAdapter<byte> Adapter;
        private Timer ColorResetTimer;

        public BindTextBoxHex()
        {
            KeyDown+=BindTextBoxHex_KeyDown;
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            if (BindDoubleclick != null)
                BindDoubleclick(Adapter, ID);
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
            if(EnableChangeColor)
            {
                BackColor = System.Drawing.Color.FromArgb(255, 170, 170);
                ColorResetTimer.Start();
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            ColorResetTimer.Stop();
        }
    }
}
