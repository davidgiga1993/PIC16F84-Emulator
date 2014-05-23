using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PIC16F84_Emulator.PIC.Data;

namespace PIC16F84_Emulator.UIElements
{
    /// <summary>
    /// Entspricht einem TextBox UI Element mit der Funktionalität sich an eine Datenquelle zu binden
    /// Zusätzlich werden alle Daten als Hexadezimal angezeigt und können auch geparst werden
    /// </summary>
    class BindTextBoxHex : TextBox
    {
        public delegate void OnBindDoubleclick(DataAdapter<int> Adapter, string ID);
        public event OnBindDoubleclick BindDoubleclick;

        /// <summary>
        /// Wenn true ändert sich die Hintergrundfarbe für ca 1 Sekunde wenn sich der Datenadapter aktualisiert hat
        /// </summary>
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

        private DataAdapter<int> Adapter;
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

        public void Bind(DataAdapter<int> Adapter)
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
                    Adapter.Value = Temp;
                }
                catch
                {
                    Adapter_DataChanged(Adapter.Value, null);
                }
            }
        }

        private void Adapter_DataChanged(int Value, object Sender)
        {
            if (InvokeRequired)
            {
                BeginInvoke((MethodInvoker)delegate { Adapter_DataChanged(Value, Sender); });
            }
            else
            {
                Text = Value.ToString("X2");
                if (EnableChangeColor)
                {
                    BackColor = System.Drawing.Color.FromArgb(255, 170, 170);
                    ColorResetTimer.Start();
                }
            }
        }

        private void T_Tick(object sender, EventArgs e)
        {
            BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
            ColorResetTimer.Stop();
        }
    }
}
